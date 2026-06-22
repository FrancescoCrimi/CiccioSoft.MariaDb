// Original portions of this file are based on MySqlConnector.
// Repo: https://github.com/mysql-net/MySqlConnector
// Original File: /src/MySqlConnector/MySqlSslMode.cs (o il percorso reale del file)
// Copyright (c) 2016-2026 Bradley Grainger
// 
// Copyright (c) 2026 Francesco Crimi
// 
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace CiccioSoft.Data.MariaDb;

/// <summary>
/// SSL connection options.
/// </summary>
#pragma warning disable CA1027 // not a [Flags] enum
public enum MariaDbSslMode
{
	/// <summary>
	/// Do not use SSL.
	/// </summary>
	None,

	/// <summary>
	/// Do not use SSL. This is the same as <see cref="None"/>.
	/// </summary>
#pragma warning disable CA1069 // Enum values should not be duplicated
	Disabled = 0,
#pragma warning restore CA1069 // Enum values should not be duplicated

	/// <summary>
	/// Use SSL if the server supports it.
	/// </summary>
	Preferred,

	/// <summary>
	/// Always use SSL. Deny connection if server does not support SSL.
	/// </summary>
	Required,

	/// <summary>
	///  Always use SSL. Validate the Certificate Authority but tolerate name mismatch.
	/// </summary>
	VerifyCA,

	/// <summary>
	/// Always use SSL. Fail if the host name is not correct.
	/// </summary>
	VerifyFull,
}
