// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

// Classe helper per costruire array di MYSQL_BIND
public sealed class MySqlBindBuilder : IDisposable
{
    private readonly int _count;
    private MySqlBind[] _mySqlBinds;

    public MySqlBind this[int index]
    {
        get { return _mySqlBinds[index]; }
    }

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

    public void Dispose()
    {
        foreach (var bind in _mySqlBinds)
            bind.Dispose();
    }
}