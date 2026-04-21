// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

public sealed unsafe class MySqlStatement : IDisposable
{
    private nint _stmt;
    private bool _disposed;

    private MySqlStatement(nint stmt) => _stmt = stmt;

    // public static MySqlStatement Init(MySql connection)
    // {
    //     nint stmt = NativeMariadbStmt.mysql_stmt_init(
    //         connection.DangerousGetHandle());
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
                _stmt, p, (uint)(sqlBytes.Length - 1));
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
        if (NativeMariadbStmt.mysql_stmt_execute(_stmt) != 0)
            ThrowStmtError();
    }

    public uint ParamCount => NativeMariadbStmt.mysql_stmt_param_count(_stmt);

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        NativeMariadbStmt.mysql_stmt_close(_stmt);
        GC.SuppressFinalize(this);
    }

    private void EnsureNotDisposed()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(MySqlStatement));
    }

    private void ThrowStmtError()
    {
        byte* pMsg   = NativeMariadbStmt.mysql_stmt_error(_stmt);
        byte* pState = NativeMariadbStmt.mysql_stmt_sqlstate(_stmt);
        uint  errno  = NativeMariadbStmt.mysql_stmt_errno(_stmt);
        throw new MySqlException(
            Utils.GetStringFromPointerBytes(pMsg),
            (int)errno,
            Utils.GetStringFromPointerBytes(pState));
    }

    internal nint DangerousGetHandle() => _stmt;
}