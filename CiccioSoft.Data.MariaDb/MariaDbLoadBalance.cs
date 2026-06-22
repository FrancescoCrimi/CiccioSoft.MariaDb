// Original portions of this file are based on MySqlConnector.
// Repo: https://github.com/mysql-net/MySqlConnector
// Original File: /src/MySqlConnector/MySqlLoadBalance.cs (o il percorso reale del file)
// Copyright (c) 2016-2026 Bradley Grainger
// 
// Copyright (c) 2026 Francesco Crimi
// 
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace CiccioSoft.Data.MariaDb;

public enum MariaDbLoadBalance
{
	/// <summary>
	/// Each new connection opened for a connection pool uses the next host name (sequentially with wraparound).
	/// </summary>
	RoundRobin,

	/// <summary>
	/// Each new connection tries to connect to the first host; subsequent hosts are used only if connecting to the first one fails.
	/// </summary>
	FailOver,

	/// <summary>
	/// Servers are tried in random order.
	/// </summary>
	Random,

	/// <summary>
	/// Servers are tried in ascending order of number of currently-open connections.
	/// </summary>
	LeastConnections,
}
