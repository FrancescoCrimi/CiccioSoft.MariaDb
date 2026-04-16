using System.Data;
using System.Data.Common;

namespace CiccioSoft.Data.MariaDbEmbedded;

/// <summary>
/// Represents a parameter to a MariaDbEmbeddedCommand.
/// </summary>
public sealed class MariaDbEmbeddedParameter : DbParameter
{
    private DbType _dbType;
    private ParameterDirection _direction = ParameterDirection.Input;
    private bool _isNullable;
    private string _parameterName = string.Empty;
    private int _size;
    private string _sourceColumn = string.Empty;
    private bool _sourceColumnNullMapping;
    private DataRowVersion _sourceVersion = DataRowVersion.Current;
    private object? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="MariaDbEmbeddedParameter"/> class.
    /// </summary>
    public MariaDbEmbeddedParameter()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MariaDbEmbeddedParameter"/> class with the specified name and value.
    /// </summary>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    public MariaDbEmbeddedParameter(string parameterName, object? value)
    {
        ParameterName = parameterName;
        Value = value;
    }

    /// <inheritdoc />
    public override DbType DbType
    {
        get => _dbType;
        set => _dbType = value;
    }

    /// <inheritdoc />
    public override ParameterDirection Direction
    {
        get => _direction;
        set => _direction = value;
    }

    /// <inheritdoc />
    public override bool IsNullable
    {
        get => _isNullable;
        set => _isNullable = value;
    }

    /// <inheritdoc />
#nullable disable
    public override string ParameterName
    {
        get => _parameterName;
        set => _parameterName = value ?? string.Empty;
    }
#nullable restore

    /// <inheritdoc />
    public override int Size
    {
        get => _size;
        set => _size = value;
    }

    /// <inheritdoc />
#nullable disable
    public override string SourceColumn
    {
        get => _sourceColumn;
        set => _sourceColumn = value ?? string.Empty;
    }
#nullable restore

    /// <inheritdoc />
    public override bool SourceColumnNullMapping
    {
        get => _sourceColumnNullMapping;
        set => _sourceColumnNullMapping = value;
    }

    /// <inheritdoc />
    public override DataRowVersion SourceVersion
    {
        get => _sourceVersion;
        set => _sourceVersion = value;
    }

    /// <inheritdoc />
    public override object? Value
    {
        get => _value;
        set => _value = value;
    }

    /// <inheritdoc />
    public override void ResetDbType()
    {
        _dbType = DbType.String;
    }
}
