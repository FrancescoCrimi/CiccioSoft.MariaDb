using System;
using System.Runtime.InteropServices;
using static CiccioSoft.MariaDb.Interop.Native.MySqlFieldTypes;
using static CiccioSoft.MariaDb.Interop.Native.enum_session_state_type;

namespace CiccioSoft.MariaDb.Interop.Native
{
    internal static unsafe partial class MariadbComNative
    {
        [NativeTypeName("#define CLIENT_DEFAULT_FLAGS ((CLIENT_SUPPORTED_FLAGS & ~CLIENT_COMPRESS)\\\r\n                                                      & ~CLIENT_SSL)")]
        public const uint CLIENT_DEFAULT_FLAGS = unchecked((uint)(((1 | 2 | 4 | 8 | 16 | 32 | 64 | 128 | 256 | 1024 | 2048 | 4096 | 8192 | 512 | 16384 | 32768 | (1U << 16) | (1U << 17) | (1U << 29) | (1U << 30) | (1U << 31) | (1U << 19) | (1U << 23) | (1U << 20)) & ~32) & ~2048));


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



        // Function like macro definition records not supported by ClangSharp
        // IS_MARIADB_EXTENDED_SERVER net_new_transaction
    }
}
