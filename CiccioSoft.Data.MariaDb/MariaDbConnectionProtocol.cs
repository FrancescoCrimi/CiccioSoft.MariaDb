// Original portions of this file are based on MySqlConnector.
// Repo: https://github.com/mysql-net/MySqlConnector
// Original File: /src/MySqlConnector/MySqlConnectionProtocol.cs (o il percorso reale del file)
// Copyright (c) 2016-2026 Bradley Grainger
// 
// Copyright (c) 2026 Francesco Crimi
// 
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace CiccioSoft.Data.MariaDb;

#pragma warning disable CA1027 // Mark enums with FlagsAttribute
#pragma warning disable CA1069 // Enum values should not be duplicated
/// <summary>
/// Specifies the type of connection to make to the server.
/// </summary>
public enum MariaDbConnectionProtocol
{
	/// <summary>
	/// TCP/IP connection.
	/// </summary>
	Sockets = 1,
	Socket = 1,
	Tcp = 1,

	/// <summary>
	/// Named pipe connection. Only works on Windows.
	/// </summary>
	Pipe = 2,
	NamedPipe = 2,

	/// <summary>
	/// Unix domain socket connection. Only works on Unix/Linux.
	/// </summary>
	UnixSocket = 3,
	Unix = 3,

	/// <summary>
	/// Shared memory connection. Not currently supported.
	/// </summary>
	SharedMemory = 4,
	Memory = 4,
}
#pragma warning restore CA1069 // Enum values should not be duplicated
