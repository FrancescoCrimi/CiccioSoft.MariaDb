// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.IA
{
    /// <summary>
    /// Exception raised when a native MariaDB/MySQL interop call fails.
    /// </summary>
    [Serializable]
    public sealed class MySqlInteropException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlInteropException"/> class.
        /// </summary>
        public MySqlInteropException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlInteropException"/> class with an error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MySqlInteropException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlInteropException"/> class
        /// with an error message and an inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that caused the current exception.</param>
        public MySqlInteropException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
