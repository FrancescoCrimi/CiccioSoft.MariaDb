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
/// Wrapper managed per un prepared statement nativo (<c>MYSQL_STMT*</c>).
/// I nomi dei metodi coincidono con le funzioni C <c>mysql_stmt_*</c>.
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
    /// Prepara la query SQL sul server.
    /// Corrisponde a <c>mysql_stmt_prepare</c>.
    /// </summary>
    public void Prepare(string sql)
    {
        EnsureNotDisposed();
        byte[] sqlBytes = Utils.BuildUtf8NullTerminated(sql);
        int rc;
        fixed (byte* p = sqlBytes)
            rc = MariadbStmtNative.mysql_stmt_prepare(
                _handle.DangerousGetHandle(), p, (uint)(sqlBytes.Length - 1));
        if (rc != 0)
            ThrowStmtError();
    }


    #region  BindParam / BindResult

    /// <summary>
    /// Associa i parametri di input (<c>?</c>) allo statement.
    /// Corrisponde a <c>mysql_stmt_bind_param</c>.
    /// </summary>
    public void BindParam(MySqlBindBuilder binds)
    {
        EnsureNotDisposed();
        fixed (MySqlBindNative* ptr = binds.GetNativeArray().AsSpan<MySqlBindNative>())
        {
            if (MariadbStmtNative.mysql_stmt_bind_param(_handle.DangerousGetHandle(), ptr) != 0)
                ThrowStmtError();
        }
    }

    /// <summary>
    /// Associa i buffer di output per i risultati.
    /// Corrisponde a <c>mysql_stmt_bind_result</c>.
    /// </summary>
    public void BindResult(MySqlBindBuilder binds)
    {
        EnsureNotDisposed();
        fixed (MySqlBindNative* ptr = binds.GetNativeArray().AsSpan<MySqlBindNative>())
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
    /// Esegue lo statement preparato.
    /// Corrisponde a <c>mysql_stmt_execute</c>.
    /// </summary>
    public void Execute()
    {
        EnsureNotDisposed();
        if (MariadbStmtNative.mysql_stmt_execute(_handle.DangerousGetHandle()) != 0)
            ThrowStmtError();
    }


    #region StoreResult / FreeResult

    /// <summary>
    /// Trasferisce l'intero result set in memoria client.
    /// Corrisponde a <c>mysql_stmt_store_result</c>.
    /// </summary>
    public void StoreResult()
    {
        EnsureNotDisposed();
        int rc = MariadbStmtNative.mysql_stmt_store_result(_handle.DangerousGetHandle());
        if (rc != 0) ThrowStmtError();
    }

    /// <summary>
    /// Libera il result set del prepared statement.
    /// Corrisponde a <c>mysql_stmt_free_result</c>.
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
    /// Legge la riga successiva nel buffer associato con bind_result.
    /// Restituisce <c>true</c> se c'è una riga, <c>false</c> a fine set.
    /// Corrisponde a <c>mysql_stmt_fetch</c>.
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

    // /// <summary>
    // /// Legge una singola colonna della riga corrente.
    // /// Corrisponde a <c>mysql_stmt_fetch_column</c>.
    // /// </summary>
    // public void FetchColumn(MySqlBind bind, uint column, nuint offset = 0)
    // {
    //     EnsureNotDisposed();
    //     int rc = MariadbStmtNative.mysql_stmt_fetch_column(_handle.DangerousGetHandle(), &bind.Native, column, offset);
    //     if (rc != 0) ThrowStmtError();
    // }

    #endregion


    // ------------------------------------------------------------------
    //  mysql_stmt_reset
    // ------------------------------------------------------------------

    /// <summary>
    /// Resetta lo statement azzerando i buffer.
    /// Corrisponde a <c>mysql_stmt_reset</c>.
    /// </summary>
    public void Reset()
    {
        EnsureNotDisposed();
        int rc = MariadbStmtNative.mysql_stmt_reset(_handle.DangerousGetHandle());
        if (rc != 0) ThrowStmtError();
    }


    // ------------------------------------------------------------------
    //  Metadati e informazioni di stato
    // ------------------------------------------------------------------

    /// <summary>Numero di colonne nel result set. Corrisponde a <c>mysql_stmt_field_count</c>.</summary>
    public uint FieldCount()
    {
        EnsureNotDisposed();
        return MariadbStmtNative.mysql_stmt_field_count(_handle.DangerousGetHandle());
    }

    /// <summary>Numero di parametri (<c>?</c>). Corrisponde a <c>mysql_stmt_param_count</c>.</summary>
    public uint ParamCount()
    {
        EnsureNotDisposed();
        return MariadbStmtNative.mysql_stmt_param_count(_handle.DangerousGetHandle());
    }

    /// <summary>Righe modificate/inserite/cancellate. Corrisponde a <c>mysql_stmt_affected_rows</c>.</summary>
    public ulong AffectedRows()
    {
        EnsureNotDisposed();
        return MariadbStmtNative.mysql_stmt_affected_rows(_handle.DangerousGetHandle());
    }

    /// <summary>Ultimo AUTO_INCREMENT generato. Corrisponde a <c>mysql_stmt_insert_id</c>.</summary>
    public ulong InsertId()
    {
        EnsureNotDisposed();
        return MariadbStmtNative.mysql_stmt_insert_id(_handle.DangerousGetHandle());
    }

    /// <summary>Numero di righe nel result set (solo dopo store_result). Corrisponde a <c>mysql_stmt_num_rows</c>.</summary>
    public ulong NumRows()
    {
        EnsureNotDisposed();
        return MariadbStmtNative.mysql_stmt_num_rows(_handle.DangerousGetHandle());
    }

    // /// <summary>
    // /// Restituisce il result set dei metadati delle colonne.
    // /// Corrisponde a <c>mysql_stmt_result_metadata</c>.
    // /// </summary>
    // public MysqlResult? mysql_stmt_result_metadata()
    // {
    //     EnsureNotDisposed();
    //     var res = NativeMariadbStmt.mysql_stmt_result_metadata(_handle.DangerousGetHandle());
    //     return res == IntPtr.Zero ? null : new MysqlResult(res);
    // }

    // /// <summary>Cursore corrente per navigazione. Corrisponde a <c>mysql_stmt_row_tell</c>.</summary>
    // public ulong RowTell()
    // {
    //     EnsureNotDisposed();
    //     return NativeMariadbStmt.mysql_stmt_row_tell(_stmt);
    // }

    // /// <summary>Sposta il cursore. Corrisponde a <c>mysql_stmt_row_seek</c>.</summary>
    // public ulong RowSeek(ulong offset)
    // {
    //     EnsureNotDisposed();
    //     return NativeMariadbStmt.mysql_stmt_row_seek(_handle.DangerousGetHandle(), offset);
    // }

    /// <summary>Sposta il cursore assoluto. Corrisponde a <c>mysql_stmt_data_seek</c>.</summary>
    public void DataSeek(ulong offset)
    {
        EnsureNotDisposed();
        MariadbStmtNative.mysql_stmt_data_seek(_handle.DangerousGetHandle(), offset);
    }


    #region Errori dello statement

    /// <summary>Messaggio di errore corrente. Corrisponde a <c>mysql_stmt_error</c>.</summary>
    public string mysql_stmt_error()
    {
        EnsureNotDisposed();
        var ptr = MariadbStmtNative.mysql_stmt_error(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(ptr);
    }

    /// <summary>Codice di errore numerico. Corrisponde a <c>mysql_stmt_errno</c>.</summary>
    public uint mysql_stmt_errno()
    {
        EnsureNotDisposed();
        return MariadbStmtNative.mysql_stmt_errno(_handle.DangerousGetHandle());
    }

    /// <summary>SQLSTATE corrente. Corrisponde a <c>mysql_stmt_sqlstate</c>.</summary>
    public string Sqlstate()
    {
        EnsureNotDisposed();
        var ptr = MariadbStmtNative.mysql_stmt_sqlstate(_handle.DangerousGetHandle());
        return Utils.GetStringFromPointerBytes(ptr);
    }

    #endregion


    /// <summary>
    /// Avanza al prossimo result set in una query multi-risultato.
    /// Corrisponde a <c>mysql_stmt_next_result</c>.
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