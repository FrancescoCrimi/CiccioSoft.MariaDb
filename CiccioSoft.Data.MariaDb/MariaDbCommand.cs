// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace CiccioSoft.Data.MariaDb;

/// <summary>
/// Represents a SQL command to execute against a MariaDB Embedded database.
/// </summary>
public sealed class MariaDbCommand : DbCommand
{
    private string _commandText = string.Empty;
    private int _commandTimeout;
    private CommandType _commandType;
    private MariaDbConnection? _connection;
    private readonly MariaDbParameterCollection _parameters = new();
    private MariaDbTransaction? _transaction;
    private bool _designTimeVisible;
    private UpdateRowSource _updatedRowSource = UpdateRowSource.Both;

    /// <summary>
    /// Initializes a new instance of the <see cref="MariaDbCommand"/> class.
    /// </summary>
    public MariaDbCommand()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MariaDbCommand"/> class with the specified command text.
    /// </summary>
    /// <param name="cmdText">The command text.</param>
    public MariaDbCommand(string cmdText)
    {
        CommandText = cmdText;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MariaDbCommand"/> class with the specified command text and connection.
    /// </summary>
    /// <param name="cmdText">The command text.</param>
    /// <param name="connection">The connection.</param>
    public MariaDbCommand(string cmdText, MariaDbConnection connection)
    {
        CommandText = cmdText;
        Connection = connection;
    }

    /// <inheritdoc />
#nullable disable
    public override string CommandText
    {
        get => _commandText;
        set => _commandText = value ?? string.Empty;
    }
#nullable restore

    /// <inheritdoc />
    public override int CommandTimeout
    {
        get => _commandTimeout;
        set => _commandTimeout = value;
    }

    /// <inheritdoc />
    public override CommandType CommandType
    {
        get => _commandType;
        set => _commandType = value;
    }

    /// <summary>
    /// Gets or sets the connection.
    /// </summary>
    public new MariaDbConnection? Connection
    {
        get => _connection;
        set => _connection = value;
    }

    /// <inheritdoc />
    protected override DbConnection? DbConnection
    {
        get => _connection;
        set => _connection = value as MariaDbConnection;
    }

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    public new MariaDbParameterCollection Parameters => _parameters;

    /// <inheritdoc />
    protected override DbParameterCollection DbParameterCollection => _parameters;

    /// <summary>
    /// Gets or sets the transaction.
    /// </summary>
    public new MariaDbTransaction? Transaction
    {
        get => _transaction;
        set => _transaction = value;
    }

    /// <inheritdoc />
    protected override DbTransaction? DbTransaction
    {
        get => _transaction;
        set => _transaction = value as MariaDbTransaction;
    }

    /// <inheritdoc />
    public override bool DesignTimeVisible
    {
        get => _designTimeVisible;
        set => _designTimeVisible = value;
    }

    /// <inheritdoc />
    public override UpdateRowSource UpdatedRowSource
    {
        get => _updatedRowSource;
        set => _updatedRowSource = value;
    }

    /// <inheritdoc />
    public override void Cancel()
    {
    }

    /// <summary>
    /// Creates a new parameter.
    /// </summary>
    /// <returns>A new <see cref="MariaDbParameter"/>.</returns>
    public new MariaDbParameter CreateParameter()
    {
        return new MariaDbParameter();
    }

    /// <inheritdoc />
    protected override DbParameter CreateDbParameter()
    {
        return new MariaDbParameter();
    }

    /// <inheritdoc />
    public override int ExecuteNonQuery()
    {
        return 0;
    }

    /// <inheritdoc />
    public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<int>(cancellationToken);
        }

        return Task.FromResult(ExecuteNonQuery());
    }

    /// <summary>
    /// Executes the command and returns a data reader.
    /// </summary>
    /// <returns>A <see cref="MariaDbDataReader"/>.</returns>
    public new MariaDbDataReader ExecuteReader()
    {
        return ExecuteReader(CommandBehavior.Default);
    }

    /// <summary>
    /// Executes the command and returns a data reader.
    /// </summary>
    /// <param name="behavior">The command behavior.</param>
    /// <returns>A <see cref="MariaDbDataReader"/>.</returns>
    public new MariaDbDataReader ExecuteReader(CommandBehavior behavior)
    {
        return new MariaDbDataReader();
    }

    /// <inheritdoc />
    public new Task<DbDataReader> ExecuteReaderAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<DbDataReader>(cancellationToken);
        }

        return Task.FromResult<DbDataReader>(ExecuteReader());
    }

    /// <summary>
    /// Executes the command asynchronously and returns a provider-specific data reader.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<MariaDbDataReader> ExecuteReaderAsyncTyped(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<MariaDbDataReader>(cancellationToken);
        }

        return Task.FromResult(ExecuteReader());
    }

    /// <inheritdoc />
    public new Task<DbDataReader> ExecuteReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<DbDataReader>(cancellationToken);
        }

        return Task.FromResult<DbDataReader>(ExecuteReader(behavior));
    }

    /// <summary>
    /// Executes the command asynchronously and returns a provider-specific data reader.
    /// </summary>
    /// <param name="behavior">The command behavior.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task<MariaDbDataReader> ExecuteReaderAsyncTyped(CommandBehavior behavior, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<MariaDbDataReader>(cancellationToken);
        }

        return Task.FromResult(ExecuteReader(behavior));
    }

    /// <inheritdoc />
    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        return ExecuteReader(behavior);
    }

    /// <inheritdoc />
    protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<DbDataReader>(cancellationToken);
        }

        return Task.FromResult<DbDataReader>(ExecuteDbDataReader(behavior));
    }

    /// <inheritdoc />
    public override object? ExecuteScalar()
    {
        return null;
    }

    /// <inheritdoc />
    public override Task<object?> ExecuteScalarAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<object?>(cancellationToken);
        }

        return Task.FromResult(ExecuteScalar());
    }

    /// <inheritdoc />
    public override void Prepare()
    {
    }
}
