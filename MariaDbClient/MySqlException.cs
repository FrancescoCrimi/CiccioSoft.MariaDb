using System;

namespace MariaDbClient.Exceptions;

/// <summary>
/// Eccezione base per tutti gli errori MariaDB/MySQL.
/// Contiene il codice di errore nativo e lo SQLSTATE.
/// </summary>
public class MySqlException : Exception
{
    /// <summary>Codice di errore nativo (mysql_errno).</summary>
    public uint ErrorCode { get; }

    /// <summary>Codice SQLSTATE a 5 caratteri (mysql_sqlstate).</summary>
    public string SqlState { get; }

    public MySqlException(string message, uint errorCode = 0, string sqlState = "HY000")
        : base(message)
    {
        ErrorCode = errorCode;
        SqlState  = sqlState;
    }

    public MySqlException(string message, Exception inner, uint errorCode = 0, string sqlState = "HY000")
        : base(message, inner)
    {
        ErrorCode = errorCode;
        SqlState  = sqlState;
    }

    public override string ToString()
        => $"MySqlException [{ErrorCode}/{SqlState}]: {Message}";
}

/// <summary>Errore di connessione al server.</summary>
public sealed class MySqlConnectionException : MySqlException
{
    public MySqlConnectionException(string message, uint errorCode = 0, string sqlState = "08001")
        : base(message, errorCode, sqlState) { }
}

/// <summary>Errore durante l'esecuzione di una query SQL.</summary>
public sealed class MySqlQueryException : MySqlException
{
    /// <summary>Query che ha causato l'errore (se disponibile).</summary>
    public string? Query { get; }

    public MySqlQueryException(string message, string? query = null, uint errorCode = 0, string sqlState = "HY000")
        : base(message, errorCode, sqlState)
    {
        Query = query;
    }
}

/// <summary>Errore in un prepared statement.</summary>
public sealed class MySqlStmtException : MySqlException
{
    public MySqlStmtException(string message, uint errorCode = 0, string sqlState = "HY000")
        : base(message, errorCode, sqlState) { }
}
