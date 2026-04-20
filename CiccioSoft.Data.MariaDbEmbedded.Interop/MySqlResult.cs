// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;
using Microsoft.Win32.SafeHandles;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

public sealed class MySqlResultHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal MySqlResultHandle(nint handle) : base(true)
    {
        SetHandle(handle);
    }

    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        NativeMySql.mysql_free_result(handle);
        return true;
    }
}

public sealed unsafe class MySqlResult : IDisposable
{
    private readonly MySqlResultHandle _handle;

    internal MySqlResult(MySqlResultHandle handle)
    {
        _handle = handle;
    }

    public void Dispose()
    {
        _handle.Dispose();
    }
}
