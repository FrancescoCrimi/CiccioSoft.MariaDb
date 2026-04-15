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
        NativeMysql.mysql_close(handle);
        return true;
    }
}


public sealed unsafe class MySql : IDisposable
{
    private readonly MySqlHandle _handle;

    private MySql(MySqlHandle handle)
    {
        _handle = handle;
    }

    public static MySql Open(string host, uint port, string user, string password, string database)
    {
        nint pMySql = NativeMysql.mysql_init(nint.Zero);
        if (pMySql == nint.Zero)
        {
            throw new MySqlInteropException("Unable to allocate MYSQL handle via mysql_init.");
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
                connected = NativeMysql.mysql_real_connect(
                    pMySql,
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
            string error = GetLastError(pMySql);
            NativeMysql.mysql_close(pMySql);
            throw new MySqlInteropException($"mysql_real_connect failed: {error}");
        }
        return new MySql(new MySqlHandle(pMySql));
    }



    public void Ping()
    {
        EnsureNotDisposed();
        int result = NativeMysql.mysql_ping(_handle.DangerousGetHandle());
        if (result != 0)
        {
            throw new MySqlInteropException($"mysql_ping failed: {GetLastError(_handle.DangerousGetHandle())}");
        }
    }

    public string? GetClientInfo()
    {
        var pText = NativeMysql.mysql_get_client_info();
        return Marshal.PtrToStringUTF8((nint)pText);
    }

    public string? GetServerInfo()
    {
        var pText = NativeMysql.mysql_get_server_info(_handle.DangerousGetHandle());
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
        byte* ptr = NativeMysql.mysql_error(handle);
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


    public void Dispose()
    {
        throw new NotImplementedException();
    }
}