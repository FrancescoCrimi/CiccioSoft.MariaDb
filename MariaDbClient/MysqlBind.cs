using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using MariaDbClient.Native;

namespace MariaDbClient;

/// <summary>
/// Costruisce e gestisce un array di <see cref="MysqlBindNative"/> per i
/// prepared statement. Mantiene pinned i buffer nativi finché non viene
/// chiamato <see cref="Dispose"/>.
/// </summary>
public sealed unsafe class MysqlBind : IDisposable
{
    private readonly int _count;

    // Memoria nativa contigua per l'array MYSQL_BIND
    private readonly MysqlBindNative[] _binds;

    // GCHandle per ogni buffer managed che deve restare pinned
    private readonly List<GCHandle> _pins = new();

    // Buffer di supporto per lunghezze e flag null (uno per bind)
    private readonly uint[] _lengths;
    private readonly byte[]  _isNull;

    public int Count => _count;

    /// <summary>Puntatore all'array nativo MYSQL_BIND (per mysql_stmt_bind_*).</summary>
    public fixed_ptr NativeArray { get; }

    public MysqlBind(int count)
    {
        _count   = count;
        _binds   = new MysqlBindNative[count];
        _lengths = new uint[count];
        _isNull  = new byte[count];
        NativeArray = new fixed_ptr(_binds, _pins);
    }

    // ------------------------------------------------------------------
    //  Setter per ogni tipo blittabile
    // ------------------------------------------------------------------

    /// <summary>Imposta il parametro come SQL NULL.</summary>
    public MysqlBind SetNull(int index, MysqlFieldType type = MysqlFieldType.Null)
    {
        _isNull[index] = 1;
        fixed (byte* pNull = _isNull)
        fixed (uint* pLen = _lengths)
        {
            _binds[index].BufferType  = type;
            _binds[index].Buffer      = null;
            _binds[index].BufferLength = 0;
            _binds[index].IsNull      = pNull + index;
            _binds[index].Length      = pLen + index;
        }
        return this;
    }

    /// <summary>Imposta un valore <c>long</c> (BIGINT).</summary>
    public MysqlBind SetInt64(int index, long value)
    {
        var buf = new long[] { value };
        PinAndSet(index, buf, MysqlFieldType.LongLong, sizeof(long));
        return this;
    }

    /// <summary>Imposta un valore <c>int</c> (INT).</summary>
    public MysqlBind SetInt32(int index, int value)
    {
        var buf = new int[] { value };
        PinAndSet(index, buf, MysqlFieldType.Long, sizeof(int));
        return this;
    }

    /// <summary>Imposta un valore <c>double</c> (DOUBLE).</summary>
    public MysqlBind SetDouble(int index, double value)
    {
        var buf = new double[] { value };
        PinAndSet(index, buf, MysqlFieldType.Double, sizeof(double));
        return this;
    }

    /// <summary>Imposta una stringa UTF-8 (VARCHAR / TEXT).</summary>
    public MysqlBind SetString(int index, string? value)
    {
        if (value is null) return SetNull(index, MysqlFieldType.VarString);
        return SetBytes(index, Encoding.UTF8.GetBytes(value), MysqlFieldType.VarString);
    }

    /// <summary>Imposta un array di byte (BLOB / BINARY).</summary>
    public MysqlBind SetBytes(int index, byte[]? value,
        MysqlFieldType type = MysqlFieldType.Blob)
    {
        if (value is null) return SetNull(index, type);
        PinAndSet(index, value, type, value.Length);
        return this;
    }

    /// <summary>Imposta una data/ora tramite <see cref="MysqlTimeNative"/>.</summary>
    public MysqlBind SetDateTime(int index, DateTime value,
        MysqlFieldType type = MysqlFieldType.Datetime)
    {
        var t = new MysqlTimeNative[]
        {
            new()
            {
                Year   = (uint)value.Year,
                Month  = (uint)value.Month,
                Day    = (uint)value.Day,
                Hour   = (uint)value.Hour,
                Minute = (uint)value.Minute,
                Second = (uint)value.Second,
                SecondPart = (ulong)(value.Millisecond * 1000)
            }
        };
        PinAndSet(index, t, type, sizeof(MysqlTimeNative));
        return this;
    }

    /// <summary>Imposta un <c>decimal</c> come stringa (compatibile con DECIMAL).</summary>
    public MysqlBind SetDecimal(int index, decimal value)
        => SetString(index, value.ToString(System.Globalization.CultureInfo.InvariantCulture));

    // ------------------------------------------------------------------
    //  Accesso al puntatore pinned dell'array nativo
    // ------------------------------------------------------------------

    /// <summary>
    /// Restituisce il puntatore all'array MYSQL_BIND pinned.
    /// Valido fino a <see cref="Dispose"/>.
    /// </summary>
    internal MysqlBindNative* GetNativePtr()
    {
        fixed (MysqlBindNative* p = _binds)
            return p; // sicuro solo in contesto fixed; usare NativeArray per lunga durata
    }

    // ------------------------------------------------------------------
    //  Helpers
    // ------------------------------------------------------------------

    private void PinAndSet(int index, Array buffer, MysqlFieldType type, int bufLen)
    {
        var h = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        _pins.Add(h);

        _lengths[index] = (uint)bufLen;
        _isNull[index]  = 0;

        fixed (uint* pLen = _lengths)
        fixed (byte*  pNull = _isNull)
        {
            _binds[index].Buffer       = (void*)h.AddrOfPinnedObject();
            _binds[index].BufferType   = type;
            _binds[index].BufferLength = (uint)bufLen;
            _binds[index].Length       = pLen + (uint)index;
            _binds[index].IsNull       = pNull + index;
        }
    }

    public void Dispose()
    {
        foreach (var h in _pins)
            if (h.IsAllocated) h.Free();
        _pins.Clear();
    }

    // ------------------------------------------------------------------
    //  Classe helper per mantenere l'array pinned per tutta la vita del MysqlBind
    // ------------------------------------------------------------------

    /// <summary>
    /// Mantiene un GCHandle pinned sull'array MYSQL_BIND e ne espone il puntatore.
    /// </summary>
    public sealed class fixed_ptr : IDisposable
    {
        private GCHandle _handle;

        public MysqlBindNative* Ptr { get; }

        internal fixed_ptr(MysqlBindNative[] arr, List<GCHandle> pins)
        {
            _handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            pins.Add(_handle);
            Ptr = (MysqlBindNative*)_handle.AddrOfPinnedObject();
        }

        public void Dispose()
        {
            if (_handle.IsAllocated) _handle.Free();
        }
    }
}
