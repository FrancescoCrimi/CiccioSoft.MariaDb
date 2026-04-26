using MariaDbClient.Native;

namespace MariaDbClient;

/// <summary>
/// Rappresentazione managed dei metadati di una colonna.
/// Costruita leggendo <see cref="MysqlFieldNative"/> dalla memoria nativa.
/// </summary>
public sealed class MysqlField
{
    /// <summary>Nome della colonna.</summary>
    public string Name { get; }

    /// <summary>Nome originale della colonna (prima di un alias).</summary>
    public string OrgName { get; }

    /// <summary>Nome della tabella (o alias).</summary>
    public string Table { get; }

    /// <summary>Nome originale della tabella.</summary>
    public string OrgTable { get; }

    /// <summary>Nome del database.</summary>
    public string Db { get; }

    /// <summary>Lunghezza massima dichiarata della colonna.</summary>
    public nuint Length { get; }

    /// <summary>Lunghezza massima effettiva nel result set corrente.</summary>
    public nuint MaxLength { get; }

    /// <summary>Flag della colonna (NOT NULL, PRI KEY, ecc.).</summary>
    public uint Flags { get; }

    /// <summary>Numero di decimali (per colonne numeriche).</summary>
    public uint Decimals { get; }

    /// <summary>Numero del charset.</summary>
    public uint CharSetNr { get; }

    /// <summary>Tipo nativo MariaDB della colonna.</summary>
    public MysqlFieldType Type { get; }

    // Flag derivati dai bit di Flags
    public bool IsNotNull        => (Flags & 1U)    != 0;
    public bool IsPrimaryKey     => (Flags & 2U)    != 0;
    public bool IsUniqueKey      => (Flags & 4U)    != 0;
    public bool IsMultipleKey    => (Flags & 8U)    != 0;
    public bool IsBlob           => (Flags & 16U)   != 0;
    public bool IsUnsigned       => (Flags & 32U)   != 0;
    public bool IsZeroFill       => (Flags & 64U)   != 0;
    public bool IsBinary         => (Flags & 128U)  != 0;
    public bool IsAutoIncrement  => (Flags & 512U)  != 0;
    public bool IsNumeric        => (Flags & 32768U) != 0;

    internal unsafe MysqlField(MysqlFieldNative* f)
    {
        Name      = MariaDbImports.PtrToStringUtf8(f->Name)      ?? string.Empty;
        OrgName   = MariaDbImports.PtrToStringUtf8(f->OrgName)   ?? string.Empty;
        Table     = MariaDbImports.PtrToStringUtf8(f->Table)     ?? string.Empty;
        OrgTable  = MariaDbImports.PtrToStringUtf8(f->OrgTable)  ?? string.Empty;
        Db        = MariaDbImports.PtrToStringUtf8(f->Db)        ?? string.Empty;
        Length    = f->Length;
        MaxLength = f->MaxLength;
        Flags     = f->Flags;
        Decimals  = f->Decimals;
        CharSetNr = f->CharSetNr;
        Type      = f->Type;
    }

    public override string ToString() => $"{Table}.{Name} ({Type})";
}
