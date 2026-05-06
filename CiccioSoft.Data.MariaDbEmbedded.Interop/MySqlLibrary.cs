// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Threading;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

public static class MySqlLibrary
{
    private static int _initialized = 0;

    public static void Initialize()
    {
        EnsureInitialized();
    }

    internal static void EnsureInitialized()
    {
        if (Interlocked.CompareExchange(ref _initialized, 1, 0) != 0)
            return;

        unsafe
        {
            int rc = MySqlNative.mysql_server_init(0, null, null);
            if (rc != 0)
            {
                Interlocked.Exchange(ref _initialized, 0);
                throw new InvalidOperationException(
                    "mysql_library_init failed. Verificare che libmariadb sia installata.");
            }
        }
    }

    public static void Shutdown()
    {
        if (Interlocked.CompareExchange(ref _initialized, 0, 1) != 1)
            return;

        MySqlNative.mysql_server_end();
    }
}