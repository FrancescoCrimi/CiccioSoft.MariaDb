// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;
using System.Text;
using CiccioSoft.Data.MariaDbEmbedded.Interop.IA.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.IA
{
    /// <summary>
    /// Thin idiomatic OOP wrapper around a native <c>MYSQL*</c> connection handle.
    /// </summary>
    public sealed class MySql : IDisposable
    {
        private IntPtr _handle;

        private MySql(IntPtr handle)
        {
            _handle = handle;
        }

        /// <summary>
        /// Allocates, initializes and opens a connection using <c>mysql_real_connect</c>.
        /// </summary>
        /// <param name="host">Server host name or IP address.</param>
        /// <param name="port">Server TCP port.</param>
        /// <param name="user">User name used to authenticate.</param>
        /// <param name="password">Password used to authenticate.</param>
        /// <param name="database">Default schema name selected after connecting.</param>
        /// <returns>A connected <see cref="MySql"/> instance.</returns>
        /// <exception cref="MySqlInteropException">Thrown when native initialization or connection fails.</exception>
        public static MySql Open(string host, uint port, string user, string password, string database)
        {
            IntPtr handle = NativeMySqlClient.mysql_init(IntPtr.Zero);
            if (handle == IntPtr.Zero)
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
                    connected = NativeMySqlClient.mysql_real_connect(
                        handle,
                        phost,
                        puser,
                        ppassword,
                        pdatabase,
                        port,
                        unix_socket: (byte*)IntPtr.Zero,
                        client_flag: 0);
                }
            }

            if (connected == IntPtr.Zero)
            {
                string error = GetLastError(handle);
                NativeMySqlClient.mysql_close(handle);
                throw new MySqlInteropException($"mysql_real_connect failed: {error}");
            }

            return new MySql(handle);
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
                    int result = NativeMySqlClient.mysql_options(_handle, option, pvalue);
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
                int result = NativeMySqlClient.mysql_options(_handle, option, (byte*)&localValue);
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

        /// <summary>
        /// Executes a SQL statement using the native <c>mysql_query</c> API.
        /// </summary>
        /// <param name="sql">SQL command text to execute.</param>
        /// <returns><see cref="MySqlResultCode.Ok"/> when successful.</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
        /// <exception cref="MySqlInteropException">Thrown when native call returns an error code.</exception>
        public MySqlResultCode Query(string sql)
        {
            EnsureNotDisposed();
            byte[] queryBytes = BuildUtf8NullTerminated(sql);

            unsafe
            {
                fixed (byte* psql = queryBytes)
                {
                    int result = NativeMySqlClient.mysql_query(_handle, psql);
                    if (result != 0)
                    {
                        throw new MySqlInteropException($"mysql_query failed: {GetLastError(_handle)}");
                    }
                }
            }

            return MySqlResultCode.Ok;
        }

        /// <summary>
        /// Checks if the server connection is alive by calling <c>mysql_ping</c>.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when the client has already been disposed.</exception>
        /// <exception cref="MySqlInteropException">Thrown when native call returns an error code.</exception>
        public void Ping()
        {
            EnsureNotDisposed();
            int result = NativeMySqlClient.mysql_ping(_handle);
            if (result != 0)
            {
                throw new MySqlInteropException($"mysql_ping failed: {GetLastError(_handle)}");
            }
        }

        /// <summary>
        /// Closes the native connection handle and releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_handle == IntPtr.Zero)
            {
                return;
            }

            NativeMySqlClient.mysql_close(_handle);
            _handle = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }

        private void EnsureNotDisposed()
        {
            if (_handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException(nameof(MySql));
            }
        }

        private void ThrowIfError(int result, string operationName)
        {
            if (result != 0)
            {
                throw new MySqlInteropException($"{operationName} failed: {GetLastError(_handle)}");
            }
        }

        private static string GetLastError(IntPtr handle)
        {
            IntPtr ptr = NativeMySqlClient.mysql_error(handle);
            return ptr == IntPtr.Zero
                ? "unknown error"
                : Marshal.PtrToStringUTF8(ptr) ?? "unknown error";
        }

        private static byte[] BuildUtf8NullTerminated(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
            byte[] nullTerminated = new byte[bytes.Length + 1];
            bytes.CopyTo(nullTerminated, 0);
            return nullTerminated;
        }
    }
}
