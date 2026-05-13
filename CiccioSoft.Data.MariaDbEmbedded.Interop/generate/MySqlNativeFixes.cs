using static CiccioSoft.Data.MariaDbEmbedded.Interop.Native.mariadb_field_attr_t;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal static unsafe partial class MySqlNative
    {
        [NativeTypeName("#define MARIADB_FIELD_ATTR_LAST MARIADB_FIELD_ATTR_FORMAT_NAME")]
        public const int MARIADB_FIELD_ATTR_LAST = (int)MARIADB_FIELD_ATTR_FORMAT_NAME;

        // [NativeTypeName("#define unknown_sqlstate SQLSTATE_UNKNOWN")]
        // public static readonly byte* unknown_sqlstate = SQLSTATE_UNKNOWN;


        // Function like macro definition records not supported by ClangSharp

        // #define IS_PRI_KEY(n) ((n) & PRI_KEY_FLAG)
        public static bool IsPriKey(int n) => (n & MariadbComNative.PRI_KEY_FLAG) != 0;

        // #define IS_NOT_NULL(n) ((n) & NOT_NULL_FLAG)
        public static bool IsNotNull(int n) => (n & MariadbComNative.NOT_NULL_FLAG) != 0;

        // #define IS_BLOB(n) ((n) & BLOB_FLAG)
        public static bool IsBlob(int n) => (n & MariadbComNative.BLOB_FLAG) != 0;


        // #define IS_NUM(t) (((t) <= MYSQL_TYPE_INT24 && (t) != MYSQL_TYPE_TIMESTAMP) || (t) == MYSQL_TYPE_YEAR || (t) == MYSQL_TYPE_NEWDECIMAL)
        public static bool IsNum(MySqlFieldTypes t)
        {
            return ((t <= MySqlFieldTypes.MYSQL_TYPE_INT24 && t != MySqlFieldTypes.MYSQL_TYPE_TIMESTAMP)
                    || t == MySqlFieldTypes.MYSQL_TYPE_YEAR
                    || t == MySqlFieldTypes.MYSQL_TYPE_NEWDECIMAL);
        }


        // #define IS_NUM_FIELD(f) ((f)->flags & NUM_FLAG)
        public static bool IsNumField(MySqlField f) => (f.Flags & MariadbComNative.NUM_FLAG) != 0;

        // #define INTERNAL_NUM_FIELD(f) ...
        public static bool InternalNumField(MySqlField f)
        {
            return ((f.Type <= MySqlFieldTypes.MYSQL_TYPE_INT24 &&
                    (f.Type != MySqlFieldTypes.MYSQL_TYPE_TIMESTAMP || f.Length == 14 || f.Length == 8))
                    || f.Type == MySqlFieldTypes.MYSQL_TYPE_YEAR
                    || f.Type == MySqlFieldTypes.MYSQL_TYPE_NEWDECIMAL
                    || f.Type == MySqlFieldTypes.MYSQL_TYPE_DECIMAL);
        }


        // SET_CLIENT_ERROR set_mariadb_error CLEAR_CLIENT_ERROR mysql_reload mariadb_connect
        // must be wrapped in C wrapper like that
        /*
            // Questa funzione "reale" può essere esportata
            __declspec(dllexport) int SET_CLIENT_ERROR_Wrapper(int a, int b) {
                return SET_CLIENT_ERROR(a, b, c, d);
            }
        */
    }
}
