// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace CiccioSoft.MariaDb.Interop.Example;

internal static class Program
{
    static int Main(string[] args)
    {
        return ErrorHandlingPolicy.Run("Interop example", () =>
        {
            using var mysql = ConnectionSetup.OpenDefaultConnection();
            DemoScenarios.RunAll(mysql);
        });
    }
}
