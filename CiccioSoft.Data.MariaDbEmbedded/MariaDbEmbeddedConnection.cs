using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace CiccioSoft.Data.MariaDbEmbedded;

/// <summary>
/// Represents a connection to a MariaDB Embedded database.
/// </summary>
public sealed class MariaDbEmbeddedConnection : DbConnection
{
    private string _connectionString = string.Empty;
    private ConnectionState _state = ConnectionState.Closed;

    /// <summary>
    /// Initializes a new instance of the <see cref="MariaDbEmbeddedConnection"/> class.
    /// </summary>
    public MariaDbEmbeddedConnection()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MariaDbEmbeddedConnection"/> class with the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    public MariaDbEmbeddedConnection(string connectionString)
    {
        ConnectionString = connectionString;
    }



    /// <summary>
    /// Begins a database transaction.
    /// </summary>
    /// <returns>A <see cref="MariaDbEmbeddedTransaction"/> representing the new database transaction.</returns>
    /// <remarks>Transactions may not be nested.</remarks>
    public new MariaDbEmbeddedTransaction BeginTransaction()
        => BeginTransaction(IsolationLevel.Unspecified);

    /// <summary>
    /// Begins a database transaction.
    /// </summary>
    /// <param name="isolationLevel">The <see cref="IsolationLevel"/> for the transaction.</param>
    /// <returns>A <see cref="MariaDbEmbeddedTransaction"/> representing the new database transaction.</returns>
    /// <remarks>Transactions may not be nested.</remarks>
    public new MariaDbEmbeddedTransaction BeginTransaction(IsolationLevel isolationLevel)
        => new MariaDbEmbeddedTransaction(this, isolationLevel);    

    /// <summary>
    /// Begins a database transaction.
    /// </summary>
    /// <param name="isolationLevel">The <see cref="IsolationLevel"/> for the transaction.</param>
    /// <param name="isReadOnly">If <c>true</c>, changes to tables used in the transaction are prohibited; otherwise, they are permitted.</param>
    /// <returns>A <see cref="MariaDbEmbeddedTransaction"/> representing the new database transaction.</returns>
    /// <remarks>Transactions may not be nested.</remarks>
    public MariaDbEmbeddedTransaction BeginTransaction(IsolationLevel isolationLevel, bool isReadOnly)
        => throw new NotImplementedException();

    /// <inheritdoc />
    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        => BeginTransaction(isolationLevel);

    /// <summary>
    /// Begins a database transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task{MariaDbEmbeddedTransaction}"/> representing the new database transaction.</returns>
    /// <remarks>Transactions may not be nested.</remarks>
    public new ValueTask<MariaDbEmbeddedTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => BeginTransactionAsync(IsolationLevel.Unspecified, default, cancellationToken);

    /// <summary>
    /// Begins a database transaction asynchronously.
    /// </summary>
    /// <param name="isolationLevel">The <see cref="IsolationLevel"/> for the transaction.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task{MariaDbEmbeddedTransaction}"/> representing the new database transaction.</returns>
    /// <remarks>Transactions may not be nested.</remarks>
    public new ValueTask<MariaDbEmbeddedTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        => BeginTransactionAsync(isolationLevel, default, cancellationToken);

    /// <inheritdoc />
    protected override async ValueTask<DbTransaction> BeginDbTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
        => await BeginTransactionAsync(isolationLevel, default, cancellationToken);

    /// <summary>
    /// Begins a database transaction asynchronously.
    /// </summary>
    /// <param name="isolationLevel">The <see cref="IsolationLevel"/> for the transaction.</param>
    /// <param name="isReadOnly">If <c>true</c>, changes to tables used in the transaction are prohibited; otherwise, they are permitted.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task{MariaDbEmbeddedTransaction}"/> representing the new database transaction.</returns>
    /// <remarks>Transactions may not be nested.</remarks>
    public ValueTask<MariaDbEmbeddedTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, bool isReadOnly, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();




    /// <inheritdoc />
    public override void Close()
    {
        if (_state == ConnectionState.Closed)
        {
            return;
        }

        _state = ConnectionState.Closed;
    }

    /// <inheritdoc />
    public override Task CloseAsync()
    {
        Close();
        return Task.CompletedTask;
    }


    /// <inheritdoc />
    public override void ChangeDatabase(string databaseName)
    {
        if (_state != ConnectionState.Open)
        {
            throw new InvalidOperationException("The connection must be open to change the database.");
        }
    }

    /// <inheritdoc />
    public override Task ChangeDatabaseAsync(string databaseName, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();


    public new MariaDbEmbeddedCommand CreateCommand() => (MariaDbEmbeddedCommand) base.CreateCommand();

    /// <inheritdoc />
    protected override DbCommand CreateDbCommand()
    {
        return new MariaDbEmbeddedCommand
        {
            Connection = this
        };
    }


    public bool Ping() => PingAsync(CancellationToken.None).GetAwaiter().GetResult();

    public Task<bool> PingAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();



    /// <inheritdoc />
    public override void Open()
    {
        if (_state != ConnectionState.Closed)
        {
            return;
        }

        _state = ConnectionState.Open;
    }

    /// <inheritdoc />
    public override Task OpenAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled(cancellationToken);
        }

        Open();
        return Task.CompletedTask;
    }


    /// <inheritdoc />
#nullable disable
    public override string ConnectionString
    {
        get => _connectionString;
        set => _connectionString = value ?? string.Empty;
    }
#nullable restore

    /// <inheritdoc />
    public override string Database => string.Empty;

    /// <inheritdoc />
    public override ConnectionState State => _state;

    /// <inheritdoc />
    public override string DataSource => string.Empty;

    /// <inheritdoc />
    public override string ServerVersion => string.Empty;



    /// <inheritdoc />
    protected override DbProviderFactory DbProviderFactory
        => MariaDbEmbeddedConnectorFactory.Instance;



    /// <inheritdoc />
    public override DataTable GetSchema()
        => throw new NotImplementedException();

    /// <inheritdoc />
    public override DataTable GetSchema(string collectionName)
        => throw new NotImplementedException();

    /// <inheritdoc />
    public override DataTable GetSchema(string collectionName, string?[] restrictionValues)
        => throw new NotImplementedException();

    /// <inheritdoc />
    public override Task<DataTable> GetSchemaAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <inheritdoc />
    public override Task<DataTable> GetSchemaAsync(string collectionName, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <inheritdoc />
    public override Task<DataTable> GetSchemaAsync(string collectionName, string?[] restrictionValues, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();



    /// <inheritdoc />
    public override int ConnectionTimeout
        => throw new NotImplementedException();


    // public new MySqlBatch CreateBatch() => new(this);
    // protected override DbBatch CreateDbBatch() => CreateBatch();
    /// <inheritdoc />
    public override bool CanCreateBatch => false;

}
