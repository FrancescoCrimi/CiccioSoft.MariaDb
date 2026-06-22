// Original portions of this file are based on MySqlConnector.
// Repo: https://github.com/mysql-net/MySqlConnector
// Original File: /src/MySqlConnector/MySqlDateTimeKind.cs (o il percorso reale del file)
// Copyright (c) 2016-2026 Bradley Grainger
// 
// Copyright (c) 2026 Francesco Crimi
// 
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace CiccioSoft.Data.MariaDb;

/// <summary>
/// The <see cref="DateTimeKind" /> used when reading <see cref="DateTime" /> from the database.
/// </summary>
public enum MariaDbDateTimeKind
{
	/// <summary>
	/// Use <see cref="DateTimeKind.Unspecified" /> when reading; allow any <see cref="DateTimeKind" /> in command parameters.
	/// </summary>
	Unspecified = DateTimeKind.Unspecified,

	/// <summary>
	/// Use <see cref="DateTimeKind.Utc" /> when reading; reject <see cref="DateTimeKind.Local" /> in command parameters.
	/// </summary>
	Utc = DateTimeKind.Utc,

	/// <summary>
	/// Use <see cref="DateTimeKind.Local" /> when reading; reject <see cref="DateTimeKind.Utc" /> in command parameters.
	/// </summary>
	Local = DateTimeKind.Local,
}
