// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace CiccioSoft.Data.MariaDb;

/// <summary>
/// Provides a way of reading a forward-only stream of rows from a MariaDB Embedded database.
/// </summary>
public sealed class MariaDbDataReader : DbDataReader
{
    private bool _isClosed;

    internal MariaDbDataReader()
    {
    }

    public override int FieldCount => 0;
    public override bool HasRows => false;
    public override bool IsClosed => _isClosed;
    public override int RecordsAffected => 0;
    public override int Depth => 0;
    public override object this[string name] => throw new IndexOutOfRangeException($"Field '{name}' was not found.");
    public override object this[int ordinal] => throw new IndexOutOfRangeException($"Field ordinal {ordinal} is out of range.");

    public override bool GetBoolean(int ordinal) => ThrowInvalidOrdinal<bool>(ordinal);
    public override byte GetByte(int ordinal) => ThrowInvalidOrdinal<byte>(ordinal);
    public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length) => ThrowInvalidOrdinal<long>(ordinal);
    public override char GetChar(int ordinal) => ThrowInvalidOrdinal<char>(ordinal);
    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length) => ThrowInvalidOrdinal<long>(ordinal);
    public override string GetDataTypeName(int ordinal) => ThrowInvalidOrdinal<string>(ordinal);
    public override DateTime GetDateTime(int ordinal) => ThrowInvalidOrdinal<DateTime>(ordinal);
    public override decimal GetDecimal(int ordinal) => ThrowInvalidOrdinal<decimal>(ordinal);
    public override double GetDouble(int ordinal) => ThrowInvalidOrdinal<double>(ordinal);
    public override Type GetFieldType(int ordinal) => ThrowInvalidOrdinal<Type>(ordinal);
    public override float GetFloat(int ordinal) => ThrowInvalidOrdinal<float>(ordinal);
    public override Guid GetGuid(int ordinal) => ThrowInvalidOrdinal<Guid>(ordinal);
    public override short GetInt16(int ordinal) => ThrowInvalidOrdinal<short>(ordinal);
    public override int GetInt32(int ordinal) => ThrowInvalidOrdinal<int>(ordinal);
    public override long GetInt64(int ordinal) => ThrowInvalidOrdinal<long>(ordinal);
    public override string GetName(int ordinal) => ThrowInvalidOrdinal<string>(ordinal);
    public override int GetOrdinal(string name) => throw new IndexOutOfRangeException($"Field '{name}' was not found.");
    public override object GetValue(int ordinal) => ThrowInvalidOrdinal<object>(ordinal);
    public override int GetValues(object[] values) => 0;
    public override bool IsDBNull(int ordinal) => ThrowInvalidOrdinal<bool>(ordinal);
    public override string GetString(int ordinal) => ThrowInvalidOrdinal<string>(ordinal);

    public override bool NextResult() => false;
    public override bool Read() => false;

    public override void Close()
    {
        _isClosed = true;
    }

    public override Task<bool> NextResultAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }

        return Task.FromResult(false);
    }

    public override Task<bool> ReadAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }

        return Task.FromResult(false);
    }

    public override Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<bool>(cancellationToken);
        }

        return ThrowInvalidOrdinal<Task<bool>>(ordinal);
    }

    public override IEnumerator GetEnumerator()
    {
        return Array.Empty<object>().GetEnumerator();
    }

    private static T ThrowInvalidOrdinal<T>(int ordinal)
    {
        throw new IndexOutOfRangeException($"Field ordinal {ordinal} is out of range.");
    }
}
