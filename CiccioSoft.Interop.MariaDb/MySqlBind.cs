// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;
using System.Text;
using CiccioSoft.Interop.MariaDb.Native;

namespace CiccioSoft.Interop.MariaDb;

public unsafe sealed class MySqlBind : IDisposable
{
    private MySqlBindNative _bind;

    uint[] _length;
    byte[] _isNull;

    long[]? _longBuffer;
    int[]? _intBuffer;
    short[]? _shortBuffer;
    float[]? _floatBuffer;
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

    /// <summary>Sets the parameter to SQL NULL.</summary>
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
    /// Sets a <c>long</c> (BIGINT) value.
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

    /// <summary>Sets an <c>int</c> (INT) value.</summary>
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

    public void SetInt16(short value)
    {
        EnsureBufferFreed();
        _shortBuffer = [value];
        _hBuffer = GCHandle.Alloc(_shortBuffer, GCHandleType.Pinned);

        SetNotNull();
        _length[0] = sizeof(short);

        _bind.buffer = (void*)_hBuffer.AddrOfPinnedObject();
        _bind.buffer_type = MySqlFieldTypes.MYSQL_TYPE_SHORT;
        _bind.buffer_length = sizeof(short);
    }

    public void SetFloat(float value)
    {
        EnsureBufferFreed();
        _floatBuffer = [value];
        _hBuffer = GCHandle.Alloc(_floatBuffer, GCHandleType.Pinned);

        SetNotNull();
        _length[0] = sizeof(float);

        _bind.buffer = (void*)_hBuffer.AddrOfPinnedObject();
        _bind.buffer_type = MySqlFieldTypes.MYSQL_TYPE_FLOAT;
        _bind.buffer_length = sizeof(float);
    }

    /// <summary>
    /// Sets a <c>double</c> (DOUBLE) value.
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

    /// <summary>Sets a UTF-8 string value (VARCHAR / TEXT).</summary>
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

    /// <summary>Sets a byte array value (BLOB / BINARY).</summary>
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

    /// <summary>Sets a date/time value through <see cref="MysqlTimeNative"/>.</summary>
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

    /// <summary>Sets a <c>decimal</c> value as a string (DECIMAL-compatible).</summary>
    public void SetDecimal(decimal value)
        => SetString(value.ToString(System.Globalization.CultureInfo.InvariantCulture));

    // public void SetBoolean(bool value)
    //     => SetInt16(value ? (short)1 : (short)0);

    // public void SetGuid(Guid value)
    //     => SetString(value.ToString());

    public void SetTimeSpan(TimeSpan value)
    {
        EnsureBufferFreed();
        TimeSpan normalized = value.Duration();
        _timeBuffer = new st_mysql_time[]
        {
            new()
            {
                hour = (uint)normalized.TotalHours,
                minute = (uint)normalized.Minutes,
                second = (uint)normalized.Seconds,
                second_part = (uint)(normalized.Milliseconds * 1000),
                neg = value.Ticks < 0 ? (sbyte)1 : (sbyte)0,
                time_type = enum_mysql_timestamp_type.MYSQL_TIMESTAMP_TIME
            }
        };
        _hBuffer = GCHandle.Alloc(_timeBuffer, GCHandleType.Pinned);

        SetNotNull();
        _length[0] = (uint)sizeof(st_mysql_time);

        _bind.buffer = (void*)_hBuffer.AddrOfPinnedObject();
        _bind.buffer_type = MySqlFieldTypes.MYSQL_TYPE_TIME;
        _bind.buffer_length = (uint)sizeof(st_mysql_time);
    }
    #endregion


    #region Get Value

    public long GetInt64()
    {
        if (_longBuffer == null) throw new InvalidOperationException("Buffer for long value is not initialized.");
        return _longBuffer[0];
    }

    public int GetInt32()
    {
        if (_intBuffer == null) throw new InvalidOperationException("Buffer for int value is not initialized.");
        return _intBuffer[0];
    }

    public short GetInt16()
    {
        if (_shortBuffer == null) throw new InvalidOperationException("Buffer for short value is not initialized.");
        return _shortBuffer[0];
    }

    public float GetFloat()
    {
        if (_floatBuffer == null) throw new InvalidOperationException("Buffer for float value is not initialized.");
        return _floatBuffer[0];
    }

    public double GetDouble()
    {
        if (_doubleBuffer == null) throw new InvalidOperationException("Buffer for double value is not initialized.");
        return _doubleBuffer[0];
    }

    public string GetString()
    {
        if (_byteBuffer == null) throw new InvalidOperationException("Buffer for string value is not initialized.");
        int count = (int)_length[0];
        string str = Encoding.UTF8.GetString(_byteBuffer, 0, count);
        return str;
    }

    public byte[] GetBytes()
    {
        if (_byteBuffer == null) throw new InvalidOperationException("Buffer for byte value is not initialized.");
        return _byteBuffer;
    }

    public DateTime GetDateTime()
    {
        if (_timeBuffer == null) throw new InvalidOperationException("Buffer for DateTime value is not initialized.");
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
        string value = GetString();
        return decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
    }

    // public bool GetBoolean() => GetInt16() != 0;

    // public Guid GetGuid() => Guid.Parse(GetString());

    public TimeSpan GetTimeSpan()
    {
        if (_timeBuffer == null) throw new InvalidOperationException("Buffer for TimeSpan value is not initialized.");
        TimeSpan duration = new TimeSpan(
            hours: (int)_timeBuffer[0].hour,
            minutes: (int)_timeBuffer[0].minute,
            seconds: (int)_timeBuffer[0].second)
            .Add(TimeSpan.FromMilliseconds(_timeBuffer[0].second_part / 1000d));
        return _timeBuffer[0].neg == 1 ? duration.Negate() : duration;
    }

    #endregion 

    public void Clear()
    {
        if (_longBuffer != null) Array.Clear(_longBuffer);
        if (_intBuffer != null) Array.Clear(_intBuffer);
        if (_shortBuffer != null) Array.Clear(_shortBuffer);
        if (_floatBuffer != null) Array.Clear(_floatBuffer);
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
        if (_hBuffer.IsAllocated)
            _hBuffer.Free();
        _hLength.Free();
        _hIsNull.Free();
    }
}
