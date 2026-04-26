using System;
using System.Runtime.InteropServices;

namespace MariaDbClient.Native;

// ============================================================
//  Tutti i campi sono blittabili: IntPtr per puntatori C,
//  uint/ulong per interi C, byte per my_bool/char.
//  Nessun bool, nessuna string, nessun MarshalAs.
// ============================================================

/// <summary>
/// Tipi di campo MySQL/MariaDB (enum_field_types).
/// Blittabile come uint.
/// </summary>
public enum MysqlFieldType : uint
{
    Decimal    = 0,
    Tiny       = 1,
    Short      = 2,
    Long       = 3,
    Float      = 4,
    Double     = 5,
    Null       = 6,
    Timestamp  = 7,
    LongLong   = 8,
    Int24      = 9,
    Date       = 10,
    Time       = 11,
    Datetime   = 12,
    Year       = 13,
    NewDate    = 14,
    Varchar    = 15,
    Bit        = 16,
    NewDecimal = 246,
    Enum       = 247,
    Set        = 248,
    TinyBlob   = 249,
    MediumBlob = 250,
    LongBlob   = 251,
    Blob       = 252,
    VarString  = 253,
    String     = 254,
    Geometry   = 255
}

/// <summary>Flag client per mysql_real_connect.</summary>
[Flags]
public enum ClientFlags : ulong
{
    None             = 0UL,
    LongPassword     = 1UL,
    FoundRows        = 2UL,
    LongFlag         = 4UL,
    ConnectWithDb    = 8UL,
    NoSchema         = 16UL,
    Compress         = 32UL,
    LocalFiles       = 128UL,
    IgnoreSpace      = 256UL,
    Protocol41       = 512UL,
    Interactive      = 1024UL,
    Ssl              = 2048UL,
    Transactions     = 8192UL,
    MultiStatements  = 1UL << 16,
    MultiResults     = 1UL << 17,
    PsMultiResults   = 1UL << 18,
    PluginAuth       = 1UL << 19,
    SessionTrack     = 1UL << 23,
}

/// <summary>Opzioni configurabili via mysql_options.</summary>
public enum MysqlOption : int
{
    OptConnectTimeout    = 0,
    OptCompress          = 1,
    OptNamedPipe         = 2,
    InitCommand          = 3,
    ReadDefaultFile      = 4,
    ReadDefaultGroup     = 5,
    SetCharsetDir        = 6,
    SetCharsetName       = 7,
    OptLocalInfile       = 8,
    OptProtocol          = 9,
    OptReadTimeout       = 11,
    OptWriteTimeout      = 12,
    OptReconnect         = 15,
    PluginDir            = 16,
    DefaultAuth          = 17,
    OptBindAddress       = 18,
    OptSslKey            = 19,
    OptSslCert           = 20,
    OptSslCa             = 21,
    OptSslCapath         = 22,
    OptSslCipher         = 23,
    OptSslVerifyServerCert = 24,
    TlsVersion           = 32,
}

// ------------------------------------------------------------
//  MYSQL_FIELD  –  metadati di una colonna nel result set
// ------------------------------------------------------------

/// <summary>
/// Replica blittabile di MYSQL_FIELD.
/// I campi char* sono IntPtr; usare <see cref="MysqlField"/>
/// per la versione managed con string.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct MysqlFieldNative
{
    public byte*          Name;
    public byte*          OrgName;
    public byte*          Table;
    public byte*          OrgTable;
    public byte*          Db;
    public byte*          Catalog;
    public byte*          Def;
    /* versione originale ma buggata perche il tipo in c è 
       unsigned long */
    // public nuint          Length;
    // public nuint          MaxLength;
    public uint           Length;
    public uint           MaxLength;
    public uint           NameLength;
    public uint           OrgNameLength;
    public uint           TableLength;
    public uint           OrgTableLength;
    public uint           DbLength;
    public uint           CatalogLength;
    public uint           DefLength;
    public uint           Flags;
    public uint           Decimals;
    public uint           CharSetNr;
    public MysqlFieldType Type;
    public void*          Extension;
}

// ------------------------------------------------------------
//  MYSQL_TIME  –  usata in MYSQL_BIND per date/time
// ------------------------------------------------------------

/// <summary>
/// Replica blittabile di MYSQL_TIME.
/// <c>Neg</c> è byte (my_bool), <c>TimeType</c> è int.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct MysqlTimeNative
{
    public uint  Year;
    public uint  Month;
    public uint  Day;
    public uint  Hour;
    public uint  Minute;
    public uint  Second;
    public ulong SecondPart;   // microsecondi
    public byte  Neg;          // my_bool: 0 = false, 1 = true
    public int   TimeType;     // enum_mysql_timestamp_type
}

// ------------------------------------------------------------
//  MYSQL_BIND  –  parametri / risultati prepared statement
// ------------------------------------------------------------

/// <summary>
/// Replica blittabile di MYSQL_BIND.
/// Tutti i puntatori sono void* / byte* per garantire blittabilità.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct MysqlBindNative
{
    public uint*         Length;          // unsigned long*
    public byte*          IsNull;          // my_bool*
    public void*          Buffer;          // void*
    public byte*          Error;           // my_bool*
    public byte*          RowPtr;          // interno
    public void*          StoreParam;      // interno
    public void*          FetchResult;     // interno
    public void*          SkipResult;      // interno
    public uint          BufferLength;
    public uint          Offset;
    public uint          LengthValue;
    public uint           ParamNumber;
    public uint           PackLength;
    public MysqlFieldType BufferType;
    public byte           ErrorValue;      // my_bool
    public byte           IsUnsigned;      // my_bool
    public byte           LongDataUsed;    // my_bool
    public byte           IsNullValue;     // my_bool
    public void*          Extension;
}
