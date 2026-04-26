using System;
using System.Collections;
using System.Collections.Generic;
using MariaDbClient.Native;

namespace MariaDbClient;

/// <summary>
/// Wrapper managed per un result set nativo (<c>MYSQL_RES*</c>).
/// Implementa <see cref="IEnumerable{MysqlRow}"/> e <see cref="IDisposable"/>.
/// Chiama automaticamente <c>mysql_free_result</c> nel Dispose.
/// </summary>
public sealed class MysqlResult : IDisposable, IEnumerable<MysqlRow>
{
    private IntPtr _res;
    private MysqlField[]? _fields;
    private bool _disposed;

    /// <summary>Handle nativo al result set (MYSQL_RES*).</summary>
    public IntPtr Handle => _res;

    /// <summary>
    /// Numero di righe nel result set.
    /// Valido solo se caricato con <c>mysql_store_result</c>;
    /// con <c>mysql_use_result</c> restituisce 0 finché non sono lette tutte le righe.
    /// </summary>
    public ulong NumRows
    {
        get
        {
            ThrowIfDisposed();
            return MariaDbImports.mysql_num_rows(_res);
        }
    }

    /// <summary>Numero di colonne nel result set.</summary>
    public uint NumFields
    {
        get
        {
            ThrowIfDisposed();
            return MariaDbImports.mysql_num_fields(_res);
        }
    }

    internal MysqlResult(IntPtr res)
    {
        _res = res;
    }

    // ------------------------------------------------------------------
    //  Metadati colonne
    // ------------------------------------------------------------------

    /// <summary>
    /// Restituisce i metadati di tutte le colonne.
    /// Corrisponde a <c>mysql_fetch_fields</c>.
    /// </summary>
    public unsafe MysqlField[] mysql_fetch_fields()
    {
        ThrowIfDisposed();
        if (_fields is not null) return _fields;

        uint count = MariaDbImports.mysql_num_fields(_res);
        MysqlFieldNative* native = MariaDbImports.mysql_fetch_fields(_res);
        _fields = new MysqlField[count];
        for (uint i = 0; i < count; i++)
            _fields[i] = new MysqlField(&native[i]);
        return _fields;
    }

    /// <summary>
    /// Restituisce i metadati della colonna corrente avanzando il cursore interno.
    /// Corrisponde a <c>mysql_fetch_field</c>.
    /// </summary>
    public unsafe MysqlField? mysql_fetch_field()
    {
        ThrowIfDisposed();
        MysqlFieldNative* f = MariaDbImports.mysql_fetch_field(_res);
        return f == null ? null : new MysqlField(f);
    }

    /// <summary>
    /// Restituisce i metadati della colonna <paramref name="fieldNr"/>.
    /// Corrisponde a <c>mysql_fetch_field_direct</c>.
    /// </summary>
    public unsafe MysqlField mysql_fetch_field_direct(uint fieldNr)
    {
        ThrowIfDisposed();
        MysqlFieldNative* f = MariaDbImports.mysql_fetch_field_direct(_res, fieldNr);
        if (f == null) throw new ArgumentOutOfRangeException(nameof(fieldNr));
        return new MysqlField(f);
    }

    // ------------------------------------------------------------------
    //  Iterazione righe
    // ------------------------------------------------------------------

    /// <summary>
    /// Legge la riga successiva dal result set.
    /// Corrisponde a <c>mysql_fetch_row</c> + <c>mysql_fetch_lengths</c>.
    /// Restituisce <c>null</c> a fine result set.
    /// </summary>
    public unsafe MysqlRow? mysql_fetch_row()
    {
        ThrowIfDisposed();
        byte** row = MariaDbImports.mysql_fetch_row(_res);
        if (row == null) return null;

        uint* lengths = MariaDbImports.mysql_fetch_lengths(_res);
        MysqlField[] fields = mysql_fetch_fields();
        return new MysqlRow(row, lengths, fields, trackNull: true);
    }

    /// <summary>
    /// Sposta il cursore di lettura alla riga <paramref name="offset"/>.
    /// Corrisponde a <c>mysql_data_seek</c>.
    /// </summary>
    public void mysql_data_seek(ulong offset)
    {
        ThrowIfDisposed();
        MariaDbImports.mysql_data_seek(_res, offset);
    }

    /// <summary>
    /// Restituisce il cursore corrente (MYSQL_ROW_OFFSET).
    /// Corrisponde a <c>mysql_row_tell</c>.
    /// </summary>
    public IntPtr mysql_row_tell()
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_row_tell(_res);
    }

    /// <summary>
    /// Sposta il cursore a <paramref name="offset"/> e restituisce quello precedente.
    /// Corrisponde a <c>mysql_row_seek</c>.
    /// </summary>
    public IntPtr mysql_row_seek(IntPtr offset)
    {
        ThrowIfDisposed();
        return MariaDbImports.mysql_row_seek(_res, offset);
    }

    // ------------------------------------------------------------------
    //  IEnumerable<MysqlRow>
    // ------------------------------------------------------------------

    /// <summary>
    /// Permette di iterare il result set con <c>foreach</c>.
    /// Internamente chiama <see cref="mysql_fetch_row"/> in sequenza.
    /// </summary>
    public IEnumerator<MysqlRow> GetEnumerator()
    {
        ThrowIfDisposed();
        MysqlRow? row;
        while ((row = mysql_fetch_row()) is not null)
            yield return row;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // ------------------------------------------------------------------
    //  Dispose / mysql_free_result
    // ------------------------------------------------------------------

    /// <summary>
    /// Libera il result set nativo.
    /// Corrisponde a <c>mysql_free_result</c>.
    /// </summary>
    public void mysql_free_result()
    {
        if (_res != IntPtr.Zero)
        {
            MariaDbImports.mysql_free_result(_res);
            _res = IntPtr.Zero;
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        mysql_free_result();
    }

    private void ThrowIfDisposed()
    {
        if (_disposed) throw new ObjectDisposedException(nameof(MysqlResult));
    }
}
