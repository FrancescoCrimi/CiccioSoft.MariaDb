// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

internal static class Utils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Forza il JIT a eliminare la chiamata al metodo
    internal unsafe static string GetStringFromPointerBytes(byte* pBytes)
    {
        if (pBytes == null)
            return string.Empty;

        int nbBytes = 0;
        while (pBytes[nbBytes] != 0)
            nbBytes++;

        ReadOnlySpan<byte> span = new(pBytes, nbBytes);

        return Encoding.UTF8.GetString(span);
    }
}
