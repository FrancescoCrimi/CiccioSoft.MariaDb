using System;
using System.Text;
using MariaDbClient.Native;

namespace MariaDbClient;

/// <summary>
/// Rappresenta una riga del result set.
/// Dati letti direttamente dal puntatore nativo MYSQL_ROW (char**).
/// </summary>
public sealed class MysqlRow
{
    private readonly byte[][] _data;
    private readonly MysqlField[] _fields;

    /// <summary>Numero di colonne nella riga.</summary>
    public int FieldCount => _fields.Length;

    internal unsafe MysqlRow(byte** row, uint* lengths, MysqlField[] fields)
    {
        _fields = fields;
        _data   = new byte[fields.Length][];

        for (int i = 0; i < fields.Length; i++)
        {
            if (row[i] == null)
            {
                _data[i] = [];
            }
            else
            {
                uint len = lengths[i];
                _data[i] = new byte[len];
                fixed (byte* dst = _data[i])
                    Buffer.MemoryCopy(row[i], dst, len, len);
            }
        }
    }

    // ------------------------------------------------------------------
    //  Accesso per indice
    // ------------------------------------------------------------------

    /// <summary>
    /// Restituisce i byte raw della colonna <paramref name="index"/>.
    /// Restituisce <c>null</c> se il valore SQL è NULL.
    /// </summary>
    public byte[]? GetRawBytes(int index) =>
        IsDbNull(index) ? null : _data[index];

    /// <summary>Verifica se la colonna è SQL NULL.</summary>
    public bool IsDbNull(int index) => _data[index].Length == 0 && IsNullInternal(index);

    // Distinzione NULL vs stringa vuota: MYSQL_ROW entry == null → NULL SQL
    // Poiché abbiamo già copiato i bytes, teniamo un flag separato.
    // (La lunghezza 0 da sola non basta: "" ha length=0 ma non è NULL.)
    private readonly bool[] _isNull;

    internal unsafe MysqlRow(byte** row, uint* lengths, MysqlField[] fields, bool trackNull)
    {
        _fields = fields;
        _data   = new byte[fields.Length][];
        _isNull = new bool[fields.Length];

        for (int i = 0; i < fields.Length; i++)
        {
            _isNull[i] = row[i] == null;
            if (_isNull[i])
            {
                _data[i] = [];
            }
            else
            {
                uint len = lengths[i];
                _data[i] = new byte[len];
                if (len > 0)
                    fixed (byte* dst = _data[i])
                        Buffer.MemoryCopy(row[i], dst, len, len);
            }
        }
    }

    private bool IsNullInternal(int index) =>
        _isNull is not null && _isNull[index];

    // ------------------------------------------------------------------
    //  Conversioni typed
    // ------------------------------------------------------------------

    public string? GetString(int index)
    {
        if (IsNullInternal(index)) return null;
        return Encoding.UTF8.GetString(_data[index]);
    }

    public string? GetString(string fieldName) => GetString(IndexOf(fieldName));

    public int? GetInt32(int index)
    {
        var s = GetString(index);
        return s is null ? null : int.Parse(s);
    }

    public int? GetInt32(string fieldName) => GetInt32(IndexOf(fieldName));

    public long? GetInt64(int index)
    {
        var s = GetString(index);
        return s is null ? null : long.Parse(s);
    }

    public long? GetInt64(string fieldName) => GetInt64(IndexOf(fieldName));

    public double? GetDouble(int index)
    {
        var s = GetString(index);
        return s is null ? null : double.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
    }

    public double? GetDouble(string fieldName) => GetDouble(IndexOf(fieldName));

    public decimal? GetDecimal(int index)
    {
        var s = GetString(index);
        return s is null ? null : decimal.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
    }

    public decimal? GetDecimal(string fieldName) => GetDecimal(IndexOf(fieldName));

    public DateTime? GetDateTime(int index)
    {
        var s = GetString(index);
        return s is null ? null : DateTime.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
    }

    public DateTime? GetDateTime(string fieldName) => GetDateTime(IndexOf(fieldName));

    public bool? GetBool(int index)
    {
        var s = GetString(index);
        if (s is null) return null;
        return s == "1" || string.Equals(s, "true", StringComparison.OrdinalIgnoreCase);
    }

    public bool? GetBool(string fieldName) => GetBool(IndexOf(fieldName));

    /// <summary>Indexer per accesso per nome colonna.</summary>
    public string? this[string fieldName] => GetString(IndexOf(fieldName));

    /// <summary>Indexer per accesso per indice.</summary>
    public string? this[int index] => GetString(index);

    // ------------------------------------------------------------------
    //  Metadati
    // ------------------------------------------------------------------

    public MysqlField GetField(int index) => _fields[index];

    public MysqlField[] GetFields() => (MysqlField[])_fields.Clone();

    private int IndexOf(string name)
    {
        for (int i = 0; i < _fields.Length; i++)
            if (string.Equals(_fields[i].Name, name, StringComparison.OrdinalIgnoreCase))
                return i;
        throw new ArgumentException($"Campo '{name}' non trovato nella riga.");
    }

    public override string ToString()
    {
        var parts = new string[_fields.Length];
        for (int i = 0; i < _fields.Length; i++)
            parts[i] = $"{_fields[i].Name}={GetString(i) ?? "NULL"}";
        return string.Join(", ", parts);
    }
}
