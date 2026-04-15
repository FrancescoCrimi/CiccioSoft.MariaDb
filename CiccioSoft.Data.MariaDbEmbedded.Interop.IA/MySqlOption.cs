// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.IA
{
    /// <summary>
    /// Identifies options that can be configured on a native <c>MYSQL*</c> handle
    /// through the <c>mysql_options</c> API.
    /// </summary>
    public enum MySqlOption
    {
        /// <summary>
        /// Configures the connection timeout in seconds.
        /// </summary>
        ConnectTimeout = 0,

        /// <summary>
        /// Enables or disables automatic reconnect logic.
        /// </summary>
        Reconnect = 20,

        /// <summary>
        /// Configures the timeout used for socket reads in seconds.
        /// </summary>
        ReadTimeout = 11,

        /// <summary>
        /// Configures the timeout used for socket writes in seconds.
        /// </summary>
        WriteTimeout = 12,

        /// <summary>
        /// Selects the default character set used by the connection.
        /// </summary>
        SetCharsetName = 7,
    }
}
