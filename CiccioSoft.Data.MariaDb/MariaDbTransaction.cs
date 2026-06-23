// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Data;
using System.Data.Common;

namespace CiccioSoft.Data.MariaDb;

/// <summary>
/// Represents a transaction to be performed at a MariaDB Embedded database.
/// </summary>
public sealed class MariaDbTransaction : DbTransaction
{
    private readonly MariaDbConnection _connection;
    private bool _disposed;

    internal MariaDbTransaction(MariaDbConnection connection, IsolationLevel isolationLevel)
    {
        _connection = connection;
        IsolationLevel = isolationLevel;
    }

    /// <summary>
    /// Gets the connection associated with the transaction.
    /// </summary>
    public new MariaDbConnection Connection => _connection;

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
            throw new ObjectDisposedException(nameof(MariaDbTransaction));
        }
    }
}
