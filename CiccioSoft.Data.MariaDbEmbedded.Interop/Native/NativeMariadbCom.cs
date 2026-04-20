using System;
using System.Runtime.InteropServices;
using static CiccioSoft.Data.MariaDbEmbedded.Interop.Native.enum_field_types;
using static CiccioSoft.Data.MariaDbEmbedded.Interop.Native.enum_session_state_type;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal static unsafe partial class NativeMariadbCom
    {
        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int ma_net_init([NativeTypeName("NET *")] st_net* net, [NativeTypeName("MARIADB_PVIO *")] st_ma_pvio* pvio);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void ma_net_end([NativeTypeName("NET *")] st_net* net);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void ma_net_clear([NativeTypeName("NET *")] st_net* net);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int ma_net_flush([NativeTypeName("NET *")] st_net* net);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int ma_net_write([NativeTypeName("NET *")] st_net* net, [NativeTypeName("const unsigned char *")] byte* packet, [NativeTypeName("size_t")] nuint len);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int ma_net_write_buff([NativeTypeName("NET *")] st_net* net, [NativeTypeName("const char *")] byte* packet, [NativeTypeName("size_t")] nuint len);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int ma_net_write_command([NativeTypeName("NET *")] st_net* net, [NativeTypeName("unsigned char")] byte command, [NativeTypeName("const char *")] byte* packet, [NativeTypeName("size_t")] nuint len, [NativeTypeName("my_bool")] sbyte disable_flush);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int ma_net_real_write([NativeTypeName("NET *")] st_net* net, [NativeTypeName("const char *")] byte* packet, [NativeTypeName("size_t")] nuint len);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long")]
        public static extern uint ma_net_read([NativeTypeName("NET *")] st_net* net);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern byte* ma_scramble_323([NativeTypeName("char *")] byte* to, [NativeTypeName("const char *")] byte* message, [NativeTypeName("const char *")] byte* password);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void ma_scramble_41([NativeTypeName("const unsigned char *")] byte* buffer, [NativeTypeName("const char *")] byte* scramble, [NativeTypeName("const char *")] byte* password);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void ma_hash_password([NativeTypeName("unsigned long *")] uint* result, [NativeTypeName("const char *")] byte* password, [NativeTypeName("size_t")] nuint len);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void ma_make_scrambled_password([NativeTypeName("char *")] byte* to, [NativeTypeName("const char *")] byte* password);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void mariadb_load_defaults([NativeTypeName("const char *")] byte* conf_file, [NativeTypeName("const char **")] byte** groups, int* argc, [NativeTypeName("char ***")] byte*** argv);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte ma_thread_init();

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void ma_thread_end();

        [NativeTypeName("#define NAME_CHAR_LEN 64")]
        public const int NAME_CHAR_LEN = 64;

        [NativeTypeName("#define NAME_LEN 256")]
        public const int NAME_LEN = 256;

        [NativeTypeName("#define HOSTNAME_LENGTH 255")]
        public const int HOSTNAME_LENGTH = 255;

        [NativeTypeName("#define SYSTEM_MB_MAX_CHAR_LENGTH 4")]
        public const int SYSTEM_MB_MAX_CHAR_LENGTH = 4;

        [NativeTypeName("#define USERNAME_CHAR_LENGTH 128")]
        public const int USERNAME_CHAR_LENGTH = 128;

        [NativeTypeName("#define USERNAME_LENGTH (USERNAME_CHAR_LENGTH * SYSTEM_MB_MAX_CHAR_LENGTH)")]
        public const int USERNAME_LENGTH = (128 * 4);

        [NativeTypeName("#define SERVER_VERSION_LENGTH 60")]
        public const int SERVER_VERSION_LENGTH = 60;

        [NativeTypeName("#define SQLSTATE_LENGTH 5")]
        public const int SQLSTATE_LENGTH = 5;

        [NativeTypeName("#define SCRAMBLE_LENGTH 20")]
        public const int SCRAMBLE_LENGTH = 20;

        [NativeTypeName("#define SCRAMBLE_LENGTH_323 8")]
        public const int SCRAMBLE_LENGTH_323 = 8;

        [NativeTypeName("#define LOCAL_HOST \"localhost\"")]
        public static ReadOnlySpan<byte> LOCAL_HOST => "localhost"u8;

        [NativeTypeName("#define LOCAL_HOST_NAMEDPIPE \".\"")]
        public static ReadOnlySpan<byte> LOCAL_HOST_NAMEDPIPE => "."u8;

        [NativeTypeName("#define MARIADB_NAMEDPIPE \"MySQL\"")]
        public static ReadOnlySpan<byte> MARIADB_NAMEDPIPE => "MySQL"u8;

        [NativeTypeName("#define MYSQL_SERVICENAME \"MySql\"")]
        public static ReadOnlySpan<byte> MYSQL_SERVICENAME => "MySql"u8;

        [NativeTypeName("#define MYSQL_AUTODETECT_CHARSET_NAME \"auto\"")]
        public static ReadOnlySpan<byte> MYSQL_AUTODETECT_CHARSET_NAME => "auto"u8;

        [NativeTypeName("#define BINCMP_FLAG 131072")]
        public const int BINCMP_FLAG = 131072;

        [NativeTypeName("#define NOT_NULL_FLAG 1")]
        public const int NOT_NULL_FLAG = 1;

        [NativeTypeName("#define PRI_KEY_FLAG 2")]
        public const int PRI_KEY_FLAG = 2;

        [NativeTypeName("#define UNIQUE_KEY_FLAG 4")]
        public const int UNIQUE_KEY_FLAG = 4;

        [NativeTypeName("#define MULTIPLE_KEY_FLAG 8")]
        public const int MULTIPLE_KEY_FLAG = 8;

        [NativeTypeName("#define BLOB_FLAG 16")]
        public const int BLOB_FLAG = 16;

        [NativeTypeName("#define UNSIGNED_FLAG 32")]
        public const int UNSIGNED_FLAG = 32;

        [NativeTypeName("#define ZEROFILL_FLAG 64")]
        public const int ZEROFILL_FLAG = 64;

        [NativeTypeName("#define BINARY_FLAG 128")]
        public const int BINARY_FLAG = 128;

        [NativeTypeName("#define ENUM_FLAG 256")]
        public const int ENUM_FLAG = 256;

        [NativeTypeName("#define AUTO_INCREMENT_FLAG 512")]
        public const int AUTO_INCREMENT_FLAG = 512;

        [NativeTypeName("#define TIMESTAMP_FLAG 1024")]
        public const int TIMESTAMP_FLAG = 1024;

        [NativeTypeName("#define SET_FLAG 2048")]
        public const int SET_FLAG = 2048;

        [NativeTypeName("#define NO_DEFAULT_VALUE_FLAG 4096")]
        public const int NO_DEFAULT_VALUE_FLAG = 4096;

        [NativeTypeName("#define ON_UPDATE_NOW_FLAG 8192")]
        public const int ON_UPDATE_NOW_FLAG = 8192;

        [NativeTypeName("#define NUM_FLAG 32768")]
        public const int NUM_FLAG = 32768;

        [NativeTypeName("#define PART_KEY_FLAG 16384")]
        public const int PART_KEY_FLAG = 16384;

        [NativeTypeName("#define GROUP_FLAG 32768")]
        public const int GROUP_FLAG = 32768;

        [NativeTypeName("#define UNIQUE_FLAG 65536")]
        public const int UNIQUE_FLAG = 65536;

        [NativeTypeName("#define REFRESH_GRANT 1")]
        public const int REFRESH_GRANT = 1;

        [NativeTypeName("#define REFRESH_LOG 2")]
        public const int REFRESH_LOG = 2;

        [NativeTypeName("#define REFRESH_TABLES 4")]
        public const int REFRESH_TABLES = 4;

        [NativeTypeName("#define REFRESH_HOSTS 8")]
        public const int REFRESH_HOSTS = 8;

        [NativeTypeName("#define REFRESH_STATUS 16")]
        public const int REFRESH_STATUS = 16;

        [NativeTypeName("#define REFRESH_THREADS 32")]
        public const int REFRESH_THREADS = 32;

        [NativeTypeName("#define REFRESH_SLAVE 64")]
        public const int REFRESH_SLAVE = 64;

        [NativeTypeName("#define REFRESH_MASTER 128")]
        public const int REFRESH_MASTER = 128;

        [NativeTypeName("#define REFRESH_READ_LOCK 16384")]
        public const int REFRESH_READ_LOCK = 16384;

        [NativeTypeName("#define REFRESH_FAST 32768")]
        public const int REFRESH_FAST = 32768;

        [NativeTypeName("#define CLIENT_MYSQL 1")]
        public const int CLIENT_MYSQL = 1;

        [NativeTypeName("#define CLIENT_FOUND_ROWS 2")]
        public const int CLIENT_FOUND_ROWS = 2;

        [NativeTypeName("#define CLIENT_LONG_FLAG 4")]
        public const int CLIENT_LONG_FLAG = 4;

        [NativeTypeName("#define CLIENT_CONNECT_WITH_DB 8")]
        public const int CLIENT_CONNECT_WITH_DB = 8;

        [NativeTypeName("#define CLIENT_NO_SCHEMA 16")]
        public const int CLIENT_NO_SCHEMA = 16;

        [NativeTypeName("#define CLIENT_COMPRESS 32")]
        public const int CLIENT_COMPRESS = 32;

        [NativeTypeName("#define CLIENT_ODBC 64")]
        public const int CLIENT_ODBC = 64;

        [NativeTypeName("#define CLIENT_LOCAL_FILES 128")]
        public const int CLIENT_LOCAL_FILES = 128;

        [NativeTypeName("#define CLIENT_IGNORE_SPACE 256")]
        public const int CLIENT_IGNORE_SPACE = 256;

        [NativeTypeName("#define CLIENT_INTERACTIVE 1024")]
        public const int CLIENT_INTERACTIVE = 1024;

        [NativeTypeName("#define CLIENT_SSL 2048")]
        public const int CLIENT_SSL = 2048;

        [NativeTypeName("#define CLIENT_IGNORE_SIGPIPE 4096")]
        public const int CLIENT_IGNORE_SIGPIPE = 4096;

        [NativeTypeName("#define CLIENT_TRANSACTIONS 8192")]
        public const int CLIENT_TRANSACTIONS = 8192;

        [NativeTypeName("#define CLIENT_PROTOCOL_41 512")]
        public const int CLIENT_PROTOCOL_41 = 512;

        [NativeTypeName("#define CLIENT_RESERVED 16384")]
        public const int CLIENT_RESERVED = 16384;

        [NativeTypeName("#define CLIENT_SECURE_CONNECTION 32768")]
        public const int CLIENT_SECURE_CONNECTION = 32768;

        [NativeTypeName("#define CLIENT_MULTI_STATEMENTS (1UL << 16)")]
        public const uint CLIENT_MULTI_STATEMENTS = (1U << 16);

        [NativeTypeName("#define CLIENT_MULTI_RESULTS (1UL << 17)")]
        public const uint CLIENT_MULTI_RESULTS = (1U << 17);

        [NativeTypeName("#define CLIENT_PS_MULTI_RESULTS (1UL << 18)")]
        public const uint CLIENT_PS_MULTI_RESULTS = (1U << 18);

        [NativeTypeName("#define CLIENT_PLUGIN_AUTH (1UL << 19)")]
        public const uint CLIENT_PLUGIN_AUTH = (1U << 19);

        [NativeTypeName("#define CLIENT_CONNECT_ATTRS (1UL << 20)")]
        public const uint CLIENT_CONNECT_ATTRS = (1U << 20);

        [NativeTypeName("#define CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA (1UL << 21)")]
        public const uint CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA = (1U << 21);

        [NativeTypeName("#define CLIENT_CAN_HANDLE_EXPIRED_PASSWORDS (1UL << 22)")]
        public const uint CLIENT_CAN_HANDLE_EXPIRED_PASSWORDS = (1U << 22);

        [NativeTypeName("#define CLIENT_SESSION_TRACKING (1UL << 23)")]
        public const uint CLIENT_SESSION_TRACKING = (1U << 23);

        [NativeTypeName("#define CLIENT_ZSTD_COMPRESSION (1UL << 26)")]
        public const uint CLIENT_ZSTD_COMPRESSION = (1U << 26);

        [NativeTypeName("#define CLIENT_PROGRESS (1UL << 29)")]
        public const uint CLIENT_PROGRESS = (1U << 29);

        [NativeTypeName("#define CLIENT_PROGRESS_OBSOLETE CLIENT_PROGRESS")]
        public const uint CLIENT_PROGRESS_OBSOLETE = (1U << 29);

        [NativeTypeName("#define CLIENT_SSL_VERIFY_SERVER_CERT (1UL << 30)")]
        public const uint CLIENT_SSL_VERIFY_SERVER_CERT = (1U << 30);

        [NativeTypeName("#define CLIENT_SSL_VERIFY_SERVER_CERT_OBSOLETE CLIENT_SSL_VERIFY_SERVER_CERT")]
        public const uint CLIENT_SSL_VERIFY_SERVER_CERT_OBSOLETE = (1U << 30);

        [NativeTypeName("#define CLIENT_REMEMBER_OPTIONS (1UL << 31)")]
        public const uint CLIENT_REMEMBER_OPTIONS = (1U << 31);

        [NativeTypeName("#define MARIADB_CLIENT_FLAGS 0xFFFFFFFF00000000ULL")]
        public const ulong MARIADB_CLIENT_FLAGS = 0xFFFFFFFF00000000UL;

        [NativeTypeName("#define MARIADB_CLIENT_PROGRESS (1ULL << 32)")]
        public const ulong MARIADB_CLIENT_PROGRESS = (1UL << 32);

        [NativeTypeName("#define MARIADB_CLIENT_RESERVED_1 (1ULL << 33)")]
        public const ulong MARIADB_CLIENT_RESERVED_1 = (1UL << 33);

        [NativeTypeName("#define MARIADB_CLIENT_STMT_BULK_OPERATIONS (1ULL << 34)")]
        public const ulong MARIADB_CLIENT_STMT_BULK_OPERATIONS = (1UL << 34);

        [NativeTypeName("#define MARIADB_CLIENT_EXTENDED_METADATA (1ULL << 35)")]
        public const ulong MARIADB_CLIENT_EXTENDED_METADATA = (1UL << 35);

        [NativeTypeName("#define MARIADB_CLIENT_CACHE_METADATA (1ULL << 36)")]
        public const ulong MARIADB_CLIENT_CACHE_METADATA = (1UL << 36);

        [NativeTypeName("#define MARIADB_CLIENT_BULK_UNIT_RESULTS (1ULL << 37)")]
        public const ulong MARIADB_CLIENT_BULK_UNIT_RESULTS = (1UL << 37);

        [NativeTypeName("#define MARIADB_CLIENT_SUPPORTED_FLAGS (MARIADB_CLIENT_PROGRESS |\\\r\n                                       MARIADB_CLIENT_STMT_BULK_OPERATIONS|\\\r\n                                       MARIADB_CLIENT_EXTENDED_METADATA|\\\r\n                                       MARIADB_CLIENT_CACHE_METADATA|\\\r\n                                       MARIADB_CLIENT_BULK_UNIT_RESULTS)")]
        public const ulong MARIADB_CLIENT_SUPPORTED_FLAGS = ((1UL << 32) | (1UL << 34) | (1UL << 35) | (1UL << 36) | (1UL << 37));

        [NativeTypeName("#define CLIENT_SUPPORTED_FLAGS (CLIENT_MYSQL |\\\r\n                                 CLIENT_FOUND_ROWS |\\\r\n                                 CLIENT_LONG_FLAG |\\\r\n                                 CLIENT_CONNECT_WITH_DB |\\\r\n                                 CLIENT_NO_SCHEMA |\\\r\n                                 CLIENT_COMPRESS |\\\r\n                                 CLIENT_ODBC |\\\r\n                                 CLIENT_LOCAL_FILES |\\\r\n                                 CLIENT_IGNORE_SPACE |\\\r\n                                 CLIENT_INTERACTIVE |\\\r\n                                 CLIENT_SSL |\\\r\n                                 CLIENT_IGNORE_SIGPIPE |\\\r\n                                 CLIENT_TRANSACTIONS |\\\r\n                                 CLIENT_PROTOCOL_41 |\\\r\n                                 CLIENT_RESERVED |\\\r\n                                 CLIENT_SECURE_CONNECTION |\\\r\n                                 CLIENT_MULTI_STATEMENTS |\\\r\n                                 CLIENT_MULTI_RESULTS |\\\r\n                                 CLIENT_PROGRESS |\\\r\n                                 CLIENT_SSL_VERIFY_SERVER_CERT |\\\r\n                                 CLIENT_REMEMBER_OPTIONS |\\\r\n                                 CLIENT_PLUGIN_AUTH |\\\r\n                                 CLIENT_SESSION_TRACKING |\\\r\n                                 CLIENT_CONNECT_ATTRS)")]
        public const uint CLIENT_SUPPORTED_FLAGS = (1 | 2 | 4 | 8 | 16 | 32 | 64 | 128 | 256 | 1024 | 2048 | 4096 | 8192 | 512 | 16384 | 32768 | (1U << 16) | (1U << 17) | (1U << 29) | (1U << 30) | (1U << 31) | (1U << 19) | (1U << 23) | (1U << 20));

        [NativeTypeName("#define CLIENT_ALLOWED_FLAGS (CLIENT_SUPPORTED_FLAGS |\\\r\n                                 CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA |\\\r\n                                 CLIENT_CAN_HANDLE_EXPIRED_PASSWORDS |\\\r\n                                 CLIENT_ZSTD_COMPRESSION |\\\r\n                                 CLIENT_PS_MULTI_RESULTS |\\\r\n                                 CLIENT_REMEMBER_OPTIONS)")]
        public const uint CLIENT_ALLOWED_FLAGS = ((1 | 2 | 4 | 8 | 16 | 32 | 64 | 128 | 256 | 1024 | 2048 | 4096 | 8192 | 512 | 16384 | 32768 | (1U << 16) | (1U << 17) | (1U << 29) | (1U << 30) | (1U << 31) | (1U << 19) | (1U << 23) | (1U << 20)) | (1U << 21) | (1U << 22) | (1U << 26) | (1U << 18) | (1U << 31));

        [NativeTypeName("#define CLIENT_CAPABILITIES (CLIENT_MYSQL | \\\r\n                                 CLIENT_LONG_FLAG |\\\r\n                                 CLIENT_TRANSACTIONS |\\\r\n                                 CLIENT_SECURE_CONNECTION |\\\r\n                                 CLIENT_MULTI_RESULTS | \\\r\n                                 CLIENT_PS_MULTI_RESULTS |\\\r\n                                 CLIENT_PROTOCOL_41 |\\\r\n                                 CLIENT_PLUGIN_AUTH |\\\r\n                                 CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA | \\\r\n                                 CLIENT_SESSION_TRACKING |\\\r\n                                 CLIENT_CONNECT_ATTRS)")]
        public const uint CLIENT_CAPABILITIES = (1 | 4 | 8192 | 32768 | (1U << 17) | (1U << 18) | 512 | (1U << 19) | (1U << 21) | (1U << 23) | (1U << 20));

        [NativeTypeName("#define CLIENT_DEFAULT_FLAGS ((CLIENT_SUPPORTED_FLAGS & ~CLIENT_COMPRESS)\\\r\n                                                      & ~CLIENT_SSL)")]
        public const uint CLIENT_DEFAULT_FLAGS = unchecked((uint)(((1 | 2 | 4 | 8 | 16 | 32 | 64 | 128 | 256 | 1024 | 2048 | 4096 | 8192 | 512 | 16384 | 32768 | (1U << 16) | (1U << 17) | (1U << 29) | (1U << 30) | (1U << 31) | (1U << 19) | (1U << 23) | (1U << 20)) & ~32) & ~2048));

        [NativeTypeName("#define CLIENT_DEFAULT_EXTENDED_FLAGS (MARIADB_CLIENT_SUPPORTED_FLAGS &\\\r\n                                 ~MARIADB_CLIENT_BULK_UNIT_RESULTS)")]
        public const ulong CLIENT_DEFAULT_EXTENDED_FLAGS = (((1UL << 32) | (1UL << 34) | (1UL << 35) | (1UL << 36) | (1UL << 37)) & ~(1UL << 37));

        [NativeTypeName("#define SERVER_STATUS_IN_TRANS 1")]
        public const int SERVER_STATUS_IN_TRANS = 1;

        [NativeTypeName("#define SERVER_STATUS_AUTOCOMMIT 2")]
        public const int SERVER_STATUS_AUTOCOMMIT = 2;

        [NativeTypeName("#define SERVER_MORE_RESULTS_EXIST 8")]
        public const int SERVER_MORE_RESULTS_EXIST = 8;

        [NativeTypeName("#define SERVER_QUERY_NO_GOOD_INDEX_USED 16")]
        public const int SERVER_QUERY_NO_GOOD_INDEX_USED = 16;

        [NativeTypeName("#define SERVER_QUERY_NO_INDEX_USED 32")]
        public const int SERVER_QUERY_NO_INDEX_USED = 32;

        [NativeTypeName("#define SERVER_STATUS_CURSOR_EXISTS 64")]
        public const int SERVER_STATUS_CURSOR_EXISTS = 64;

        [NativeTypeName("#define SERVER_STATUS_LAST_ROW_SENT 128")]
        public const int SERVER_STATUS_LAST_ROW_SENT = 128;

        [NativeTypeName("#define SERVER_STATUS_DB_DROPPED 256")]
        public const int SERVER_STATUS_DB_DROPPED = 256;

        [NativeTypeName("#define SERVER_STATUS_NO_BACKSLASH_ESCAPES 512")]
        public const int SERVER_STATUS_NO_BACKSLASH_ESCAPES = 512;

        [NativeTypeName("#define SERVER_STATUS_METADATA_CHANGED 1024")]
        public const int SERVER_STATUS_METADATA_CHANGED = 1024;

        [NativeTypeName("#define SERVER_QUERY_WAS_SLOW 2048")]
        public const int SERVER_QUERY_WAS_SLOW = 2048;

        [NativeTypeName("#define SERVER_PS_OUT_PARAMS 4096")]
        public const int SERVER_PS_OUT_PARAMS = 4096;

        [NativeTypeName("#define SERVER_STATUS_IN_TRANS_READONLY 8192")]
        public const int SERVER_STATUS_IN_TRANS_READONLY = 8192;

        [NativeTypeName("#define SERVER_SESSION_STATE_CHANGED 16384")]
        public const int SERVER_SESSION_STATE_CHANGED = 16384;

        [NativeTypeName("#define SERVER_STATUS_ANSI_QUOTES 32768")]
        public const int SERVER_STATUS_ANSI_QUOTES = 32768;

        [NativeTypeName("#define MYSQL_ERRMSG_SIZE 512")]
        public const int MYSQL_ERRMSG_SIZE = 512;

        [NativeTypeName("#define NET_READ_TIMEOUT 30")]
        public const int NET_READ_TIMEOUT = 30;

        [NativeTypeName("#define NET_WRITE_TIMEOUT 60")]
        public const int NET_WRITE_TIMEOUT = 60;

        [NativeTypeName("#define NET_WAIT_TIMEOUT (8*60*60)")]
        public const int NET_WAIT_TIMEOUT = (8 * 60 * 60);

        [NativeTypeName("#define LIST_PROCESS_HOST_LEN 64")]
        public const int LIST_PROCESS_HOST_LEN = 64;

        [NativeTypeName("#define MYSQL50_TABLE_NAME_PREFIX \"#mysql50#\"")]
        public static ReadOnlySpan<byte> MYSQL50_TABLE_NAME_PREFIX => "#mysql50#"u8;

        [NativeTypeName("#define MYSQL50_TABLE_NAME_PREFIX_LENGTH (sizeof(MYSQL50_TABLE_NAME_PREFIX)-1)")]
        public const ulong MYSQL50_TABLE_NAME_PREFIX_LENGTH = (10 - 1);

        [NativeTypeName("#define SAFE_NAME_LEN (NAME_LEN + MYSQL50_TABLE_NAME_PREFIX_LENGTH)")]
        public const ulong SAFE_NAME_LEN = (256 + (10 - 1));

        [NativeTypeName("#define MAX_CHAR_WIDTH 255")]
        public const int MAX_CHAR_WIDTH = 255;

        [NativeTypeName("#define MAX_BLOB_WIDTH 8192")]
        public const int MAX_BLOB_WIDTH = 8192;

        [NativeTypeName("#define MAX_TINYINT_WIDTH 3")]
        public const int MAX_TINYINT_WIDTH = 3;

        [NativeTypeName("#define MAX_SMALLINT_WIDTH 5")]
        public const int MAX_SMALLINT_WIDTH = 5;

        [NativeTypeName("#define MAX_MEDIUMINT_WIDTH 8")]
        public const int MAX_MEDIUMINT_WIDTH = 8;

        [NativeTypeName("#define MAX_INT_WIDTH 10")]
        public const int MAX_INT_WIDTH = 10;

        [NativeTypeName("#define MAX_BIGINT_WIDTH 20")]
        public const int MAX_BIGINT_WIDTH = 20;

        [NativeTypeName("#define packet_error ((unsigned int) -1)")]
        public const uint packet_error = unchecked((uint)(-1));

        [NativeTypeName("#define SESSION_TRACK_BEGIN 0")]
        public const int SESSION_TRACK_BEGIN = 0;

        [NativeTypeName("#define SESSION_TRACK_END SESSION_TRACK_TRANSACTION_STATE")]
        public const int SESSION_TRACK_END = (int)SESSION_TRACK_TRANSACTION_STATE;

        [NativeTypeName("#define SESSION_TRACK_TYPES (SESSION_TRACK_END + 1)")]
        public const int SESSION_TRACK_TYPES = (int)(SESSION_TRACK_TRANSACTION_STATE + 1);

        [NativeTypeName("#define SESSION_TRACK_TRANSACTION_TYPE SESSION_TRACK_TRANSACTION_STATE")]
        public const int SESSION_TRACK_TRANSACTION_TYPE = (int)SESSION_TRACK_TRANSACTION_STATE;

        [NativeTypeName("#define FIELD_TYPE_CHAR FIELD_TYPE_TINY")]
        public const int FIELD_TYPE_CHAR = (int)MYSQL_TYPE_TINY;

        [NativeTypeName("#define FIELD_TYPE_INTERVAL FIELD_TYPE_ENUM")]
        public const int FIELD_TYPE_INTERVAL = (int)MYSQL_TYPE_ENUM;

        [NativeTypeName("#define FIELD_TYPE_DECIMAL MYSQL_TYPE_DECIMAL")]
        public const int FIELD_TYPE_DECIMAL = (int)MYSQL_TYPE_DECIMAL;

        [NativeTypeName("#define FIELD_TYPE_NEWDECIMAL MYSQL_TYPE_NEWDECIMAL")]
        public const int FIELD_TYPE_NEWDECIMAL = (int)MYSQL_TYPE_NEWDECIMAL;

        [NativeTypeName("#define FIELD_TYPE_TINY MYSQL_TYPE_TINY")]
        public const int FIELD_TYPE_TINY = (int)MYSQL_TYPE_TINY;

        [NativeTypeName("#define FIELD_TYPE_SHORT MYSQL_TYPE_SHORT")]
        public const int FIELD_TYPE_SHORT = (int)MYSQL_TYPE_SHORT;

        [NativeTypeName("#define FIELD_TYPE_LONG MYSQL_TYPE_LONG")]
        public const int FIELD_TYPE_LONG = (int)MYSQL_TYPE_LONG;

        [NativeTypeName("#define FIELD_TYPE_FLOAT MYSQL_TYPE_FLOAT")]
        public const int FIELD_TYPE_FLOAT = (int)MYSQL_TYPE_FLOAT;

        [NativeTypeName("#define FIELD_TYPE_DOUBLE MYSQL_TYPE_DOUBLE")]
        public const int FIELD_TYPE_DOUBLE = (int)MYSQL_TYPE_DOUBLE;

        [NativeTypeName("#define FIELD_TYPE_NULL MYSQL_TYPE_NULL")]
        public const int FIELD_TYPE_NULL = (int)MYSQL_TYPE_NULL;

        [NativeTypeName("#define FIELD_TYPE_TIMESTAMP MYSQL_TYPE_TIMESTAMP")]
        public const int FIELD_TYPE_TIMESTAMP = (int)MYSQL_TYPE_TIMESTAMP;

        [NativeTypeName("#define FIELD_TYPE_LONGLONG MYSQL_TYPE_LONGLONG")]
        public const int FIELD_TYPE_LONGLONG = (int)MYSQL_TYPE_LONGLONG;

        [NativeTypeName("#define FIELD_TYPE_INT24 MYSQL_TYPE_INT24")]
        public const int FIELD_TYPE_INT24 = (int)MYSQL_TYPE_INT24;

        [NativeTypeName("#define FIELD_TYPE_DATE MYSQL_TYPE_DATE")]
        public const int FIELD_TYPE_DATE = (int)MYSQL_TYPE_DATE;

        [NativeTypeName("#define FIELD_TYPE_TIME MYSQL_TYPE_TIME")]
        public const int FIELD_TYPE_TIME = (int)MYSQL_TYPE_TIME;

        [NativeTypeName("#define FIELD_TYPE_DATETIME MYSQL_TYPE_DATETIME")]
        public const int FIELD_TYPE_DATETIME = (int)MYSQL_TYPE_DATETIME;

        [NativeTypeName("#define FIELD_TYPE_YEAR MYSQL_TYPE_YEAR")]
        public const int FIELD_TYPE_YEAR = (int)MYSQL_TYPE_YEAR;

        [NativeTypeName("#define FIELD_TYPE_NEWDATE MYSQL_TYPE_NEWDATE")]
        public const int FIELD_TYPE_NEWDATE = (int)MYSQL_TYPE_NEWDATE;

        [NativeTypeName("#define FIELD_TYPE_ENUM MYSQL_TYPE_ENUM")]
        public const int FIELD_TYPE_ENUM = (int)MYSQL_TYPE_ENUM;

        [NativeTypeName("#define FIELD_TYPE_SET MYSQL_TYPE_SET")]
        public const int FIELD_TYPE_SET = (int)MYSQL_TYPE_SET;

        [NativeTypeName("#define FIELD_TYPE_TINY_BLOB MYSQL_TYPE_TINY_BLOB")]
        public const int FIELD_TYPE_TINY_BLOB = (int)MYSQL_TYPE_TINY_BLOB;

        [NativeTypeName("#define FIELD_TYPE_MEDIUM_BLOB MYSQL_TYPE_MEDIUM_BLOB")]
        public const int FIELD_TYPE_MEDIUM_BLOB = (int)MYSQL_TYPE_MEDIUM_BLOB;

        [NativeTypeName("#define FIELD_TYPE_LONG_BLOB MYSQL_TYPE_LONG_BLOB")]
        public const int FIELD_TYPE_LONG_BLOB = (int)MYSQL_TYPE_LONG_BLOB;

        [NativeTypeName("#define FIELD_TYPE_BLOB MYSQL_TYPE_BLOB")]
        public const int FIELD_TYPE_BLOB = (int)MYSQL_TYPE_BLOB;

        [NativeTypeName("#define FIELD_TYPE_VAR_STRING MYSQL_TYPE_VAR_STRING")]
        public const int FIELD_TYPE_VAR_STRING = (int)MYSQL_TYPE_VAR_STRING;

        [NativeTypeName("#define FIELD_TYPE_STRING MYSQL_TYPE_STRING")]
        public const int FIELD_TYPE_STRING = (int)MYSQL_TYPE_STRING;

        [NativeTypeName("#define FIELD_TYPE_GEOMETRY MYSQL_TYPE_GEOMETRY")]
        public const int FIELD_TYPE_GEOMETRY = (int)MYSQL_TYPE_GEOMETRY;

        [NativeTypeName("#define FIELD_TYPE_BIT MYSQL_TYPE_BIT")]
        public const int FIELD_TYPE_BIT = (int)MYSQL_TYPE_BIT;

        [NativeTypeName("#define MARIADB_CONNECTION_UNIXSOCKET 0")]
        public const int MARIADB_CONNECTION_UNIXSOCKET = 0;

        [NativeTypeName("#define MARIADB_CONNECTION_TCP 1")]
        public const int MARIADB_CONNECTION_TCP = 1;

        [NativeTypeName("#define MARIADB_CONNECTION_NAMEDPIPE 2")]
        public const int MARIADB_CONNECTION_NAMEDPIPE = 2;

        [NativeTypeName("#define MARIADB_CONNECTION_SHAREDMEM 3")]
        public const int MARIADB_CONNECTION_SHAREDMEM = 3;

        [NativeTypeName("#define NET_HEADER_SIZE 4")]
        public const int NET_HEADER_SIZE = 4;

        [NativeTypeName("#define COMP_HEADER_SIZE 3")]
        public const int COMP_HEADER_SIZE = 3;

        [NativeTypeName("#define native_password_plugin_name \"mysql_native_password\"")]
        public static ReadOnlySpan<byte> native_password_plugin_name => "mysql_native_password"u8;

        [NativeTypeName("#define old_password_plugin_name \"mysql_old_password\"")]
        public static ReadOnlySpan<byte> old_password_plugin_name => "mysql_old_password"u8;

        [NativeTypeName("#define NULL_LENGTH ((unsigned long) ~0)")]
        public const uint NULL_LENGTH = unchecked((uint)(~0));
    }
}
