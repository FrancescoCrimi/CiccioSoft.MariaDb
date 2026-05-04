// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;
using System.Text;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

public unsafe sealed class MySqlBind : IDisposable
{
    private MySqlBindNative _bind;

    uint[] _length;
    byte[] _isNull;

    long[]? _longBuffer;
    int[]? _intBuffer;
    double[]? _doubleBuffer;
    byte[]? _byteBuffer;
    st_mysql_time[]? _timeBuffer;

    GCHandle _hBuffer;
    GCHandle _hLength;
    GCHandle _hIsNull;

    internal ref MySqlBindNative Native => ref _bind;

    public MySqlBind()
    {
        _bind = new MySqlBindNative();

        _length = new uint[1];
        _isNull = new byte[1];

        _hLength = GCHandle.Alloc(_length, GCHandleType.Pinned);
        _hIsNull = GCHandle.Alloc(_isNull, GCHandleType.Pinned);

        _bind.length = (uint*)_hLength.AddrOfPinnedObject();
        _bind.is_null = (byte*)_hIsNull.AddrOfPinnedObject();
    }

    public void SetFieldType(MySqlFieldTypes type, int? length = 0)
    {
        _bind.buffer_type = type;
        if (type == MySqlFieldTypes.MYSQL_TYPE_VAR_STRING && length is int nnlength && nnlength != 0)
        {
            _byteBuffer = new byte[nnlength];
            _hBuffer = GCHandle.Alloc(_byteBuffer, GCHandleType.Pinned);
            _bind.buffer = (void*)_hBuffer.AddrOfPinnedObject();
            _bind.buffer_length = (uint)_byteBuffer.Length;
        }
        if (type == MySqlFieldTypes.MYSQL_TYPE_LONG)
        {
            _intBuffer = new int[1];
            _hBuffer = GCHandle.Alloc(_intBuffer, GCHandleType.Pinned);
            _bind.buffer = (void*)_hBuffer.AddrOfPinnedObject();
            _bind.buffer_length = sizeof(int);
        }
    }

    #region Set Value

    /// <summary>Imposta il parametro come SQL NULL.</summary>
    public void SetNull()
    {
        if (_hBuffer.IsAllocated)
            _hBuffer.Free();

        _isNull[0] = 1;

        _bind.buffer = null;
        _bind.buffer_type = MySqlFieldTypes.MYSQL_TYPE_NULL;
        _bind.buffer_length = 0;
    }

    /// <summary>
    /// Imposta un valore <c>long</c> (BIGINT).
    /// </summary>
    public void SetInt64(long value)
    {
        EnsureBufferFreed();
        _longBuffer = [value];
        _hBuffer = GCHandle.Alloc(_longBuffer, GCHandleType.Pinned);

        SetNotNull();
        _length[0] = sizeof(long);

        _bind.buffer = (void*)_hBuffer.AddrOfPinnedObject();
        _bind.buffer_type = MySqlFieldTypes.MYSQL_TYPE_LONGLONG;
        _bind.buffer_length = sizeof(long);
    }

    /// <summary>Imposta un valore <c>int</c> (INT).</summary>
    public void SetInt32(int value)
    {
        EnsureBufferFreed();
        _intBuffer = [value];
        _hBuffer = GCHandle.Alloc(_intBuffer, GCHandleType.Pinned);

        SetNotNull();
        _length[0] = sizeof(int);

        _bind.buffer = (void*)_hBuffer.AddrOfPinnedObject();
        _bind.buffer_type = MySqlFieldTypes.MYSQL_TYPE_LONG;
        _bind.buffer_length = sizeof(int);
    }

    /// <summary>
    /// Imposta un valore <c>double</c> (DOUBLE).
    /// </summary>
    public void SetDouble(double value)
    {
        EnsureBufferFreed();
        _doubleBuffer = [value];
        _hBuffer = GCHandle.Alloc(_doubleBuffer, GCHandleType.Pinned);

        SetNotNull();
        _length[0] = sizeof(double);

        _bind.buffer = (void*)_hBuffer.AddrOfPinnedObject();
        _bind.buffer_type = MySqlFieldTypes.MYSQL_TYPE_DOUBLE;
        _bind.buffer_length = sizeof(double);
    }

    /// <summary>Imposta una stringa UTF-8 (VARCHAR / TEXT).</summary>
    public void SetString(string value)
    {
        EnsureBufferFreed();
        _byteBuffer = Encoding.UTF8.GetBytes(value);
        _hBuffer = GCHandle.Alloc(_byteBuffer, GCHandleType.Pinned);

        SetNotNull();
        _length[0] = (uint)_byteBuffer.Length;

        _bind.buffer = (void*)_hBuffer.AddrOfPinnedObject();
        _bind.buffer_type = MySqlFieldTypes.MYSQL_TYPE_VAR_STRING;
        _bind.buffer_length = (uint)_byteBuffer.Length;
    }

        public void SetString(int length)
    {
        EnsureBufferFreed();
        _byteBuffer = new byte[length];
        _hBuffer = GCHandle.Alloc(_byteBuffer, GCHandleType.Pinned);

        SetNotNull();
        _length[0] = (uint)_byteBuffer.Length;

        _bind.buffer = (void*)_hBuffer.AddrOfPinnedObject();
        _bind.buffer_type = MySqlFieldTypes.MYSQL_TYPE_VAR_STRING;
        _bind.buffer_length = (uint)_byteBuffer.Length;
    }

    /// <summary>Imposta un array di byte (BLOB / BINARY).</summary>
    public void SetBytes(byte[]? value,
        MySqlFieldTypes type = MySqlFieldTypes.MYSQL_TYPE_BLOB)
    {
        if (value is null)
        {
            _bind.is_null_value = 1;
            _bind.buffer_type = type;
            _bind.buffer = null;
            _bind.buffer_length = 0;
            return;
        }

        EnsureBufferFreed();
        _byteBuffer = value;
        _hBuffer = GCHandle.Alloc(value, GCHandleType.Pinned);

        SetNotNull();
        _length[0] = (uint)value.Length;

        _bind.buffer = (void*)_hBuffer.AddrOfPinnedObject();
        _bind.buffer_type = type;
        _bind.buffer_length = (uint)value.Length;
    }

    /// <summary>Imposta una data/ora tramite <see cref="MysqlTimeNative"/>.</summary>
    public void SetDateTime(DateTime value,
        MySqlFieldTypes type = MySqlFieldTypes.MYSQL_TYPE_DATETIME2)
    {
        EnsureBufferFreed();
        _timeBuffer = new st_mysql_time[]
        {
            new()
            {
                year   = (uint)value.Year,
                month  = (uint)value.Month,
                day    = (uint)value.Day,
                hour   = (uint)value.Hour,
                minute = (uint)value.Minute,
                second = (uint)value.Second,
                second_part = (uint)(value.Millisecond * 1000)
            }
        };
        _hBuffer = GCHandle.Alloc(_timeBuffer, GCHandleType.Pinned);

        SetNotNull();
        _length[0] = (uint)sizeof(st_mysql_time);

        _bind.buffer = (void*)_hBuffer.AddrOfPinnedObject();
        _bind.buffer_type = type;
        _bind.buffer_length = (uint)sizeof(st_mysql_time);
    }

    /// <summary>Imposta un <c>decimal</c> come stringa (compatibile con DECIMAL).</summary>
    public void SetDecimal(decimal value)
        => SetString(value.ToString(System.Globalization.CultureInfo.InvariantCulture));

    #endregion


    #region Get Value

    public long GetInt64()
    {
        if (_longBuffer == null) throw new Exception("Errore del Cazzo");
        return _longBuffer[0];
    }

    public int GetInt32()
    {
        if (_intBuffer == null) throw new Exception("Errore del Cazzo");
        return _intBuffer[0];
    }

    public double GetDouble()
    {
        if (_doubleBuffer == null) throw new Exception("Errore del Cazzo");
        return _doubleBuffer[0];
    }

    public string GetString()
    {
        if (_byteBuffer == null) throw new Exception("Errore del Cazzo");
        int count = (int)_length[0];
        string str = Encoding.UTF8.GetString(_byteBuffer, 0, count);
        return str;
    }

    public byte[] GetBytes()
    {
        if (_byteBuffer == null) throw new Exception("Errore del Cazzo");
        return _byteBuffer;
    }

    public DateTime GetDateTime()
    {
        if (_timeBuffer == null) throw new Exception("Errore del Cazzo");
        DateTime datetime = new DateTime(
            year: (int)_timeBuffer[0].year,
            month: (int)_timeBuffer[0].month,
            day: (int)_timeBuffer[0].day,
            hour: (int)_timeBuffer[0].hour,
            minute: (int)_timeBuffer[0].minute,
            second: (int)_timeBuffer[0].second,
            millisecond: (int)_timeBuffer[0].second_part / 1000);
        return datetime;
    }

    public decimal GetDecimal()
    {
        throw new NotImplementedException();
    }


    #endregion 

    public void Clear()
    {
        if (_longBuffer != null) Array.Clear(_longBuffer);
        if (_intBuffer != null) Array.Clear(_intBuffer);
        if (_doubleBuffer != null) Array.Clear(_doubleBuffer);
        if (_byteBuffer != null) Array.Clear(_byteBuffer);
        Array.Clear(_length);
        Array.Clear(_isNull);
    }

    #region Helper

    private void SetNotNull()
    {
        _isNull[0] = 0;
        _bind.is_null_value = 0;
    }

    private void EnsureBufferFreed()
    {
        if (_hBuffer.IsAllocated)
            _hBuffer.Free();
    }

    #endregion

    public void Dispose()
    {
        _hBuffer.Free();
        _hLength.Free();
        _hIsNull.Free();
    }
}
