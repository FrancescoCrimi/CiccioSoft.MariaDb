// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;
using Microsoft.Win32.SafeHandles;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

public sealed class MySqlResHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal MySqlResHandle(nint handle) : base(true)
    {
        SetHandle(handle);
    }
    protected override bool ReleaseHandle()
    {
        NativeMySql.mysql_free_result(handle);
        return true;
    }
}

public sealed unsafe class MySqlRes : IDisposable
{
    private readonly MySqlResHandle _handle;

    internal MySqlRes(MySqlResHandle handle)
    {
        _handle = handle;
    }

    public void Dispose()
    {
        _handle.Dispose();
    }
}
