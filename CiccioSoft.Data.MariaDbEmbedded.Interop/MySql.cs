// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;
using System.Text;
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
    /// Use <see cref="Open(string,uint,string,string,string)"/> to actually connect.
    /// </summary>
    /// <returns>An initialized (but not connected) <see cref="MySql"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when native initialization fails.</exception>
    public static MySql Init()
    {
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
    public int SetOption(MySqlOption option, string value)
    {
        EnsureNotDisposed();
        byte[] valueBytes = Utils.BuildUtf8NullTerminated(value);

        unsafe
        {
            fixed (byte* pvalue = valueBytes)
            {
                return MySqlNative.mysql_options(_handle.DangerousGetHandle(), option, pvalue);
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
            return MySqlNative.mysql_options(_handle.DangerousGetHandle(), option, (byte*)&localValue);
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
    /// Seleziona il database corrente.
    /// Corrisponde a <c>mysql_select_db</c>.
    /// </summary>
    public void SelectDb(string db)
    {
        EnsureNotDisposed();
        byte[] bytes = Utils.BuildUtf8NullTerminated(db);

        fixed (byte* ptr = bytes)
        {
            if (MySqlNative.mysql_select_db(_handle.DangerousGetHandle(), ptr) != 0)
                throw MySqlException.FromHandle(_handle.DangerousGetHandle());
        }
    }

    /// <summary>
    /// Checks if the server connection is alive by calling <c>mysql_ping</c>.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    public int Ping()
    {
        EnsureNotDisposed();
        return MySqlNative.mysql_ping(_handle.DangerousGetHandle());
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
                return MySqlNative.mysql_query(_handle.DangerousGetHandle(), psql);
            }
        }
    }


    #region Result Set

    /// <summary>
    /// Recupera l'intero result set in memoria client dopo una query SELECT.
    /// Corrisponde a <c>mysql_store_result</c>.
    /// Restituisce <c>null</c> se la query non produce righe (es. INSERT).
    /// </summary>
    public MySqlResult? StoreResult()
    {
        EnsureNotDisposed();

        nint ptr = MySqlNative.mysql_store_result(_handle.DangerousGetHandle());
        if (ptr == 0)
        {
            // se c'è un errore reale, lancialo
            uint err = MySqlNative.mysql_errno(_handle.DangerousGetHandle());
            if (err != 0)
                throw MySqlException.FromHandle(_handle.DangerousGetHandle());
            return null; // query senza result set (INSERT, UPDATE…)
        }

        return new MySqlResult(new MySqlResultHandle(ptr));
    }

    /// <summary>
    /// Avvia il recupero streaming del result set (una riga per volta).
    /// Corrisponde a <c>mysql_use_result</c>.
    /// </summary>
    public MySqlResult? UseResult()
    {
        EnsureNotDisposed();

        nint ptr = MySqlNative.mysql_use_result(_handle.DangerousGetHandle());
        if (ptr == 0)
        {
            // se c'è un errore reale, lancialo
            uint err = MySqlNative.mysql_errno(_handle.DangerousGetHandle());
            if (err != 0)
                throw MySqlException.FromHandle(_handle.DangerousGetHandle());
            return null; // query senza result set (INSERT, UPDATE…)
        }

        return new MySqlResult(new MySqlResultHandle(ptr));
    }

    #endregion


    #region Stato post-query

    /// <summary>
    /// Numero di righe modificate dall'ultima query DML. Corrisponde a <c>mysql_affected_rows</c>.
    /// </summary>
    public ulong AffectedRows()
    {
        EnsureNotDisposed();
        return MySqlNative.mysql_affected_rows(_handle.DangerousGetHandle());
    }

    /// <summary>
    /// Ultimo valore AUTO_INCREMENT generato. Corrisponde a <c>mysql_insert_id</c>.
    /// </summary>
    public ulong InsertId()
    {
        EnsureNotDisposed();
        return MySqlNative.mysql_insert_id(_handle.DangerousGetHandle());
    }

    /// <summary>
    /// Numero di warning generati dall'ultima query. Corrisponde a <c>mysql_warning_count</c>.
    /// </summary>
    public uint WarningCount()
    {
        EnsureNotDisposed();
        return MySqlNative.mysql_warning_count(_handle.DangerousGetHandle());
    }

    /// <summary>
    /// Stringa informativa sull'ultima query (es. "Rows matched: 3"). Corrisponde a <c>mysql_info</c>.
    /// </summary>
    public string Info()
    {
        EnsureNotDisposed();
        byte* pBytes = MySqlNative.mysql_info(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(pBytes);
    }

    /// <summary>
    /// Stringa di stato del server. Corrisponde a <c>mysql_stat</c>.
    /// </summary>
    public string Stat()
    {
        EnsureNotDisposed();
        byte* pBytes = MySqlNative.mysql_stat(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(pBytes);
    }

    /// <summary>
    /// ID del thread di connessione sul server. Corrisponde a <c>mysql_thread_id</c>.
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
    /// Versione numerica della libreria client. Corrisponde a <c>mysql_get_client_version</c>.
    /// </summary>
    /// <returns>The client library version as a numeric value.</returns>
    public static uint GetClientVersion()
        => MySqlNative.mysql_get_client_version();

    #endregion


    #region Transazioni

    /// <summary>
    /// Abilita o disabilita l'autocommit.
    /// Corrisponde a <c>mysql_autocommit</c>.
    /// </summary>
    public void AutoCommit(bool enabled)
    {
        EnsureNotDisposed();
        MySqlNative.mysql_autocommit(
            _handle.DangerousGetHandle(),
            enabled ? (sbyte)1 : (sbyte)0);
    }

    /// <summary>
    /// Esegue il commit della transazione corrente. Corrisponde a <c>mysql_commit</c>.
    /// </summary>
    public void Commit()
    {
        EnsureNotDisposed();
        if (MySqlNative.mysql_commit(_handle.DangerousGetHandle()) != 0)
            throw MySqlException.FromHandle(_handle.DangerousGetHandle());
    }

    /// <summary>
    /// Esegue il rollback della transazione corrente. Corrisponde a <c>mysql_rollback</c>.
    /// </summary>
    public void Rollback()
    {
        EnsureNotDisposed();
        if (MySqlNative.mysql_rollback(_handle.DangerousGetHandle()) != 0)
            throw MySqlException.FromHandle(_handle.DangerousGetHandle());
    }

    #endregion


    /// <summary>
    /// Effettua l'escape di una stringa per uso sicuro in una query.
    /// Corrisponde a <c>mysql_real_escape_string</c>.
    /// </summary>
    public string RealEscapeString(string input)
    {
        EnsureNotDisposed();
        byte[] from = Encoding.UTF8.GetBytes(input);
        // Il buffer di destinazione deve essere lungo almeno length*2+1
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
    /// Verifica se ci sono altri result set da leggere.
    /// Corrisponde a <c>mysql_more_results</c>.
    /// </summary>
    public bool MoreResults()
    {
        EnsureNotDisposed();
        return MySqlNative.mysql_more_results(_handle.DangerousGetHandle()) != 0;
    }

    /// <summary>
    /// Avanza al prossimo result set in una query multi-statement.
    /// Corrisponde a <c>mysql_next_result</c>.
    /// Restituisce <c>false</c> quando non ci sono altri result set.
    /// </summary>
    public bool NextResult()
    {
        EnsureNotDisposed();
        int rc = MySqlNative.mysql_next_result(_handle.DangerousGetHandle());
        // if (rc > 0) ThrowLastError();
        return rc == 0;
    }

    #endregion


    /// <summary>
    /// Alloca e restituisce un nuovo <see cref="MysqlStmt"/>.
    /// Corrisponde a <c>mysql_stmt_init</c>.
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