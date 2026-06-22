// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Globalization;
using System.Text;

namespace CiccioSoft.MariaDb.Interop;

/// <summary>
/// Current row from a native result set.
/// This is a <c>ref struct</c>: it cannot be stored beyond the read loop.
/// Internal pointers become invalid after the next call to
/// <see cref="MySqlResult.FetchRow"/> or <see cref="MySqlResult.Dispose"/>.
/// </summary>
public readonly unsafe ref struct MySqlRow
{
    private readonly ReadOnlySpan<nint> _rowSpan;
    private readonly ReadOnlySpan<uint> _lengthsSpan;
    private readonly MySqlField[] _fields;

    public int FieldCount => _fields.Length;

    internal MySqlRow(ReadOnlySpan<nint> rowSpan, ReadOnlySpan<uint> lengthsSpan, MySqlField[] fields)
    {
        _rowSpan = rowSpan;
        _lengthsSpan = lengthsSpan;
        _fields = fields;
    }


    /// <summary>
    /// Checks whether the column value is SQL NULL.
    /// Maps to <c>mysql_fetch_row</c> which returns NULL pointers for SQL NULL values.
    /// </summary>
    public bool IsNull(int ordinal)
    {
        CheckOrdinal(ordinal);
        // return _row[ordinal] == null;
        return _rowSpan[ordinal] == 0;
    }


    /// <summary>
    /// Returns raw column bytes without allocations.
    /// Useful for BLOB values or custom conversions.
    /// </summary>
    public ReadOnlySpan<byte> GetRawBytes(int ordinal)
    {
        CheckOrdinal(ordinal);
        // byte* col = _row[ordinal];
        // if (col == null) return ReadOnlySpan<byte>.Empty;
        // var span  = new ReadOnlySpan<byte>(col, (int)_lengths[ordinal]);

        nint col2 = _rowSpan[ordinal];
        uint len2 = _lengthsSpan[ordinal];
        var span2 = new ReadOnlySpan<byte>(col2.ToPointer(), (int)len2);

        return span2;
    }

    #region Tipi managed

    public string? GetString(int ordinal)
    {
        if (IsNull(ordinal)) return null;
        return Encoding.UTF8.GetString(GetRawBytes(ordinal));
    }

    public string? GetString(string fieldName) => GetString(IndexOf(fieldName));

    public byte[]? GetBytes(int ordinal)
    {
        if (IsNull(ordinal)) return null;
        return GetRawBytes(ordinal).ToArray();
    }

    public int? GetInt32(int ordinal)
    {
        var s = GetString(ordinal);
        return s is null ? null
                         : int.Parse(s, CultureInfo.InvariantCulture);
    }

    public int? GetInt32(string fieldName) => GetInt32(IndexOf(fieldName));

    public long? GetInt64(int ordinal)
    {
        var s = GetString(ordinal);
        return s is null ? null : long.Parse(s, CultureInfo.InvariantCulture);
    }

    public long? GetInt64(string fieldName) => GetInt64(IndexOf(fieldName));

    public double? GetDouble(int ordinal)
    {
        var s = GetString(ordinal);
        return s is null ? null : double.Parse(s, CultureInfo.InvariantCulture);
    }

    public double? GetDouble(string fieldName) => GetDouble(IndexOf(fieldName));

    public decimal? GetDecimal(int ordinal)
    {
        var s = GetString(ordinal);
        return s is null ? null
                         : decimal.Parse(s, CultureInfo.InvariantCulture);
    }

    public decimal? GetDecimal(string fieldName) => GetDecimal(IndexOf(fieldName));

    public DateTime? GetDateTime(int ordinal)
    {
        var s = GetString(ordinal);
        if (s is null) return null;
        // MariaDB returns "YYYY-MM-DD HH:MM:SS" or "YYYY-MM-DD"
        return DateTime.Parse(s, CultureInfo.InvariantCulture);
    }

    public DateTime? GetDateTime(string fieldName) => GetDateTime(IndexOf(fieldName));

    public bool? GetBool(int ordinal)
    {
        var v = GetInt32(ordinal);
        return v is null ? null : v != 0;
    }

    public bool? GetBool(string fieldName) => GetBool(IndexOf(fieldName));

    #endregion


    #region Helper

    private void CheckOrdinal(int ordinal)
    {
        if (ordinal >= _fields.Length)
            throw new ArgumentOutOfRangeException(nameof(ordinal));
    }

    private int IndexOf(string name)
    {
        for (int i = 0; i < _fields.Length; i++)
            if (string.Equals(_fields[i].Name, name, StringComparison.OrdinalIgnoreCase))
                return i;
        throw new ArgumentException($"Field '{name}' was not found in the row.");
    }

    #endregion


    public override string ToString()
    {
        var parts = new string[_fields.Length];
        for (int i = 0; i < _fields.Length; i++)
            parts[i] = $"{_fields[i].Name}={GetString(i) ?? "NULL"}";
        return string.Join(", ", parts);
    }
}
