using System.Data.Common;

namespace CiccioSoft.Data.MariaDb;

/// <summary>
/// Represents a set of methods for creating instances of the MariaDB Embedded provider's implementation of the data source classes.
/// </summary>
public sealed class MariaDbProviderFactory : DbProviderFactory
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="MariaDbProviderFactory"/> class.
    /// </summary>
    public static readonly MariaDbProviderFactory Instance = new();

    private MariaDbProviderFactory()
    {
    }

    /// <inheritdoc />
    public override DbCommand CreateCommand()
    {
        return new MariaDbCommand();
    }

    /// <inheritdoc />
    public override DbCommandBuilder CreateCommandBuilder()
    {
        throw new NotSupportedException("DbCommandBuilder is not supported by the MariaDB Embedded provider.");
    }

    /// <inheritdoc />
    public override DbConnection CreateConnection()
    {
        return new MariaDbConnection();
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
        return new MariaDbParameter();
    }
}
