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

public sealed class MySqlHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal MySqlHandle(nint handle) : base(true)
    {
        SetHandle(handle);
    }
    protected override bool ReleaseHandle()
    {
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
    private bool _isConnected;

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
        IntPtr pMysql = NativeMySql.mysql_init(IntPtr.Zero);
        if (pMysql == IntPtr.Zero)
        {
            throw new Exception("Unable to allocate MYSQL handle via mysql_init.");
        }

        return new MySql(new MySqlHandle(pMysql));
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
    public MySql Open(string host, uint port, string user, string password, string database)
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
                    clientflag: 0);
            }
        }

        if (connected == IntPtr.Zero)
        {
            _isConnected = false;
            Dispose();
            return null!;
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

    private void EnsureNotDisposed()
    {
        if (_handle.DangerousGetHandle() == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(MySql));
        }
    }

    /// <summary>
    /// Closes the native connection handle and releases unmanaged resources.
    /// </summary>
    public void Dispose() => _handle.Dispose();
}