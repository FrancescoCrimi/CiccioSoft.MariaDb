// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;
using Microsoft.Win32.SafeHandles;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

internal sealed class MySqlResultHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal MySqlResultHandle(nint ptr) : base(true)
    {
        SetHandle(ptr);
    }

    protected override bool ReleaseHandle()
    {
        if (handle != 0)
            NativeMySql.mysql_free_result(handle);
        return true;
    }
}

public sealed unsafe class MySqlResult : IDisposable
{
    private readonly MySqlResultHandle _handle;
    private MySqlField[]? _fieldsCache;

    internal MySqlResult(MySqlResultHandle handle)
    {
        _handle = handle;
    }


    // ── informazioni generali ────────────────────────────────────────────

    public ulong RowCount => NativeMySql.mysql_num_rows(_handle.DangerousGetHandle());
    public uint FieldCount => NativeMySql.mysql_num_fields(_handle.DangerousGetHandle());


    // ── iterazione righe ─────────────────────────────────────────────────

    /// <summary>
    /// Avanza al record successivo.
    /// Restituisce <see langword="true"/> se c'è una riga disponibile, <see langword="false"/> altrimenti.
    /// <para/>
    /// ATTENZIONE: i puntatori interni di <see cref="MySqlRow"/> sono validi
    /// solo fino alla chiamata successiva di <c>FetchRow</c> o <c>Dispose</c>.
    /// </summary>
    public bool FetchRow(out MySqlRow row)
    {
        byte** r = NativeMySql.mysql_fetch_row(_handle.DangerousGetHandle());
        if (r == null)
        {
            row = default;
            return false;
        }

        uint* lengths = NativeMySql.mysql_fetch_lengths(_handle.DangerousGetHandle());
        uint count = FieldCount;
        row = new MySqlRow(r, lengths, count);
        return true;
    }

    /// <summary>
    /// Riposiziona il cursore all'inizio del result set.
    /// </summary>
    public void DataSeek() =>
        NativeMySql.mysql_data_seek(_handle.DangerousGetHandle(), 0);


    // ── metadati colonne ─────────────────────────────────────────────────

    /// <summary>
    /// Restituisce i metadati di tutte le colonne.
    /// Il risultato è memoizzato: la chiamata nativa avviene una sola volta.
    /// </summary>
    public MySqlField[] FetchFields()
    {
        if (_fieldsCache != null)
            return _fieldsCache;

        uint count = FieldCount;
        _fieldsCache = new MySqlField[count];

        for (uint i = 0; i < count; i++)
        {
            nint pFields = NativeMySql.mysql_fetch_field_direct(_handle.DangerousGetHandle(), i);
            _fieldsCache[i] = MySqlField.FromPointer(pFields);
        }

        return _fieldsCache;
    }

    /// <summary>
    /// Metadati di una singola colonna per indice.
    /// </summary>
    public MySqlField FetchField(uint index)
    {
        if (index >= FieldCount)
            throw new ArgumentOutOfRangeException(nameof(index));

        nint ptr = NativeMySql.mysql_fetch_field_direct(
            _handle.DangerousGetHandle(), index);
        return MySqlField.FromPointer(ptr);
    }


    private void EnsureNotDisposed()
    {
        if (_handle.IsClosed || _handle.IsInvalid)
        {
            throw new ObjectDisposedException(nameof(MySqlResult));
        }
    }

    // ── cleanup ──────────────────────────────────────────────────────────

    public void Dispose()
    {
        _handle.Dispose();
        GC.SuppressFinalize(this);
    }
}
