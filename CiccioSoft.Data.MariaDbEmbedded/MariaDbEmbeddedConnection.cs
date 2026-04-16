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
    public override string DataSource => string.Empty;

    /// <inheritdoc />
    public override ConnectionState State => _state;

    /// <inheritdoc />
    public override string ServerVersion => string.Empty;

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
    protected override DbCommand CreateDbCommand()
    {
        return new MariaDbEmbeddedCommand
        {
            Connection = this
        };
    }

    /// <inheritdoc />
    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        return new MariaDbEmbeddedTransaction(this, isolationLevel);
    }

    /// <inheritdoc />
    public override void ChangeDatabase(string databaseName)
    {
        if (_state != ConnectionState.Open)
        {
            throw new InvalidOperationException("The connection must be open to change the database.");
        }
    }
}
