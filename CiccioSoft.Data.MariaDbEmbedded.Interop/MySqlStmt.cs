// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using Microsoft.Win32.SafeHandles;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

internal class MySqlStmtHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal MySqlStmtHandle(nint ptr) : base(true)
    {
        SetHandle(ptr);
    }

    protected override bool ReleaseHandle()
    {
        if (handle != 0)
            MariadbStmtNative.mysql_stmt_close(handle);
        return true;
    }
}


/// <summary>
/// Managed wrapper for a native prepared statement (<c>MYSQL_STMT*</c>).
/// Method names are aligned with native C functions <c>mysql_stmt_*</c>.
/// </summary>
public sealed unsafe class MySqlStmt : IDisposable
{
    private readonly MySqlStmtHandle _handle;

    internal MySqlStmt(MySqlStmtHandle handle)
    {
        _handle = handle;
    }

    // ------------------------------------------------------------------
    //  mysql_stmt_prepare
    // ------------------------------------------------------------------

    /// <summary>
    /// Prepares SQL text on the server.
    /// Maps to <c>mysql_stmt_prepare</c>.
    /// </summary>
    public void Prepare(string sql)
    {
        EnsureNotDisposed();
        ReadOnlySpan<byte> sqlBytes = Utils.BuildUtf8NullTerminated(sql);
        int rc;
        fixed (byte* p = sqlBytes)
            rc = MariadbStmtNative.mysql_stmt_prepare(
                _handle.DangerousGetHandle(), p, (uint)(sqlBytes.Length - 1));
        if (rc != 0)
            ThrowStmtError();
    }


    #region  BindParam / BindResult

    /// <summary>
    /// Binds input parameters (<c>?</c>) to the statement.
    /// Maps to <c>mysql_stmt_bind_param</c>.
    /// </summary>
    public void BindParam(MySqlBindBuilder binds)
    {
        EnsureNotDisposed();
        var span = binds.GetNativeArray().AsSpan<MySqlBindNative>();
        fixed (MySqlBindNative* ptr = span)
        {
            if (MariadbStmtNative.mysql_stmt_bind_param(_handle.DangerousGetHandle(), ptr) != 0)
                ThrowStmtError();
        }
    }

    /// <summary>
    /// Binds output buffers for result columns.
    /// Maps to <c>mysql_stmt_bind_result</c>.
    /// </summary>
    public void BindResult(MySqlBindBuilder binds)
    {
        EnsureNotDisposed();
        var span = binds.GetNativeArray().AsSpan<MySqlBindNative>();
        fixed (MySqlBindNative* ptr = span)
        {
            if (MariadbStmtNative.mysql_stmt_bind_result(_handle.DangerousGetHandle(), ptr) != 0)
                ThrowStmtError();
        }
    }

    #endregion


    // ------------------------------------------------------------------
    //  mysql_stmt_execute
    // ------------------------------------------------------------------

    /// <summary>
    /// Executes the prepared statement.
    /// Maps to <c>mysql_stmt_execute</c>.
    /// </summary>
    public void Execute()
    {
        EnsureNotDisposed();
        if (MariadbStmtNative.mysql_stmt_execute(_handle.DangerousGetHandle()) != 0)
            ThrowStmtError();
    }


    #region StoreResult / FreeResult

    /// <summary>
    /// Transfers the full result set into client memory.
    /// Maps to <c>mysql_stmt_store_result</c>.
    /// </summary>
    public void StoreResult()
    {
        EnsureNotDisposed();
        int rc = MariadbStmtNative.mysql_stmt_store_result(_handle.DangerousGetHandle());
        if (rc != 0) ThrowStmtError();
    }

    /// <summary>
    /// Releases the prepared statement result set.
    /// Maps to <c>mysql_stmt_free_result</c>.
    /// </summary>
    public void FreeResult()
    {
        EnsureNotDisposed();
        int rc = MariadbStmtNative.mysql_stmt_free_result(_handle.DangerousGetHandle());
        if (rc != 0) ThrowStmtError();
    }

    #endregion


    #region Fetch / FetchColumn

    /// <summary>
    /// Fetches the next row into buffers bound by <c>BindResult</c>.
    /// Returns <c>true</c> when a row is available, otherwise <c>false</c> at end-of-data.
    /// Maps to <c>mysql_stmt_fetch</c>.
    /// </summary>
    public bool Fetch()
    {
        EnsureNotDisposed();
        int rc = MariadbStmtNative.mysql_stmt_fetch(_handle.DangerousGetHandle());
        // 0 = OK, 100 = MYSQL_NO_DATA, 101 = MYSQL_DATA_TRUNCATED
        if (rc == 0 || rc == 101) return true;
        if (rc == 100) return false;           // MYSQL_NO_DATA
        ThrowStmtError();
        return false;
    }

    /// <summary>
    /// Fetches a single column from the current row.
    /// Maps to <c>mysql_stmt_fetch_column</c>.
    /// </summary>
    public void FetchColumn(MySqlBind bind, uint column, uint offset = 0)
    {
        EnsureNotDisposed();        
        fixed (MySqlBindNative* ptr = &bind.Native)
        {
            int rc = MariadbStmtNative.mysql_stmt_fetch_column(_handle.DangerousGetHandle(), ptr, column, offset);
            if (rc != 0) ThrowStmtError();
        }
    }

    #endregion


    // ------------------------------------------------------------------
    //  mysql_stmt_reset
    // ------------------------------------------------------------------

    /// <summary>
    /// Resets statement state and bound buffers.
    /// Maps to <c>mysql_stmt_reset</c>.
    /// </summary>
    public void Reset()
    {
        EnsureNotDisposed();
        int rc = MariadbStmtNative.mysql_stmt_reset(_handle.DangerousGetHandle());
        if (rc != 0) ThrowStmtError();
    }


    // ------------------------------------------------------------------
    //  Metadata and status information
    // ------------------------------------------------------------------


    /// <summary>Number of columns in the statement result set. Maps to <c>mysql_stmt_field_count</c>..</summary>
    public uint FieldCount
    {
        get
        {
            EnsureNotDisposed();
            return MariadbStmtNative.mysql_stmt_field_count(_handle.DangerousGetHandle());
        }
    }

    /// <summary>Number of parameter markers (<c>?</c>). Maps to <c>mysql_stmt_param_count</c>.</summary>
    public uint ParamCount()
    {
        EnsureNotDisposed();
        return MariadbStmtNative.mysql_stmt_param_count(_handle.DangerousGetHandle());
    }

    /// <summary>Rows changed/inserted/deleted. Maps to <c>mysql_stmt_affected_rows</c>.</summary>
    public ulong AffectedRows()
    {
        EnsureNotDisposed();
        return MariadbStmtNative.mysql_stmt_affected_rows(_handle.DangerousGetHandle());
    }

    /// <summary>Last generated AUTO_INCREMENT value. Maps to <c>mysql_stmt_insert_id</c>.</summary>
    public ulong InsertId()
    {
        EnsureNotDisposed();
        return MariadbStmtNative.mysql_stmt_insert_id(_handle.DangerousGetHandle());
    }

    /// <summary>Number of rows in the result set (valid after <c>StoreResult</c>). Maps to <c>mysql_stmt_num_rows</c>.</summary>
    public ulong NumRows
    {
        get
        {
            EnsureNotDisposed();
            return MariadbStmtNative.mysql_stmt_num_rows(_handle.DangerousGetHandle());
        }
    }

    /// <summary>
    /// Returns the column metadata result set.
    /// Maps to <c>mysql_stmt_result_metadata</c>.
    /// </summary>
    public MySqlResult? ResultMetadata()
    {
        EnsureNotDisposed();
        var ptr = MariadbStmtNative.mysql_stmt_result_metadata(_handle.DangerousGetHandle());
        return ptr == IntPtr.Zero ? null : new MySqlResult(new MySqlResultHandle(ptr));
    }

    /// <summary>Current cursor offset for navigation. Maps to <c>mysql_stmt_row_tell</c>.</summary>
    // public ulong RowTell()
    // {
    //     EnsureNotDisposed();
    //     return MariadbStmtNative.mysql_stmt_row_tell(_handle.DangerousGetHandle());
    // }

    // /// <summary>Moves the cursor. Maps to <c>mysql_stmt_row_seek</c>.</summary>
    // public ulong RowSeek(ulong offset)
    // {
    //     EnsureNotDisposed();
    //     return MariadbStmtNative.mysql_stmt_row_seek(_handle.DangerousGetHandle(), offset);
    // }

    /// <summary>Moves to an absolute row offset. Maps to <c>mysql_stmt_data_seek</c>.</summary>
    public void DataSeek(ulong offset)
    {
        EnsureNotDisposed();
        MariadbStmtNative.mysql_stmt_data_seek(_handle.DangerousGetHandle(), offset);
    }


    #region Errori dello statement

    /// <summary>Current error message text. Maps to <c>mysql_stmt_error</c>.</summary>
    public string Error()
    {
        EnsureNotDisposed();
        var ptr = MariadbStmtNative.mysql_stmt_error(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(ptr);
    }

    /// <summary>Numeric error code. Maps to <c>mysql_stmt_errno</c>.</summary>
    public uint Errno()
    {
        EnsureNotDisposed();
        return MariadbStmtNative.mysql_stmt_errno(_handle.DangerousGetHandle());
    }

    /// <summary>Current SQLSTATE value. Maps to <c>mysql_stmt_sqlstate</c>.</summary>
    public string Sqlstate()
    {
        EnsureNotDisposed();
        var ptr = MariadbStmtNative.mysql_stmt_sqlstate(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(ptr);
    }

    #endregion


    /// <summary>
    /// Advances to the next result set in a multi-result execution.
    /// Maps to <c>mysql_stmt_next_result</c>.
    /// </summary>
    public bool NextResult()
    {
        EnsureNotDisposed();
        int rc = MariadbStmtNative.mysql_stmt_next_result(_handle.DangerousGetHandle());
        if (rc > 0) ThrowStmtError();
        return rc == 0;
    }


    // public uint ParamCount => NativeMariadbStmt.mysql_stmt_param_count(_handle.DangerousGetHandle());


    #region Helper

    private void EnsureNotDisposed()
    {
        if (_handle.IsClosed || _handle.IsInvalid)
        {
            throw new ObjectDisposedException(nameof(MySqlStmt));
        }
    }

    private void ThrowStmtError()
    {
        byte* pMsg = MariadbStmtNative.mysql_stmt_error(_handle.DangerousGetHandle());
        byte* pState = MariadbStmtNative.mysql_stmt_sqlstate(_handle.DangerousGetHandle());
        uint errno = MariadbStmtNative.mysql_stmt_errno(_handle.DangerousGetHandle());
        throw new MySqlException(
            Utils.GetStringFromPointerBytes(pMsg),
            (int)errno,
            Utils.GetStringFromPointerBytes(pState));
    }

    #endregion


    public void Dispose()
    {
        _handle.Dispose();
        GC.SuppressFinalize(this);
    }
}
