// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Globalization;
using System.Text;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

/// <summary>
/// Riga corrente di un result set nativo.
/// È una <c>ref struct</c>: non può essere conservata oltre il ciclo di lettura.
/// I puntatori interni diventano invalidi alla chiamata successiva di
/// <see cref="MySqlResult.FetchRow"/> o <see cref="MySqlResult.Dispose"/>.
/// </summary>
public readonly unsafe ref struct MySqlRow
{
    private readonly byte** _row;
    private readonly uint* _lengths;
    private readonly uint _fieldCount;

    internal MySqlRow(byte** row, uint* lengths, uint fieldCount)
    {
        _row = row;
        _lengths = lengths;
        _fieldCount = fieldCount;
    }

    public int FieldCount => (int)_fieldCount;


    // ── null check ───────────────────────────────────────────────────────

    public bool IsNull(int ordinal)
    {
        CheckOrdinal(ordinal);
        return _row[ordinal] == null;
    }

    
    // ── accesso raw ──────────────────────────────────────────────────────

    /// <summary>
    /// Bytes grezzi della colonna, senza allocazione.
    /// Utile per BLOB o per conversioni custom.
    /// </summary>
    public ReadOnlySpan<byte> GetRawBytes(int ordinal)
    {
        CheckOrdinal(ordinal);
        byte* col = _row[ordinal];
        if (col == null) return ReadOnlySpan<byte>.Empty;
        return new ReadOnlySpan<byte>(col, (int)_lengths[ordinal]);
    }


    // ── tipi managed ─────────────────────────────────────────────────────

    public string? GetString(int ordinal)
    {
        if (IsNull(ordinal)) return null;
        return Encoding.UTF8.GetString(GetRawBytes(ordinal));
    }

    public byte[]? GetBytes(int ordinal)
    {
        if (IsNull(ordinal)) return null;
        return GetRawBytes(ordinal).ToArray();
    }

    public long? GetInt64(int ordinal)
    {
        var s = GetString(ordinal);
        return s is null ? null : long.Parse(s, CultureInfo.InvariantCulture);
    }

    public int? GetInt32(int ordinal)
    {
        var s = GetString(ordinal);
        return s is null ? null
                         : int.Parse(s, CultureInfo.InvariantCulture);
    }

    public double? GetDouble(int ordinal)
    {
        var s = GetString(ordinal);
        return s is null ? null : double.Parse(s, CultureInfo.InvariantCulture);
    }

    public decimal? GetDecimal(int ordinal)
    {
        var s = GetString(ordinal);
        return s is null ? null
                         : decimal.Parse(s, CultureInfo.InvariantCulture);
    }

    public bool? GetBool(int ordinal)
    {
        var v = GetInt32(ordinal);
        return v is null ? null : v != 0;
    }

    public DateTime? GetDateTime(int ordinal)
    {
        var s = GetString(ordinal);
        if (s is null) return null;
        // MariaDB restituisce "YYYY-MM-DD HH:MM:SS" oppure "YYYY-MM-DD"
        return DateTime.Parse(s, CultureInfo.InvariantCulture);
    }


    // ── helper ───────────────────────────────────────────────────────────

    private void CheckOrdinal(int ordinal)
    {
        if ((uint)ordinal >= _fieldCount)
            throw new ArgumentOutOfRangeException(nameof(ordinal));
    }
}
