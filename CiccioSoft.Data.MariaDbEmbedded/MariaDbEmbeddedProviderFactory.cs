using System.Data.Common;

namespace CiccioSoft.Data.MariaDbEmbedded;

/// <summary>
/// Represents a set of methods for creating instances of the MariaDB Embedded provider's implementation of the data source classes.
/// </summary>
public sealed class MariaDbEmbeddedProviderFactory : DbProviderFactory
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="MariaDbEmbeddedProviderFactory"/> class.
    /// </summary>
    public static readonly MariaDbEmbeddedProviderFactory Instance = new();

    private MariaDbEmbeddedProviderFactory()
    {
    }

    /// <inheritdoc />
    public override DbCommand CreateCommand()
    {
        return new MariaDbEmbeddedCommand();
    }

    /// <inheritdoc />
    public override DbCommandBuilder CreateCommandBuilder()
    {
        throw new NotSupportedException("DbCommandBuilder is not supported by the MariaDB Embedded provider.");
    }

    /// <inheritdoc />
    public override DbConnection CreateConnection()
    {
        return new MariaDbEmbeddedConnection();
    }

    /// <inheritdoc />
    public override DbConnectionStringBuilder CreateConnectionStringBuilder()
    {
        return new DbConnectionStringBuilder();
    }

    /// <inheritdoc />
    public override DbDataAdapter CreateDataAdapter()
    {
        throw new NotSupportedException("DbDataAdapter is not supported by the MariaDB Embedded provider.");
    }

    /// <inheritdoc />
    public override DbParameter CreateParameter()
    {
        return new MariaDbEmbeddedParameter();
    }
}
