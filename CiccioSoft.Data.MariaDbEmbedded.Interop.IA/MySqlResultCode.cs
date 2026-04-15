// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.IA
{
    /// <summary>
    /// Represents a simplified outcome of native MariaDB/MySQL operations.
    /// </summary>
    public enum MySqlResultCode
    {
        /// <summary>
        /// Operation completed successfully.
        /// </summary>
        Ok = 0,

        /// <summary>
        /// Operation failed.
        /// </summary>
        Error = 1
    }
}
