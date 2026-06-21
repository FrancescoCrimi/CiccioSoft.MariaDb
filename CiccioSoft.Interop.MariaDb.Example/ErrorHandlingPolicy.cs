// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace CiccioSoft.Interop.MariaDb.Example;

internal static class ErrorHandlingPolicy
{
    internal static int Run(string operationName, Action action)
    {
        try
        {
            action();
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[ERROR] {operationName} failed.");
            Console.Error.WriteLine(ex.Message);
            return 1;
        }
    }
}
