// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using Microsoft.Win32.SafeHandles;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

internal class MySqlStatementHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal MySqlStatementHandle(MySqlHandle safeHandle) : base(true)
    {

        nint ptr = NativeMariadbStmt.mysql_stmt_init(safeHandle.DangerousGetHandle());
        if (ptr == 0)
            throw new OutOfMemoryException("mysql_stmt_init failed.");
        SetHandle(ptr);
    }

    protected override bool ReleaseHandle()
    {
        NativeMariadbStmt.mysql_stmt_close(handle);
        return true;
    }
}


public sealed unsafe class MySqlStatement : IDisposable
{
    private readonly MySqlStatementHandle _handle;
    // private nint _stmt;
    // private bool _disposed;

    internal MySqlStatement(MySqlHandle safeHandle)
    {
        // _stmt = stmt;
        _handle = new MySqlStatementHandle(safeHandle);
    }

    // public static MySqlStatement Init(MySql connection)
    // {
    //     nint stmt = NativeMariadbStmt.mysql_stmt_init(
    //         connection.Handle.DangerousGetHandle());
    //     if (stmt == 0)
    //         throw new OutOfMemoryException("mysql_stmt_init failed.");
    //     return new MySqlStatement(stmt);
    // }

    public void Prepare(string sql)
    {
        EnsureNotDisposed();
        byte[] sqlBytes = Utils.BuildUtf8NullTerminated(sql);
        int rc;
        fixed (byte* p = sqlBytes)
            rc = NativeMariadbStmt.mysql_stmt_prepare(
                _handle.DangerousGetHandle(), p, (uint)(sqlBytes.Length - 1));
        if (rc != 0)
            ThrowStmtError();
    }

    // public void BindParams(st_mysql_bind* binds)
    // {
    //     EnsureNotDisposed();
    //     if (NativeMariadbStmt.mysql_stmt_bind_param(_stmt, binds) != 0)
    //         ThrowStmtError();
    // }

    public void Execute()
    {
        EnsureNotDisposed();
        if (NativeMariadbStmt.mysql_stmt_execute(_handle.DangerousGetHandle()) != 0)
            ThrowStmtError();
    }

    public uint ParamCount => NativeMariadbStmt.mysql_stmt_param_count(_handle.DangerousGetHandle());

    // private void EnsureNotDisposed()
    // {
    //     if (_disposed) throw new ObjectDisposedException(nameof(MySqlStatement));
    // }

    private void EnsureNotDisposed()
    {
        if (_handle.IsClosed || _handle.IsInvalid)
        {
            throw new ObjectDisposedException(nameof(MySqlStatement));
        }
    }

    private void ThrowStmtError()
    {
        byte* pMsg = NativeMariadbStmt.mysql_stmt_error(_handle.DangerousGetHandle());
        byte* pState = NativeMariadbStmt.mysql_stmt_sqlstate(_handle.DangerousGetHandle());
        uint errno = NativeMariadbStmt.mysql_stmt_errno(_handle.DangerousGetHandle());
        throw new MySqlException(
            Utils.GetStringFromPointerBytes(pMsg),
            (int)errno,
            Utils.GetStringFromPointerBytes(pState));
    }

    public void Dispose()
    {
        // if (_disposed) return;
        // _disposed = true;
        // NativeMariadbStmt.mysql_stmt_close(_handle.DangerousGetHandle());
        _handle.Dispose();
        GC.SuppressFinalize(this);
    }
}