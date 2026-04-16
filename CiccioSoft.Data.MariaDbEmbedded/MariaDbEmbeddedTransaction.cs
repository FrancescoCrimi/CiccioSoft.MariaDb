using System.Data;
using System.Data.Common;

namespace CiccioSoft.Data.MariaDbEmbedded;

/// <summary>
/// Represents a transaction to be performed at a MariaDB Embedded database.
/// </summary>
public sealed class MariaDbEmbeddedTransaction : DbTransaction
{
    private readonly MariaDbEmbeddedConnection _connection;
    private bool _disposed;

    internal MariaDbEmbeddedTransaction(MariaDbEmbeddedConnection connection, IsolationLevel isolationLevel)
    {
        _connection = connection;
        IsolationLevel = isolationLevel;
    }

    /// <summary>
    /// Gets the connection associated with the transaction.
    /// </summary>
    public new MariaDbEmbeddedConnection Connection => _connection;

    /// <inheritdoc />
    protected override DbConnection DbConnection => _connection;

    /// <inheritdoc />
    public override IsolationLevel IsolationLevel { get; }

    /// <inheritdoc />
    public override void Commit()
    {
        ThrowIfDisposed();
    }

    /// <inheritdoc />
    public override void Rollback()
    {
        ThrowIfDisposed();
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        _disposed = true;
        base.Dispose(disposing);
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(MariaDbEmbeddedTransaction));
        }
    }
}
