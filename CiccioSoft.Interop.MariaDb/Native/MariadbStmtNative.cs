using System.Runtime.InteropServices;

namespace CiccioSoft.Interop.MariaDb.Native
{
    internal static unsafe partial class MariadbStmtNative
    {
        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long")]
        public static extern uint ma_net_safe_read([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void mysql_init_ps_subsystem();

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long")]
        public static extern uint net_field_length([NativeTypeName("unsigned char **")] byte** packet);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int ma_simple_command([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum enum_server_command")] enum_server_command command, [NativeTypeName("const char *")] byte* arg, [NativeTypeName("size_t")] nuint length, [NativeTypeName("my_bool")] sbyte skipp_check, void* opt_arg);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void stmt_set_error([NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("unsigned int")] uint error_nr, [NativeTypeName("const char *")] byte* sqlstate, [NativeTypeName("const char *")] byte* format, __arglist);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_STMT *")]
        public static extern nint mysql_stmt_init([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_prepare([NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("const char *")] byte* query, [NativeTypeName("unsigned long")] uint length);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_execute([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_fetch([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_fetch_column([NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("MYSQL_BIND *")] MySqlBindNative* bind_arg, [NativeTypeName("unsigned int")] uint column, [NativeTypeName("unsigned long")] uint offset);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_store_result([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long")]
        public static extern uint mysql_stmt_param_count([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_stmt_attr_set([NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("enum enum_stmt_attr_type")] enum_stmt_attr_type attr_type, [NativeTypeName("const void *")] void* attr);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_stmt_attr_get([NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("enum enum_stmt_attr_type")] enum_stmt_attr_type attr_type, void* attr);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_stmt_bind_param([NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("MYSQL_BIND *")] MySqlBindNative* bnd);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_stmt_bind_result([NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("MYSQL_BIND *")] MySqlBindNative* bnd);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_stmt_close([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_stmt_reset([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_stmt_free_result([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_stmt_send_long_data([NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("unsigned int")] uint param_number, [NativeTypeName("const char *")] byte* data, [NativeTypeName("unsigned long")] uint length);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_RES *")]
        public static extern nint mysql_stmt_result_metadata([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_RES *")]
        public static extern nint mysql_stmt_param_metadata([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint mysql_stmt_errno([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern byte* mysql_stmt_error([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern byte* mysql_stmt_sqlstate([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_ROW_OFFSET")]
        public static extern st_mysql_rows* mysql_stmt_row_seek([NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("MYSQL_ROW_OFFSET")] st_mysql_rows* offset);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_ROW_OFFSET")]
        public static extern st_mysql_rows* mysql_stmt_row_tell([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern void mysql_stmt_data_seek([NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("unsigned long long")] ulong offset);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long long")]
        public static extern ulong mysql_stmt_num_rows([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long long")]
        public static extern ulong mysql_stmt_affected_rows([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long long")]
        public static extern ulong mysql_stmt_insert_id([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint mysql_stmt_field_count([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_next_result([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_stmt_more_results([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mariadb_stmt_execute_direct([NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("const char *")] byte* stmt_str, [NativeTypeName("size_t")] nuint length);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_FIELD *")]
        public static extern MySqlFieldNative* mariadb_stmt_fetch_fields([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [NativeTypeName("#define MYSQL_NO_DATA 100")]
        public const int MYSQL_NO_DATA = 100;

        [NativeTypeName("#define MYSQL_DATA_TRUNCATED 101")]
        public const int MYSQL_DATA_TRUNCATED = 101;

        [NativeTypeName("#define MYSQL_DEFAULT_PREFETCH_ROWS (unsigned long) 1")]
        public const uint MYSQL_DEFAULT_PREFETCH_ROWS = (uint)(1);

        [NativeTypeName("#define MADB_BIND_DUMMY 1")]
        public const int MADB_BIND_DUMMY = 1;

        [NativeTypeName("#define MYSQL_PS_SKIP_RESULT_W_LEN -1")]
        public const int MYSQL_PS_SKIP_RESULT_W_LEN = -1;

        [NativeTypeName("#define MYSQL_PS_SKIP_RESULT_STR -2")]
        public const int MYSQL_PS_SKIP_RESULT_STR = -2;

        [NativeTypeName("#define STMT_ID_LENGTH 4")]
        public const int STMT_ID_LENGTH = 4;

        [NativeTypeName("#define STMT_BULK_FLAG_CLIENT_SEND_TYPES 128")]
        public const int STMT_BULK_FLAG_CLIENT_SEND_TYPES = 128;

        [NativeTypeName("#define STMT_BULK_FLAG_SEND_UNIT_RESULTS 64")]
        public const int STMT_BULK_FLAG_SEND_UNIT_RESULTS = 64;
    }
}
