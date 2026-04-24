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

// Classe helper per costruire array di MYSQL_BIND
public sealed unsafe class MySqlBindBuilder : IDisposable
{
    private readonly st_mysql_bind[] _binds;
    private readonly GCHandle[] _handles; // pin dei buffer
    private int _index;

    public MySqlBindBuilder(int count)
    {
        _binds = new st_mysql_bind[count];
        _handles = new GCHandle[count];
    }

    public MySqlBindBuilder AddInt32(int? value)
    {
        int i = _index++;
        if (value is null)
        {
            _binds[i].is_null_value = 1;
            _binds[i].buffer_type = MySqlFieldTypes.MYSQL_TYPE_LONG;
        }
        else
        {
            var box = new int[] { value.Value };
            _handles[i] = GCHandle.Alloc(box, GCHandleType.Pinned);
            _binds[i].buffer = (void*)_handles[i].AddrOfPinnedObject();
            _binds[i].buffer_type = MySqlFieldTypes.MYSQL_TYPE_LONG;
            _binds[i].buffer_length = sizeof(int);
        }
        return this;
    }

    public MySqlBindBuilder AddString(string? value)
    {
        int i = _index++;
        if (value is null)
        {
            _binds[i].is_null_value = 1;
            _binds[i].buffer_type = MySqlFieldTypes.MYSQL_TYPE_VAR_STRING;
        }
        else
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            _handles[i] = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            _binds[i].buffer = (void*)_handles[i].AddrOfPinnedObject();
            _binds[i].buffer_type = MySqlFieldTypes.MYSQL_TYPE_VAR_STRING;
            _binds[i].buffer_length = (uint)bytes.Length;
            _binds[i].length_value = (uint)bytes.Length;
        }
        return this;
    }

    internal st_mysql_bind[] Binds => _binds;

    public void Dispose()
    {
        foreach (var h in _handles)
            if (h.IsAllocated) h.Free();
    }
}