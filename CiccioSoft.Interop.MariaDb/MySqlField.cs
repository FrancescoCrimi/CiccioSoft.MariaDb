// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using CiccioSoft.Interop.MariaDb.Native;

namespace CiccioSoft.Interop.MariaDb;

/// <summary>
/// Metadata for a result-set column.
/// Managed wrapper for <c>MYSQL_FIELD</c>.
/// </summary>
public sealed class MySqlField
{
    public string Name { get; }
    public string OrgName { get; }
    public string Table { get; }
    public string OrgTable { get; }
    public string Database { get; }
    public string Catalog { get; }
    public string? Default { get; }   // null when not requested via mysql_list_fields

    public uint Length { get; }   // declared column width
    public uint MaxLength { get; }   // maximum width in the actual data
    public uint Flags { get; }
    public uint Decimals { get; }
    public uint CharsetNumber { get; }
    public MySqlFieldTypes Type { get; }

    // convenience flags
    public bool IsNotNull => (Flags & MariadbComNative.NOT_NULL_FLAG) != 0;
    public bool IsPrimaryKey => (Flags & MariadbComNative.PRI_KEY_FLAG) != 0;
    public bool IsUniqueKey => (Flags & MariadbComNative.UNIQUE_KEY_FLAG) != 0;
    public bool IsBlob => (Flags & MariadbComNative.BLOB_FLAG) != 0;
    public bool IsUnsigned => (Flags & MariadbComNative.UNSIGNED_FLAG) != 0;
    public bool IsAutoIncrement => (Flags & MariadbComNative.AUTO_INCREMENT_FLAG) != 0;
    public bool IsNumeric => (Flags & MariadbComNative.NUM_FLAG) != 0;

    internal unsafe MySqlField(MySqlFieldNative native)
    {
        Name = Utils.GetStringFromPointerBytes(native.name);
        OrgName = Utils.GetStringFromPointerBytes(native.org_name);
        Table = Utils.GetStringFromPointerBytes(native.table);
        OrgTable = Utils.GetStringFromPointerBytes(native.org_table);
        Database = Utils.GetStringFromPointerBytes(native.db);
        Catalog = Utils.GetStringFromPointerBytes(native.catalog);
        Default = native.def != null
                       ? Utils.GetStringFromPointerBytes(native.def)
                       : null;
        Length = native.length;
        MaxLength = native.max_length;
        Flags = native.flags;
        Decimals = native.decimals;
        CharsetNumber = native.charsetnr;
        Type = native.type;
    }

    public override string ToString() =>
        $"{Table}.{Name} ({Type}{(IsUnsigned ? " UNSIGNED" : "")}" +
        $"{(IsNotNull ? " NOT NULL" : "")})";
}