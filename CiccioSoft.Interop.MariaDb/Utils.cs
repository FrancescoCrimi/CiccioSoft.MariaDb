// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace CiccioSoft.Interop.MariaDb;

internal static class Utils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)] // Encourages JIT inlining to eliminate call overhead
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

    internal static ReadOnlySpan<byte> BuildUtf8NullTerminated(string value)
    {
        Span<byte> buffer;

        if (string.IsNullOrEmpty(value))
        {
            buffer = new byte[1];
            buffer[0] = 0;
            return buffer;
        }

        int byteCount = Encoding.UTF8.GetByteCount(value);
        buffer = new byte[byteCount + 1];
        Encoding.UTF8.GetBytes(value, buffer);
        buffer[byteCount] = 0;
        return buffer;
    }
}
