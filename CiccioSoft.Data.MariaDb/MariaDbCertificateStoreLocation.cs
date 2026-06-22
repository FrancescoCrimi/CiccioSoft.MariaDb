// Original portions of this file are based on MySqlConnector.
// Repo: https://github.com/mysql-net/MySqlConnector
// Original File: /src/MySqlConnector/MySqlCertificateStoreLocation.cs (o il percorso reale del file)
// Copyright (c) 2016-2026 Bradley Grainger
// 
// Copyright (c) 2026 Francesco Crimi
// 
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace CiccioSoft.Data.MariaDb;

public enum MariaDbCertificateStoreLocation
{
	/// <summary>
	/// Do not use certificate store
	/// </summary>
	None,

	/// <summary>
	/// Use certificate store for the current user
	/// </summary>
	CurrentUser,

	/// <summary>
	/// User certificate store for the machine
	/// </summary>
	LocalMachine,
}
