// Copyright (c) 2026 Francesco Crimi
// MIT License

using System;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

/// <summary>
/// Metadati di una colonna del result set.
/// Wrapper managed di <c>MYSQL_FIELD</c>.
/// </summary>
public sealed class MySqlField
{
    public string Name { get; }
    public string OrgName { get; }
    public string Table { get; }
    public string OrgTable { get; }
    public string Database { get; }
    public string Catalog { get; }
    public string? Default { get; }   // null se non richiesto con mysql_list_fields

    public uint Length { get; }   // larghezza dichiarata della colonna
    public uint MaxLength { get; }   // larghezza massima nei dati effettivi
    public uint Flags { get; }
    public uint Decimals { get; }
    public uint CharsetNumber { get; }
    public MySqlFieldTypes Type { get; }

    // flag di comodo
    public bool IsNotNull => (Flags & NativeMariadbCom.NOT_NULL_FLAG) != 0;
    public bool IsPrimaryKey => (Flags & NativeMariadbCom.PRI_KEY_FLAG) != 0;
    public bool IsUniqueKey => (Flags & NativeMariadbCom.UNIQUE_KEY_FLAG) != 0;
    public bool IsBlob => (Flags & NativeMariadbCom.BLOB_FLAG) != 0;
    public bool IsUnsigned => (Flags & NativeMariadbCom.UNSIGNED_FLAG) != 0;
    public bool IsAutoIncrement => (Flags & NativeMariadbCom.AUTO_INCREMENT_FLAG) != 0;
    public bool IsNumeric => (Flags & NativeMariadbCom.NUM_FLAG) != 0;

    private MySqlField(
        string name, string orgName,
        string table, string orgTable,
        string db, string catalog, string? def,
        uint length, uint maxLength,
        uint flags, uint decimals, uint charsetNr,
        MySqlFieldTypes type)
    {
        Name = name;
        OrgName = orgName;
        Table = table;
        OrgTable = orgTable;
        Database = db;
        Catalog = catalog;
        Default = def;
        Length = length;
        MaxLength = maxLength;
        Flags = flags;
        Decimals = decimals;
        CharsetNumber = charsetNr;
        Type = type;
    }

    /// <summary>
    /// Costruisce un'istanza managed a partire dal puntatore nativo
    /// restituito da <c>mysql_fetch_field_direct</c>.
    /// </summary>
    internal static unsafe MySqlField FromPointer(nint fieldPtr)
    {
        if (fieldPtr == 0)
            throw new ArgumentNullException(nameof(fieldPtr));

        // cast diretto — nessun offset manuale, nessuna copia
        st_mysql_field* f = (st_mysql_field*)fieldPtr.ToPointer();

        return new MySqlField(
            name: Utils.GetStringFromPointerBytes(f->name),
            orgName: Utils.GetStringFromPointerBytes(f->org_name),
            table: Utils.GetStringFromPointerBytes(f->table),
            orgTable: Utils.GetStringFromPointerBytes(f->org_table),
            db: Utils.GetStringFromPointerBytes(f->db),
            catalog: Utils.GetStringFromPointerBytes(f->catalog),
            def: f->def != null
                           ? Utils.GetStringFromPointerBytes(f->def)
                           : null,
            length: f->length,
            maxLength: f->max_length,
            flags: f->flags,
            decimals: f->decimals,
            charsetNr: f->charsetnr,
            type: f->type);
    }

    public override string ToString() =>
        $"{Table}.{Name} ({Type}{(IsUnsigned ? " UNSIGNED" : "")}" +
        $"{(IsNotNull ? " NOT NULL" : "")})";
}