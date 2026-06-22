// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace CiccioSoft.MariaDb.Interop;

public sealed class MySqlException : Exception
{
    public int ErrorCode { get; }
    public string SqlState { get; }

    public MySqlException(string message, int errorCode, string sqlState = "")
        : base(message)
    {
        ErrorCode = errorCode;
        SqlState = sqlState;
    }
}
