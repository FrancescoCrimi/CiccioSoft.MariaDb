// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

public sealed class MySqlException : Exception
{
    public int ErrorCode { get; }
    public string SqlState { get; }

    public MySqlException(string message, int errorCode, string sqlState = "")
        : base(message)
    {
        ErrorCode = errorCode;
        SqlState  = sqlState;
    }

    // factory da handle nativo
    internal static unsafe MySqlException FromHandle(nint handle)
    {
        byte* pMsg   = NativeMySql.mysql_error(handle);
        byte* pState = NativeMySql.mysql_sqlstate(handle);
        uint  errno  = NativeMySql.mysql_errno(handle);
        return new MySqlException(
            Utils.GetStringFromPointerBytes(pMsg),
            (int)errno,
            Utils.GetStringFromPointerBytes(pState));
    }
}
