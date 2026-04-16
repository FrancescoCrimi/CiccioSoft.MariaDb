using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace CiccioSoft.Data.MariaDbEmbedded;

/// <summary>
/// Represents a collection of parameters for a MariaDbEmbeddedCommand.
/// </summary>
public sealed class MariaDbEmbeddedParameterCollection : DbParameterCollection
{
    private readonly List<DbParameter> _parameters = new();

    /// <inheritdoc />
    public override int Count => _parameters.Count;

    /// <inheritdoc />
    public override bool IsSynchronized => false;

    /// <inheritdoc />
    public override bool IsReadOnly => false;

    /// <inheritdoc />
    public override bool IsFixedSize => false;

    /// <inheritdoc />
    public override object SyncRoot => this;

    /// <summary>
    /// Gets or sets the parameter at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the parameter.</param>
    /// <returns>The parameter at the specified index.</returns>
    public new MariaDbEmbeddedParameter this[int index]
    {
        get => (MariaDbEmbeddedParameter)_parameters[index];
        set => _parameters[index] = value;
    }

    /// <summary>
    /// Gets or sets the parameter with the specified name.
    /// </summary>
    /// <param name="parameterName">The name of the parameter.</param>
    public new MariaDbEmbeddedParameter this[string parameterName]
    {
        get => (MariaDbEmbeddedParameter)GetParameter(parameterName);
        set => SetParameter(parameterName, value);
    }

    /// <inheritdoc />
    protected override DbParameter GetParameter(int index)
    {
        return _parameters[index];
    }

    /// <inheritdoc />
    protected override DbParameter GetParameter(string parameterName)
    {
        int index = IndexOf(parameterName);
        if (index < 0)
        {
            throw new IndexOutOfRangeException($"Parameter '{parameterName}' not found.");
        }

        return _parameters[index];
    }

    /// <inheritdoc />
    protected override void SetParameter(int index, DbParameter value)
    {
        _parameters[index] = value;
    }

    /// <inheritdoc />
    protected override void SetParameter(string parameterName, DbParameter value)
    {
        int index = IndexOf(parameterName);
        if (index < 0)
        {
            throw new IndexOutOfRangeException($"Parameter '{parameterName}' not found.");
        }

        _parameters[index] = value;
    }

    /// <inheritdoc />
    public override int Add(object value)
    {
        if (value is not DbParameter parameter)
        {
            throw new ArgumentException("Value must be a DbParameter.", nameof(value));
        }

        _parameters.Add(parameter);
        return _parameters.Count - 1;
    }

    /// <summary>
    /// Adds a parameter to the collection.
    /// </summary>
    /// <param name="value">The parameter to add.</param>
    /// <returns>The index of the new parameter.</returns>
    public MariaDbEmbeddedParameter Add(MariaDbEmbeddedParameter value)
    {
        _parameters.Add(value);
        return value;
    }

    /// <summary>
    /// Adds a parameter with the specified name and value.
    /// </summary>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <returns>The new parameter.</returns>
    public MariaDbEmbeddedParameter AddWithValue(string parameterName, object? value)
    {
        var parameter = new MariaDbEmbeddedParameter(parameterName, value);
        _parameters.Add(parameter);
        return parameter;
    }

    /// <summary>
    /// Adds a parameter with the specified name.
    /// </summary>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <returns>The new parameter.</returns>
    public MariaDbEmbeddedParameter Add(string parameterName)
    {
        var parameter = new MariaDbEmbeddedParameter
        {
            ParameterName = parameterName
        };
        _parameters.Add(parameter);
        return parameter;
    }

    /// <inheritdoc />
    public override void Clear()
    {
        _parameters.Clear();
    }

    /// <inheritdoc />
    public override bool Contains(object value)
    {
        return value is DbParameter parameter && _parameters.Contains(parameter);
    }

    /// <inheritdoc />
    public override bool Contains(string value)
    {
        return IndexOf(value) >= 0;
    }

    /// <inheritdoc />
    public override void CopyTo(Array array, int index)
    {
        ((ICollection)_parameters).CopyTo(array, index);
    }

    /// <inheritdoc />
    public override IEnumerator GetEnumerator()
    {
        return _parameters.GetEnumerator();
    }

    /// <inheritdoc />
    public override int IndexOf(object value)
    {
        return value is DbParameter parameter ? _parameters.IndexOf(parameter) : -1;
    }

    /// <inheritdoc />
    public override int IndexOf(string parameterName)
    {
        for (int i = 0; i < _parameters.Count; i++)
        {
            if (string.Equals(_parameters[i].ParameterName, parameterName, StringComparison.Ordinal))
            {
                return i;
            }
        }

        return -1;
    }

    /// <inheritdoc />
    public override void Insert(int index, object value)
    {
        if (value is not DbParameter parameter)
        {
            throw new ArgumentException("Value must be a DbParameter.", nameof(value));
        }

        _parameters.Insert(index, parameter);
    }

    /// <inheritdoc />
    public override void Remove(object value)
    {
        if (value is DbParameter parameter)
        {
            _parameters.Remove(parameter);
        }
    }

    /// <inheritdoc />
    public override void RemoveAt(int index)
    {
        _parameters.RemoveAt(index);
    }

    /// <inheritdoc />
    public override void RemoveAt(string parameterName)
    {
        int index = IndexOf(parameterName);
        if (index >= 0)
        {
            _parameters.RemoveAt(index);
        }
    }

    /// <inheritdoc />
    public override void AddRange(Array values)
    {
        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        foreach (object item in values)
        {
            Add(item);
        }
    }
}
