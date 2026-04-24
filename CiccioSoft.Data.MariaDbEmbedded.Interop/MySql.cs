// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;
using Microsoft.Win32.SafeHandles;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

internal sealed class MySqlHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal MySqlHandle(nint ptr) : base(true)
    {
        SetHandle(ptr);
    }

    protected override bool ReleaseHandle()
    {
        if (handle != 0)
            NativeMySql.mysql_close(handle);
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
    /// Use <see cref="Open(string,uint,string,string,string)"/> to actually connect.
    /// </summary>
    /// <returns>An initialized (but not connected) <see cref="MySql"/> instance.</returns>
    /// <exception cref="Exception">Thrown when native initialization fails.</exception>
    public static MySql Init()
    {
        nint ptr = NativeMySql.mysql_init(nint.Zero);
        if (ptr == nint.Zero)
        {
            throw new Exception("Unable to allocate MYSQL handle via mysql_init.");
        }

        return new MySql(new MySqlHandle(ptr));
    }

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

        byte[] hostBytes = Utils.BuildUtf8NullTerminated(host);
        byte[] userBytes = Utils.BuildUtf8NullTerminated(user);
        byte[] passwordBytes = Utils.BuildUtf8NullTerminated(password);
        byte[] databaseBytes = Utils.BuildUtf8NullTerminated(database);

        IntPtr connected;
        unsafe
        {
            fixed (byte* phost = hostBytes)
            fixed (byte* puser = userBytes)
            fixed (byte* ppassword = passwordBytes)
            fixed (byte* pdatabase = databaseBytes)
            {
                connected = NativeMySql.mysql_real_connect(
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
            // leggi l'errore PRIMA di Dispose, che chiama mysql_close
            byte* pErr = NativeMySql.mysql_error(_handle.DangerousGetHandle());
            uint errno = NativeMySql.mysql_errno(_handle.DangerousGetHandle());
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
    /// Sets a string option on the current connection handle via <c>mysql_options</c>.
    /// </summary>
    /// <param name="option">Native option key to configure.</param>
    /// <param name="value">Option value encoded as UTF-8 and passed as null-terminated string.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public int SetOption(MySqlOption option, string value)
    {
        EnsureNotDisposed();
        byte[] valueBytes = Utils.BuildUtf8NullTerminated(value);

        unsafe
        {
            fixed (byte* pvalue = valueBytes)
            {
                return NativeMySql.mysql_options(_handle.DangerousGetHandle(), option, pvalue);
            }
        }
    }

    /// <summary>
    /// Sets a numeric option on the current connection handle via <c>mysql_options</c>.
    /// </summary>
    /// <param name="option">Native option key to configure.</param>
    /// <param name="value">Unsigned numeric option value.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public int SetOption(MySqlOption option, uint value)
    {
        EnsureNotDisposed();

        unsafe
        {
            uint localValue = value;
            return NativeMySql.mysql_options(_handle.DangerousGetHandle(), option, (byte*)&localValue);
        }
    }

    /// <summary>
    /// Sets a boolean option on the current connection handle via <c>mysql_options</c>.
    /// </summary>
    /// <param name="option">Native option key to configure.</param>
    /// <param name="enabled"><see langword="true"/> to enable the option; otherwise <see langword="false"/>.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public int SetOption(MySqlOption option, bool enabled)
    {
        return SetOption(option, enabled ? 1u : 0u);
    }

    /// <summary>
    /// Executes a SQL statement using the native <c>mysql_query</c> API.
    /// </summary>
    /// <param name="sql">SQL command text to execute.</param>
    /// <returns>Native <c>mysql_query</c> result code (zero when successful).</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public int Query(string sql)
    {
        EnsureNotDisposed();
        byte[] queryBytes = Utils.BuildUtf8NullTerminated(sql);

        unsafe
        {
            fixed (byte* psql = queryBytes)
            {
                return NativeMySql.mysql_query(_handle.DangerousGetHandle(), psql);
            }
        }
    }

    /// <summary>
    /// Gets the last error message associated with the current connection handle.
    /// </summary>
    /// <returns>Native error text; or <c>unknown error</c> if unavailable.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public string Error()
    {
        EnsureNotDisposed();
        byte* pBytes = NativeMySql.mysql_error(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(pBytes);
    }

    /// <summary>
    /// Checks if the server connection is alive by calling <c>mysql_ping</c>.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public int Ping()
    {
        EnsureNotDisposed();
        return NativeMySql.mysql_ping(_handle.DangerousGetHandle());
    }

    /// <summary>
    /// Gets the version string for the loaded client library via <c>mysql_get_client_info</c>.
    /// </summary>
    /// <returns>The client library version string; or <see langword="null"/> if unavailable.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public string GetClientInfo()
    {
        EnsureNotDisposed();
        byte* pBytes = NativeMySql.mysql_get_client_info();
        return Utils.GetStringFromPointerBytes(pBytes);
    }

    /// <summary>
    /// Gets the server version string for the current connection via <c>mysql_get_server_info</c>.
    /// </summary>
    /// <returns>The server version string; or <see langword="null"/> if unavailable.</returns>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public string GetServerInfo()
    {
        EnsureNotDisposed();
        byte* pBytes = NativeMySql.mysql_get_server_info(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(pBytes);
    }

    public ulong AffectedRows()
    {
        EnsureNotDisposed();
        return NativeMySql.mysql_affected_rows(_handle.DangerousGetHandle());
    }

    public ulong InsertId()
    {
        EnsureNotDisposed();
        return NativeMySql.mysql_insert_id(_handle.DangerousGetHandle());
    }

    public uint WarningCount()
    {
        EnsureNotDisposed();
        return NativeMySql.mysql_warning_count(_handle.DangerousGetHandle());
    }

    public void AutoCommit(bool enabled)
    {
        EnsureNotDisposed();
        NativeMySql.mysql_autocommit(
            _handle.DangerousGetHandle(),
            enabled ? (sbyte)1 : (sbyte)0);
    }

    public void Commit()
    {
        EnsureNotDisposed();
        if (NativeMySql.mysql_commit(_handle.DangerousGetHandle()) != 0)
            throw MySqlException.FromHandle(_handle.DangerousGetHandle());
    }

    public void Rollback()
    {
        EnsureNotDisposed();
        if (NativeMySql.mysql_rollback(_handle.DangerousGetHandle()) != 0)
            throw MySqlException.FromHandle(_handle.DangerousGetHandle());
    }

    // factory che lancia eccezione invece di restituire null
    public MySqlResult? StoreResult()
    {
        EnsureNotDisposed();

        nint ptr = NativeMySql.mysql_store_result(_handle.DangerousGetHandle());
        if (ptr == 0)
        {
            // se c'è un errore reale, lancialo
            uint err = NativeMySql.mysql_errno(_handle.DangerousGetHandle());
            if (err != 0)
                throw MySqlException.FromHandle(_handle.DangerousGetHandle());
            return null; // query senza result set (INSERT, UPDATE…)
        }

        return new MySqlResult(new MySqlResultHandle(ptr));
    }

    public MySqlStatement StmtInit()
    {
        EnsureNotDisposed();

        nint ptr = NativeMariadbStmt.mysql_stmt_init(_handle.DangerousGetHandle());
        if (ptr == 0)
            throw new OutOfMemoryException("mysql_stmt_init failed.");


        return new MySqlStatement(new MySqlStatementHandle(ptr));
    }

    private void EnsureNotDisposed()
    {
        if (_handle.IsClosed || _handle.IsInvalid)
        {
            throw new ObjectDisposedException(nameof(MySql));
        }
    }

    /// <summary>
    /// Closes the native connection handle and releases unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _handle.Dispose();
        GC.SuppressFinalize(this);
    }
}