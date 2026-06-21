// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using CiccioSoft.Interop.MariaDb.Native;

namespace CiccioSoft.Interop.MariaDb;

/// <summary>
/// Helper that builds an array of <c>MYSQL_BIND</c> entries consumed by
/// <c>mysql_stmt_bind_param</c> and <c>mysql_stmt_bind_result</c>.
/// </summary>
public sealed class MySqlBindBuilder : IDisposable
{
    private readonly int _count;
    private MySqlBind[] _mySqlBinds;

    public MySqlBind this[int index]
    {
        get { return _mySqlBinds[index]; }
    }

    /// <summary>
    /// Creates <paramref name="count"/> managed bind slots for a statement.
    /// </summary>
    public MySqlBindBuilder(int count)
    {
        _count = count;
        _mySqlBinds = new MySqlBind[count];
        for (int i = 0; i < count; i++)
            _mySqlBinds[i] = new MySqlBind();
    }

    internal MySqlBindNative[] GetNativeArray()
    {
        var binds = new MySqlBindNative[_count];
        for (int i = 0; i < _count; i++)
            binds[i] = _mySqlBinds[i].Native;
        return binds;
    }

    /// <summary>
    /// Releases every pinned bind buffer previously prepared for statement bind APIs.
    /// </summary>
    public void Dispose()
    {
        foreach (var bind in _mySqlBinds)
            bind.Dispose();
    }
}