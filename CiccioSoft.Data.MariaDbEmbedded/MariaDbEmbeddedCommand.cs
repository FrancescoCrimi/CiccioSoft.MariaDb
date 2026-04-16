using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace CiccioSoft.Data.MariaDbEmbedded;

/// <summary>
/// Represents a SQL command to execute against a MariaDB Embedded database.
/// </summary>
public sealed class MariaDbEmbeddedCommand : DbCommand
{
    private string _commandText = string.Empty;
    private int _commandTimeout;
    private CommandType _commandType;
    private MariaDbEmbeddedConnection? _connection;
    private readonly MariaDbEmbeddedParameterCollection _parameters = new();
    private MariaDbEmbeddedTransaction? _transaction;
    private bool _designTimeVisible;
    private UpdateRowSource _updatedRowSource = UpdateRowSource.Both;

    /// <summary>
    /// Initializes a new instance of the <see cref="MariaDbEmbeddedCommand"/> class.
    /// </summary>
    public MariaDbEmbeddedCommand()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MariaDbEmbeddedCommand"/> class with the specified command text.
    /// </summary>
    /// <param name="cmdText">The command text.</param>
    public MariaDbEmbeddedCommand(string cmdText)
    {
        CommandText = cmdText;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MariaDbEmbeddedCommand"/> class with the specified command text and connection.
    /// </summary>
    /// <param name="cmdText">The command text.</param>
    /// <param name="connection">The connection.</param>
    public MariaDbEmbeddedCommand(string cmdText, MariaDbEmbeddedConnection connection)
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
    public new MariaDbEmbeddedConnection? Connection
    {
        get => _connection;
        set => _connection = value;
    }

    /// <inheritdoc />
    protected override DbConnection? DbConnection
    {
        get => _connection;
        set => _connection = value as MariaDbEmbeddedConnection;
    }

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    public new MariaDbEmbeddedParameterCollection Parameters => _parameters;

    /// <inheritdoc />
    protected override DbParameterCollection DbParameterCollection => _parameters;

    /// <summary>
    /// Gets or sets the transaction.
    /// </summary>
    public new MariaDbEmbeddedTransaction? Transaction
    {
        get => _transaction;
        set => _transaction = value;
    }

    /// <inheritdoc />
    protected override DbTransaction? DbTransaction
    {
        get => _transaction;
        set => _transaction = value as MariaDbEmbeddedTransaction;
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
    /// <returns>A new <see cref="MariaDbEmbeddedParameter"/>.</returns>
    public new MariaDbEmbeddedParameter CreateParameter()
    {
        return new MariaDbEmbeddedParameter();
    }

    /// <inheritdoc />
    protected override DbParameter CreateDbParameter()
    {
        return new MariaDbEmbeddedParameter();
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
    /// <returns>A <see cref="MariaDbEmbeddedDataReader"/>.</returns>
    public new MariaDbEmbeddedDataReader ExecuteReader()
    {
        return ExecuteReader(CommandBehavior.Default);
    }

    /// <summary>
    /// Executes the command and returns a data reader.
    /// </summary>
    /// <param name="behavior">The command behavior.</param>
    /// <returns>A <see cref="MariaDbEmbeddedDataReader"/>.</returns>
    public new MariaDbEmbeddedDataReader ExecuteReader(CommandBehavior behavior)
    {
        return new MariaDbEmbeddedDataReader();
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
    public Task<MariaDbEmbeddedDataReader> ExecuteReaderAsyncTyped(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<MariaDbEmbeddedDataReader>(cancellationToken);
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
    public Task<MariaDbEmbeddedDataReader> ExecuteReaderAsyncTyped(CommandBehavior behavior, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<MariaDbEmbeddedDataReader>(cancellationToken);
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
