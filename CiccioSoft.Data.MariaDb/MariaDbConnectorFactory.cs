// Original portions of this file are based on MySqlConnector.
// Repo: https://github.com/mysql-net/MySqlConnector
// Original File: /src/MySqlConnector/MySqlConnectorFactory.cs (o il percorso reale del file)
// Copyright (c) 2016-2026 Bradley Grainger
// 
// Copyright (c) 2026 Francesco Crimi
// 
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Data.Common;

namespace CiccioSoft.Data.MariaDb;

/// <summary>
/// An implementation of <see cref="DbProviderFactory"/> that creates MySqlConnector objects.
/// </summary>
public sealed class MariaDbConnectorFactory : DbProviderFactory
{
    /// <summary>
    /// Provides an instance of <see cref="DbProviderFactory"/> that can create MySqlConnector objects.
    /// </summary>
    public static readonly MariaDbConnectorFactory Instance = new();

    /// <summary>
    /// Creates a new <see cref="MariaDbCommand"/> object.
    /// </summary>
    public override DbCommand CreateCommand() => new MariaDbCommand();

    /// <summary>
    /// Creates a new <see cref="MariaDbConnection"/> object.
    /// </summary>
    public override DbConnection CreateConnection() => new MariaDbConnection();

    /// <summary>
    /// Creates a new <see cref="MariaDbConnectionStringBuilder"/> object.
    /// </summary>
    public override DbConnectionStringBuilder CreateConnectionStringBuilder() => new MariaDbConnectionStringBuilder();

    /// <summary>
    /// Creates a new <see cref="MariaDbParameter"/> object.
    /// </summary>
    public override DbParameter CreateParameter() => new MariaDbParameter();

    // /// <summary>
    // /// Creates a new <see cref="MySqlCommandBuilder"/> object.
    // /// </summary>
    // public override DbCommandBuilder CreateCommandBuilder() => new MySqlCommandBuilder();

    // /// <summary>
    // /// Creates a new <see cref="MySqlDataAdapter"/> object.
    // /// </summary>
    // public override DbDataAdapter CreateDataAdapter() => new MySqlDataAdapter();

    /// <summary>
    /// Returns <c>false</c>.
    /// </summary>
    /// <remarks><see cref="DbDataSourceEnumerator"/> is not supported by MySqlConnector.</remarks>
    public override bool CanCreateDataSourceEnumerator => false;

    /// <summary>
    /// Returns <c>true</c>.
    /// </summary>
    public override bool CanCreateCommandBuilder => false;

    /// <summary>
    /// Returns <c>true</c>.
    /// </summary>
    public override bool CanCreateDataAdapter => false;

    // /// <summary>
    // /// Creates a new <see cref="MySqlBatch"/> object.
    // /// </summary>
    // public override DbBatch CreateBatch() => new MySqlBatch();

    // /// <summary>
    // /// Creates a new <see cref="MySqlBatchCommand"/> object.
    // /// </summary>
    // public override DbBatchCommand CreateBatchCommand() => new MySqlBatchCommand();

    /// <summary>
    /// Returns <c>true</c>.
    /// </summary>
    public override bool CanCreateBatch => false;

    // /// <summary>
    // /// Creates a new <see cref="MySqlDataSource"/> object.
    // /// </summary>
    // /// <param name="connectionString">The connection string.</param>
    // public override DbDataSource CreateDataSource(string connectionString) => new MySqlDataSource(connectionString);

    private MariaDbConnectorFactory()
    {
    }
}
