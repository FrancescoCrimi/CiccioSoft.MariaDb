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
    /// <exception cref="MySqlInteropException">Thrown when native initialization fails.</exception>
    public static MySql Init()
    {
        IntPtr pMysql = NativeMySql.mysql_init(IntPtr.Zero);
        if (pMysql == IntPtr.Zero)
        {
            throw new MySqlInteropException("Unable to allocate MYSQL handle via mysql_init.");
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
    /// <exception cref="MySqlInteropException">Thrown when native connection fails.</exception>
    public void Open(string host, uint port, string user, string password, string database)
    {
        EnsureNotDisposed();
        if (_isConnected)
        {
            throw new InvalidOperationException("Connection is already open.");
        }

        byte[] hostBytes = BuildUtf8NullTerminated(host);
        byte[] userBytes = BuildUtf8NullTerminated(user);
        byte[] passwordBytes = BuildUtf8NullTerminated(password);
        byte[] databaseBytes = BuildUtf8NullTerminated(database);

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
            string error = GetLastError(_handle.DangerousGetHandle());
            NativeMySql.mysql_close(_handle.DangerousGetHandle());
            throw new MySqlInteropException($"mysql_real_connect failed: {error}");
        }

        _isConnected = true;
    }

    /// <summary>
    /// Sets a string option on the current connection handle via <c>mysql_options</c>.
    /// </summary>
    /// <param name="option">Native option key to configure.</param>
    /// <param name="value">Option value encoded as UTF-8 and passed as null-terminated string.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    /// <exception cref="MySqlInteropException">Thrown when native call returns an error code.</exception>
    public void SetOption(MySqlOption option, string value)
    {
        EnsureNotDisposed();
        byte[] valueBytes = BuildUtf8NullTerminated(value);

        unsafe
        {
            fixed (byte* pvalue = valueBytes)
            {
                int result = NativeMySql.mysql_options(_handle.DangerousGetHandle(), option, pvalue);
                ThrowIfError(result, "mysql_options");
            }
        }
    }

    /// <summary>
    /// Sets a numeric option on the current connection handle via <c>mysql_options</c>.
    /// </summary>
    /// <param name="option">Native option key to configure.</param>
    /// <param name="value">Unsigned numeric option value.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    /// <exception cref="MySqlInteropException">Thrown when native call returns an error code.</exception>
    public void SetOption(MySqlOption option, uint value)
    {
        EnsureNotDisposed();

        unsafe
        {
            uint localValue = value;
            int result = NativeMySql.mysql_options(_handle.DangerousGetHandle(), option, (byte*)&localValue);
            ThrowIfError(result, "mysql_options");
        }
    }

    /// <summary>
    /// Sets a boolean option on the current connection handle via <c>mysql_options</c>.
    /// </summary>
    /// <param name="option">Native option key to configure.</param>
    /// <param name="enabled"><see langword="true"/> to enable the option; otherwise <see langword="false"/>.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
    /// <exception cref="MySqlInteropException">Thrown when native call returns an error code.</exception>
    public void SetOption(MySqlOption option, bool enabled)
    {
        SetOption(option, enabled ? 1u : 0u);
    }

    public void Ping()
    {
        EnsureNotDisposed();
        int result = NativeMySql.mysql_ping(_handle.DangerousGetHandle());
        if (result != 0)
        {
            throw new MySqlInteropException($"mysql_ping failed: {GetLastError(_handle.DangerousGetHandle())}");
        }
    }

    public string? GetClientInfo()
    {
        var pText = NativeMySql.mysql_get_client_info();
        return Marshal.PtrToStringUTF8((nint)pText);
    }

    public string? GetServerInfo()
    {
        var pText = NativeMySql.mysql_get_server_info(_handle.DangerousGetHandle());
        return Marshal.PtrToStringUTF8((nint)pText);
    }

    private void EnsureNotDisposed()
    {
        if (_handle.DangerousGetHandle() == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(MySql));
        }
    }

    private static string GetLastError(IntPtr handle)
    {
        byte* ptr = NativeMySql.mysql_error(handle);
        return ptr == null
            ? "unknown error"
            : Marshal.PtrToStringUTF8((nint)ptr) ?? "unknown error";
    }

    private static byte[] BuildUtf8NullTerminated(string value)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
        byte[] nullTerminated = new byte[bytes.Length + 1];
        bytes.CopyTo(nullTerminated, 0);
        return nullTerminated;
    }

    private void ThrowIfError(int result, string operationName)
    {
        if (result != 0)
        {
            throw new MySqlInteropException($"{operationName} failed: {GetLastError(_handle.DangerousGetHandle())}");
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}