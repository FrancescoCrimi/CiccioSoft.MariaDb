// Original portions of this file are based on MySqlConnector.
// Repo: https://github.com/mysql-net/MySqlConnector
// Original File: /src/MySqlConnector/MySqlServerRedirectionMode.cs (o il percorso reale del file)
// Copyright (c) 2016-2026 Bradley Grainger
// 
// Copyright (c) 2026 Francesco Crimi
// 
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace CiccioSoft.Data.MariaDb;

/// <summary>
/// Server redirection configuration.
/// </summary>
public enum MariaDbServerRedirectionMode
{
	/// <summary>
	/// Server redirection will not be performed.
	/// </summary>
	Disabled,

	/// <summary>
	/// Server redirection will occur if possible, otherwise the original connection will be used.
	/// </summary>
	Preferred,

	/// <summary>
	/// Server redirection must occur, otherwise connecting fails.
	/// </summary>
	Required,
}
