// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;
using System.Text;
using CiccioSoft.Interop.MariaDb.Native;
using Microsoft.Win32.SafeHandles;

namespace CiccioSoft.Interop.MariaDb;

internal sealed class MySqlHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal MySqlHandle(nint ptr) : base(true)
    {
        SetHandle(ptr);
    }

    protected override bool ReleaseHandle()
    {
        if (handle != 0)
            MySqlNative.mysql_close(handle);
        return true;
    }
}


/// <summary>
/// Thin idiomatic OOP wrapper around a native <c>MYSQL*</c> connection handle.
/// </summary>
public sealed unsafe class MySql : IDisposable
{
    private readonly MySqlHandle _handle;
    private bool _isConnected = false;

    public SafeHandle Handle => _handle;

    private MySql(MySqlHandle handle)
    {
        _handle = handle;
    }

    /// <summary>
    /// Allocates and initializes a connection handle via <c>mysql_init</c>.
    /// Use <see cref="Connect(string,uint,string,string,string,uint)"/> to actually connect.
    /// </summary>
    /// <returns>An initialized (but not connected) <see cref="MySql"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when native initialization fails.</exception>
    public static MySql Init()
    {
        MySqlLibrary.EnsureInitialized();

        nint ptr = MySqlNative.mysql_init(nint.Zero);
        if (ptr == nint.Zero)
        {
            throw new InvalidOperationException("Failed to allocate a MYSQL handle via mysql_init.");
        }

        return new MySql(new MySqlHandle(ptr));
    }


    #region Options (mysql_options)

    /// <summary>
    /// Sets a string option on the current connection handle via <c>mysql_options</c>.
    /// </summary>
    /// <param name="option">Native option key to configure.</param>
    /// <param name="value">Option value encoded as UTF-8 and passed as null-terminated string.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public void SetOption(MySqlOption option, string value)
    {
        EnsureNotDisposed();
        ReadOnlySpan<byte> valueBytes = Utils.BuildUtf8NullTerminated(value);

        unsafe
        {
            fixed (byte* pvalue = valueBytes)
            {
                if (MySqlNative.mysql_options(_handle.DangerousGetHandle(), option, pvalue) != 0)
                    ThrowError();
            }
        }
    }

    /// <summary>
    /// Sets a numeric option on the current connection handle via <c>mysql_options</c>.
    /// </summary>
    /// <param name="option">Native option key to configure.</param>
    /// <param name="value">Unsigned numeric option value.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public void SetOption(MySqlOption option, uint value)
    {
        EnsureNotDisposed();

        unsafe
        {
            uint localValue = value;
            if (MySqlNative.mysql_options(_handle.DangerousGetHandle(), option, (byte*)&localValue) != 0)
                ThrowError();
        }
    }

    /// <summary>
    /// Sets a boolean option on the current connection handle via <c>mysql_options</c>.
    /// </summary>
    /// <param name="option">Native option key to configure.</param>
    /// <param name="enabled"><see langword="true"/> to enable the option; otherwise <see langword="false"/>.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public void SetOption(MySqlOption option, bool enabled)
    {
        SetOption(option, enabled ? 1u : 0u);
    }

    #endregion


    /// <summary>
    /// Opens the current initialized handle using <c>mysql_real_connect</c>.
    /// </summary>
    /// <param name="host">Server host name or IP address.</param>
    /// <param name="port">Server TCP port.</param>
    /// <param name="user">User name used to authenticate.</param>
    /// <param name="password">Password used to authenticate.</param>
    /// <param name="database">Default schema name selected after connecting.</param>
    /// <param name="clientFlag">Client capability flags passed to <c>mysql_real_connect</c>.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    /// <exception cref="InvalidOperationException">Thrown when this instance is already connected.</exception>
    public MySql Connect(string host, uint port, string user, string password, string database, uint clientFlag = 0)
    {
        EnsureNotDisposed();
        if (_isConnected)
        {
            throw new InvalidOperationException("Connection is already open.");
        }

        ReadOnlySpan<byte> hostBytes = Utils.BuildUtf8NullTerminated(host);
        ReadOnlySpan<byte> userBytes = Utils.BuildUtf8NullTerminated(user);
        ReadOnlySpan<byte> passwordBytes = Utils.BuildUtf8NullTerminated(password);
        ReadOnlySpan<byte> databaseBytes = Utils.BuildUtf8NullTerminated(database);

        IntPtr connected;
        unsafe
        {
            fixed (byte* phost = hostBytes)
            fixed (byte* puser = userBytes)
            fixed (byte* ppassword = passwordBytes)
            fixed (byte* pdatabase = databaseBytes)
            {
                connected = MySqlNative.mysql_real_connect(
                    _handle.DangerousGetHandle(),
                    phost,
                    puser,
                    ppassword,
                    pdatabase,
                    port,
                    unix_socket: (byte*)IntPtr.Zero,
                    clientflag: clientFlag);
            }
        }

        if (connected == IntPtr.Zero)
        {
            // Read the error before calling Dispose(), which invokes mysql_close.
            byte* pErr = MySqlNative.mysql_error(_handle.DangerousGetHandle());
            uint errno = MySqlNative.mysql_errno(_handle.DangerousGetHandle());
            string msg = Utils.GetStringFromPointerBytes(pErr);
            Dispose();
            throw new MySqlException(msg, (int)errno);
        }
        else
        {
            _isConnected = true;
            return this;
        }
    }

    /// <summary>
    /// Selects the current default database.
    /// Maps to <c>mysql_select_db</c>.
    /// </summary>
    public void SelectDb(string db)
    {
        EnsureNotDisposed();
        ReadOnlySpan<byte> bytes = Utils.BuildUtf8NullTerminated(db);

        fixed (byte* ptr = bytes)
        {
            if (MySqlNative.mysql_select_db(_handle.DangerousGetHandle(), ptr) != 0)
                ThrowError();
        }
    }

    /// <summary>
    /// Checks if the server connection is alive by calling <c>mysql_ping</c>.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public void Ping()
    {
        EnsureNotDisposed();
        if (MySqlNative.mysql_ping(_handle.DangerousGetHandle()) != 0)
            ThrowError();
    }

    /// <summary>
    /// Executes a SQL statement using the native <c>mysql_query</c> API.
    /// </summary>
    /// <param name="sql">SQL command text to execute.</param>
    /// <returns>Native <c>mysql_query</c> result code (zero when successful).</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public void Query(string sql)
    {
        EnsureNotDisposed();
        ReadOnlySpan<byte> queryBytes = Utils.BuildUtf8NullTerminated(sql);

        unsafe
        {
            fixed (byte* psql = queryBytes)
            {
                if (MySqlNative.mysql_query(_handle.DangerousGetHandle(), psql) != 0)
                    ThrowError();
            }
        }
    }


    #region Result Set

    /// <summary>
    /// Retrieves the full result set into client memory after a SELECT query.
    /// Maps to <c>mysql_store_result</c>.
    /// Returns <c>null</c> when the statement does not produce a result set (for example INSERT).
    /// </summary>
    public MySqlResult? StoreResult()
    {
        EnsureNotDisposed();

        nint ptr = MySqlNative.mysql_store_result(_handle.DangerousGetHandle());
        if (ptr == 0)
        {
            // If there is a real error, throw it
            if (MySqlNative.mysql_errno(_handle.DangerousGetHandle()) != 0)
                ThrowError();
            return null; // Statement without a result set (INSERT, UPDATE, ...)
        }

        return new MySqlResult(new MySqlResultHandle(ptr));
    }

    /// <summary>
    /// Starts streaming retrieval of the result set (one row at a time).
    /// Maps to <c>mysql_use_result</c>.
    /// </summary>
    public MySqlResult? UseResult()
    {
        EnsureNotDisposed();

        nint ptr = MySqlNative.mysql_use_result(_handle.DangerousGetHandle());
        if (ptr == 0)
        {
            // If there is a real error, throw it
            if (MySqlNative.mysql_errno(_handle.DangerousGetHandle()) != 0)
                ThrowError();
            return null; // Statement without a result set (INSERT, UPDATE, ...)
        }

        return new MySqlResult(new MySqlResultHandle(ptr));
    }

    #endregion


    #region Stato post-query

    /// <summary>
    /// Number of rows changed by the last DML statement. Maps to <c>mysql_affected_rows</c>.
    /// </summary>
    public ulong AffectedRows()
    {
        EnsureNotDisposed();
        return MySqlNative.mysql_affected_rows(_handle.DangerousGetHandle());
    }

    /// <summary>
    /// Last generated AUTO_INCREMENT value. Maps to <c>mysql_insert_id</c>.
    /// </summary>
    public ulong InsertId()
    {
        EnsureNotDisposed();
        return MySqlNative.mysql_insert_id(_handle.DangerousGetHandle());
    }

    /// <summary>
    /// Number of warnings generated by the last statement. Maps to <c>mysql_warning_count</c>.
    /// </summary>
    public uint WarningCount()
    {
        EnsureNotDisposed();
        return MySqlNative.mysql_warning_count(_handle.DangerousGetHandle());
    }

    /// <summary>
    /// Informational string about the last statement (for example "Rows matched: 3"). Maps to <c>mysql_info</c>.
    /// </summary>
    public string Info()
    {
        EnsureNotDisposed();
        byte* pBytes = MySqlNative.mysql_info(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(pBytes);
    }

    /// <summary>
    /// Server status string. Maps to <c>mysql_stat</c>.
    /// </summary>
    public string Stat()
    {
        EnsureNotDisposed();
        byte* pBytes = MySqlNative.mysql_stat(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(pBytes);
    }

    /// <summary>
    /// Server-side connection thread identifier. Maps to <c>mysql_thread_id</c>.
    /// </summary>
    public uint ThreadId()
    {
        EnsureNotDisposed();
        return MySqlNative.mysql_thread_id(_handle.DangerousGetHandle());
    }

    #endregion


    /// <summary>
    /// Gets the last error message associated with the current connection handle.
    /// </summary>
    /// <returns>Native error text; or <c>unknown error</c> if unavailable.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public string Error()
    {
        EnsureNotDisposed();
        byte* pBytes = MySqlNative.mysql_error(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(pBytes);
    }


    #region Metadati server

    /// <summary>
    /// Gets the server version string for the current connection via <c>mysql_get_server_info</c>.
    /// </summary>
    /// <returns>The server version string; or <see langword="null"/> if unavailable.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public string GetServerInfo()
    {
        EnsureNotDisposed();
        byte* pBytes = MySqlNative.mysql_get_server_info(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(pBytes);
    }

    /// <summary>
    /// Gets the host information for the current connection via <c>mysql_get_host_info</c>.
    /// </summary>
    /// <returns>The host information string; or <see langword="null"/> if unavailable.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public string GetHostInfo()
    {
        EnsureNotDisposed();
        byte* pBytes = MySqlNative.mysql_get_host_info(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(pBytes);
    }

    /// <summary>
    /// Gets the server version as a numeric value (e.g., 100612). Corresponds to <c>mysql_get_server_version</c>.
    /// </summary>
    /// <returns>The server version as a numeric value.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public uint GetServerVersion()
    {
        EnsureNotDisposed();
        return MySqlNative.mysql_get_server_version(_handle.DangerousGetHandle());
    }

    /// <summary>
    /// Gets the protocol version as a numeric value (e.g., 10). Corresponds to <c>mysql_get_proto_info</c>.
    /// </summary>
    /// <returns>The protocol version as a numeric value.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public uint GetProtoInfo()
    {
        EnsureNotDisposed();
        return MySqlNative.mysql_get_proto_info(_handle.DangerousGetHandle());
    }

    /// <summary>
    /// Gets the version string for the loaded client library via <c>mysql_get_client_info</c>.
    /// </summary>
    /// <returns>The client library version string; or <see langword="null"/> if unavailable.</returns>
    public static string GetClientInfo()
    {
        byte* pBytes = MySqlNative.mysql_get_client_info();
        return Utils.GetStringFromPointerBytes(pBytes);
    }

    /// <summary>
    /// Numeric client library version. Maps to <c>mysql_get_client_version</c>.
    /// </summary>
    /// <returns>The client library version as a numeric value.</returns>
    public static uint GetClientVersion()
        => MySqlNative.mysql_get_client_version();

    #endregion


    #region Transazioni

    /// <summary>
    /// Enables or disables autocommit mode.
    /// Maps to <c>mysql_autocommit</c>.
    /// </summary>
    public void AutoCommit(bool enabled)
    {
        EnsureNotDisposed();
        if (MySqlNative.mysql_autocommit(_handle.DangerousGetHandle(), enabled ? (sbyte)1 : (sbyte)0) != 0)
            ThrowError();
    }

    /// <summary>
    /// Commits the current transaction. Maps to <c>mysql_commit</c>.
    /// </summary>
    public void Commit()
    {
        EnsureNotDisposed();
        if (MySqlNative.mysql_commit(_handle.DangerousGetHandle()) != 0)
            ThrowError();
    }

    /// <summary>
    /// Rolls back the current transaction. Maps to <c>mysql_rollback</c>.
    /// </summary>
    public void Rollback()
    {
        EnsureNotDisposed();
        if (MySqlNative.mysql_rollback(_handle.DangerousGetHandle()) != 0)
            ThrowError();
    }

    #endregion


    /// <summary>
    /// Escapes a string for safe use in SQL text.
    /// Maps to <c>mysql_real_escape_string</c>.
    /// </summary>
    public string RealEscapeString(string input)
    {
        EnsureNotDisposed();
        byte[] from = Encoding.UTF8.GetBytes(input);
        // Destination buffer must be at least length*2+1 bytes
        byte[] to = new byte[from.Length * 2 + 1];
        fixed (byte* pFrom = from)
        fixed (byte* pTo = to)
        {
            nuint outLen = MySqlNative.mysql_real_escape_string(_handle.DangerousGetHandle(), pTo, pFrom, (uint)from.Length);
            return Encoding.UTF8.GetString(to, 0, (int)outLen);
        }
        // return to;
    }


    #region Multi-statement

    /// <summary>
    /// Checks whether additional result sets are available.
    /// Maps to <c>mysql_more_results</c>.
    /// </summary>
    public bool MoreResults()
    {
        EnsureNotDisposed();
        return MySqlNative.mysql_more_results(_handle.DangerousGetHandle()) != 0;
    }

    /// <summary>
    /// Advances to the next result set in a multi-statement execution.
    /// Maps to <c>mysql_next_result</c>.
    /// Returns <c>false</c> when no more result sets are available.
    /// </summary>
    public bool NextResult()
    {
        EnsureNotDisposed();
        int rc = MySqlNative.mysql_next_result(_handle.DangerousGetHandle());
        if (rc > 0) ThrowError();
        return rc == 0;
    }

    #endregion


    /// <summary>
    /// Allocates and returns a new <see cref="MySqlStmt"/> wrapper.
    /// Maps to <c>mysql_stmt_init</c>.
    /// </summary>
    public MySqlStmt StmtInit()
    {
        EnsureNotDisposed();

        nint ptr = MariadbStmtNative.mysql_stmt_init(_handle.DangerousGetHandle());
        if (ptr == 0)
            throw new OutOfMemoryException("mysql_stmt_init failed.");


        return new MySqlStmt(new MySqlStmtHandle(ptr));
    }


    #region Helper

    private void EnsureNotDisposed()
    {
        if (_handle.IsClosed || _handle.IsInvalid)
        {
            throw new ObjectDisposedException(nameof(MySql));
        }
    }

    private void ThrowError()
    {
        byte* pMsg = MySqlNative.mysql_error(_handle.DangerousGetHandle());
        byte* pState = MySqlNative.mysql_sqlstate(_handle.DangerousGetHandle());
        uint errno = MySqlNative.mysql_errno(_handle.DangerousGetHandle());
        throw new MySqlException(
            Utils.GetStringFromPointerBytes(pMsg),
            (int)errno,
            Utils.GetStringFromPointerBytes(pState));
    }

    #endregion


    /// <summary>
    /// Closes the native connection handle and releases unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _handle.Dispose();
        GC.SuppressFinalize(this);
    }
}