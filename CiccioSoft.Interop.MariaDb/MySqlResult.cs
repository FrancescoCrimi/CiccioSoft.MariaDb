// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using CiccioSoft.Interop.MariaDb.Native;
using Microsoft.Win32.SafeHandles;

namespace CiccioSoft.Interop.MariaDb;

internal sealed class MySqlResultHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal MySqlResultHandle(nint ptr) : base(true)
    {
        SetHandle(ptr);
    }

    protected override bool ReleaseHandle()
    {
        if (handle != 0)
            MySqlNative.mysql_free_result(handle);
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


    #region Informazioni generali

    public ulong NumRows
    {
        get
        {
            EnsureNotDisposed();
            return MySqlNative.mysql_num_rows(_handle.DangerousGetHandle());
        }
    }

    public uint NumFields
    {
        get
        {
            EnsureNotDisposed();
            return MySqlNative.mysql_num_fields(_handle.DangerousGetHandle());
        }
    }

    #endregion


    #region Iterazione righe

    /// <summary>
    /// Advances to the next row.
    /// Returns <see langword="true"/> when a row is available; otherwise <see langword="false"/>.
    /// <para/>
    /// WARNING: internal pointers inside <see cref="MySqlRow"/> remain valid
    /// only until the next <c>FetchRow</c> call or <c>Dispose</c>.
    /// </summary>
    public bool FetchRow(out MySqlRow row)
    {
        EnsureNotDisposed();

        // byte** r = NativeMySql.mysql_fetch_row(_handle.DangerousGetHandle());
        nint r = MySqlNative.mysql_fetch_row(_handle.DangerousGetHandle());

        // if (r == null)
        if (r == 0)
        {
            row = default;
            return false;
        }

        uint* lengths = MySqlNative.mysql_fetch_lengths(_handle.DangerousGetHandle());
        // uint count = NumFields;
        // row = new MySqlRow(r, lengths, count);

        ReadOnlySpan<nint> rowSpan = new(r.ToPointer(), (int)NumFields);
        ReadOnlySpan<uint> lengthsSpan = new(lengths, (int)NumFields);
        row = new MySqlRow(rowSpan, lengthsSpan, FetchFields());

        return true;
    }

    /// <summary>
    /// Repositions the cursor at the beginning of the result set.
    /// </summary>
    public void DataSeek()
    {
        EnsureNotDisposed();
        MySqlNative.mysql_data_seek(_handle.DangerousGetHandle(), 0);
    }

    #endregion


    #region Metadati colonne

    /// <summary>
    /// Returns metadata for all columns.
    /// The result is cached: the native call is performed once.
    /// </summary>
    public MySqlField[] FetchFields()
    {
        EnsureNotDisposed();

        if (_fieldsCache != null)
            return _fieldsCache;

        uint count = NumFields;
        _fieldsCache = new MySqlField[count];

        MySqlFieldNative* ptr = MySqlNative.mysql_fetch_fields(_handle.DangerousGetHandle());
        Span<MySqlFieldNative> nativeFields = new(ptr, (int)count);  //MySqlFieldNative
        for (uint i = 0; i < count; i++)
        {
            ref MySqlFieldNative f = ref nativeFields[(int)i];
            _fieldsCache[i] = new MySqlField(f);
        }

        return _fieldsCache;
    }

    /// <summary>
    /// Metadata for a single column by index.
    /// </summary>
    public MySqlField FetchField(uint index)
    {
        EnsureNotDisposed();

        if (index >= NumFields)
            throw new ArgumentOutOfRangeException(nameof(index));

        MySqlFieldNative* ptr = MySqlNative.mysql_fetch_field_direct(_handle.DangerousGetHandle(), index);
        MySqlFieldNative nativeField = *ptr;
        return new MySqlField(nativeField);
    }

    #endregion


    #region Helper

    private void EnsureNotDisposed()
    {
        if (_handle.IsClosed || _handle.IsInvalid)
        {
            throw new ObjectDisposedException(nameof(MySqlResult));
        }
    }

    #endregion


    public void Dispose()
    {
        _handle.Dispose();
        GC.SuppressFinalize(this);
    }
}
