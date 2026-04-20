using System.Runtime.InteropServices;
using static CiccioSoft.Data.MariaDbEmbedded.Interop.Native.mariadb_field_attr_t;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal static unsafe partial class NativeMySql
    {
        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mariadb_field_attr([NativeTypeName("MARIADB_CONST_STRING *")] st_ma_const_string* attr, [NativeTypeName("const MYSQL_FIELD *")] nint field, [NativeTypeName("enum mariadb_field_attr_t")] mariadb_field_attr_t type);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("struct st_mysql_client_plugin *")]
        public static extern st_mysql_client_plugin* mysql_load_plugin([NativeTypeName("struct st_mysql *")] nint mysql, [NativeTypeName("const char *")] byte* name, int type, int argc, __arglist);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("struct st_mysql_client_plugin *")]
        public static extern st_mysql_client_plugin* mysql_load_plugin_v([NativeTypeName("struct st_mysql *")] nint mysql, [NativeTypeName("const char *")] byte* name, int type, int argc, [NativeTypeName("va_list")] byte* args);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("struct st_mysql_client_plugin *")]
        public static extern st_mysql_client_plugin* mysql_client_find_plugin([NativeTypeName("struct st_mysql *")] nint mysql, [NativeTypeName("const char *")] byte* name, int type);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("struct st_mysql_client_plugin *")]
        public static extern st_mysql_client_plugin* mysql_client_register_plugin([NativeTypeName("struct st_mysql *")] nint mysql, [NativeTypeName("struct st_mysql_client_plugin *")] st_mysql_client_plugin* plugin);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern void mysql_set_local_infile_handler([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("int (*)(void **, const char *, void *)")] delegate* unmanaged[Cdecl]<void**, byte*, void*, int> local_infile_init, [NativeTypeName("int (*)(void *, char *, unsigned int)")] delegate* unmanaged[Cdecl]<void*, byte*, uint, int> local_infile_read, [NativeTypeName("void (*)(void *)")] delegate* unmanaged[Cdecl]<void*, void> local_infile_end, [NativeTypeName("int (*)(void *, char *, unsigned int)")] delegate* unmanaged[Cdecl]<void*, byte*, uint, int> local_infile_error, void* param5);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void mysql_set_local_infile_default([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void my_set_error([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("unsigned int")] uint error_nr, [NativeTypeName("const char *")] byte* sqlstate, [NativeTypeName("const char *")] byte* format, __arglist);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_ulonglong")]
        public static extern ulong mysql_num_rows([NativeTypeName("MYSQL_RES *")] nint res);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint mysql_num_fields([NativeTypeName("MYSQL_RES *")] nint res);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_eof([NativeTypeName("MYSQL_RES *")] nint res);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_FIELD *")]
        public static extern nint mysql_fetch_field_direct([NativeTypeName("MYSQL_RES *")] nint res, [NativeTypeName("unsigned int")] uint fieldnr);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_FIELD *")]
        public static extern nint mysql_fetch_fields([NativeTypeName("MYSQL_RES *")] nint res);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_ROWS *")]
        public static extern st_mysql_rows* mysql_row_tell([NativeTypeName("MYSQL_RES *")] nint res);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint mysql_field_tell([NativeTypeName("MYSQL_RES *")] nint res);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint mysql_field_count([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_more_results([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_next_result([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_ulonglong")]
        public static extern ulong mysql_affected_rows([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_autocommit([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("my_bool")] sbyte mode);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_commit([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_rollback([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_ulonglong")]
        public static extern ulong mysql_insert_id([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint mysql_errno([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern byte* mysql_error([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern byte* mysql_info([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long")]
        public static extern uint mysql_thread_id([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern byte* mysql_character_set_name([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern void mysql_get_character_set_info([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("MY_CHARSET_INFO *")] character_set* cs);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_set_character_set([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* csname);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mariadb_get_infov([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum mariadb_value")] mariadb_value value, void* arg, __arglist);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mariadb_get_info([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum mariadb_value")] mariadb_value value, void* arg);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL *")]
        public static extern nint mysql_init([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_ssl_set([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* key, [NativeTypeName("const char *")] byte* cert, [NativeTypeName("const char *")] byte* ca, [NativeTypeName("const char *")] byte* capath, [NativeTypeName("const char *")] byte* cipher);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern byte* mysql_get_ssl_cipher([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_change_user([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* user, [NativeTypeName("const char *")] byte* passwd, [NativeTypeName("const char *")] byte* db);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL *")]
        public static extern nint mysql_real_connect([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* host, [NativeTypeName("const char *")] byte* user, [NativeTypeName("const char *")] byte* passwd, [NativeTypeName("const char *")] byte* db, [NativeTypeName("unsigned int")] uint port, [NativeTypeName("const char *")] byte* unix_socket, [NativeTypeName("unsigned long")] uint clientflag);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern void mysql_close([NativeTypeName("MYSQL *")] nint sock);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_select_db([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* db);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_query([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* q);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_send_query([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* q, [NativeTypeName("unsigned long")] uint length);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_read_query_result([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_real_query([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* q, [NativeTypeName("unsigned long")] uint length);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_shutdown([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum mysql_enum_shutdown_level")] mysql_enum_shutdown_level shutdown_level);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_dump_debug_info([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_refresh([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("unsigned int")] uint refresh_options);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_kill([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("unsigned long")] uint pid);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_ping([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern byte* mysql_stat([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern byte* mysql_get_server_info([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long")]
        public static extern uint mysql_get_server_version([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("char *")]
        public static extern byte* mysql_get_host_info([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint mysql_get_proto_info([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_RES *")]
        public static extern nint mysql_list_dbs([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* wild);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_RES *")]
        public static extern nint mysql_list_tables([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* wild);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_RES *")]
        public static extern nint mysql_list_fields([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* table, [NativeTypeName("const char *")] byte* wild);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_RES *")]
        public static extern nint mysql_list_processes([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_RES *")]
        public static extern nint mysql_store_result([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_RES *")]
        public static extern nint mysql_use_result([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_options([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum mysql_option")] MySqlOption option, [NativeTypeName("const void *")] void* arg);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_options4([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum mysql_option")] MySqlOption option, [NativeTypeName("const void *")] void* arg1, [NativeTypeName("const void *")] void* arg2);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern void mysql_free_result([NativeTypeName("MYSQL_RES *")] nint result);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern void mysql_data_seek([NativeTypeName("MYSQL_RES *")] nint result, [NativeTypeName("unsigned long long")] ulong offset);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_ROW_OFFSET")]
        public static extern st_mysql_rows* mysql_row_seek([NativeTypeName("MYSQL_RES *")] nint result, [NativeTypeName("MYSQL_ROW_OFFSET")] st_mysql_rows* param1);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_FIELD_OFFSET")]
        public static extern uint mysql_field_seek([NativeTypeName("MYSQL_RES *")] nint result, [NativeTypeName("MYSQL_FIELD_OFFSET")] uint offset);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MYSQL_ROW")]
        public static extern byte** mysql_fetch_row([NativeTypeName("MYSQL_RES *")] nint result);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long *")]
        public static extern uint* mysql_fetch_lengths([NativeTypeName("MYSQL_RES *")] nint result);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long")]
        public static extern uint mysql_escape_string([NativeTypeName("char *")] byte* to, [NativeTypeName("const char *")] byte* from, [NativeTypeName("unsigned long")] uint from_length);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long")]
        public static extern uint mysql_real_escape_string([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("char *")] byte* to, [NativeTypeName("const char *")] byte* from, [NativeTypeName("unsigned long")] uint length);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint mysql_thread_safe();

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint mysql_warning_count([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern byte* mysql_sqlstate([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_server_init(int argc, [NativeTypeName("char **")] byte** argv, [NativeTypeName("char **")] byte** groups);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern void mysql_server_end();

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern void mysql_thread_end();

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_thread_init();

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_set_server_option([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum enum_mysql_set_option")] enum_mysql_set_option option);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern byte* mysql_get_client_info();

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long")]
        public static extern uint mysql_get_client_version();

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mariadb_connection([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern byte* mysql_get_server_name([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MARIADB_CHARSET_INFO *")]
        public static extern nint mariadb_get_charset_by_name([NativeTypeName("const char *")] byte* csname);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("MARIADB_CHARSET_INFO *")]
        public static extern nint mariadb_get_charset_by_nr([NativeTypeName("unsigned int")] uint csnr);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("size_t")]
        public static extern nuint mariadb_convert_string([NativeTypeName("const char *")] byte* from, [NativeTypeName("size_t *")] nuint* from_len, [NativeTypeName("MARIADB_CHARSET_INFO *")] nint from_cs, [NativeTypeName("char *")] byte* to, [NativeTypeName("size_t *")] nuint* to_len, [NativeTypeName("MARIADB_CHARSET_INFO *")] nint to_cs, int* errorcode);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int mysql_optionsv([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum mysql_option")] MySqlOption option, __arglist);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int mysql_get_optionv([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum mysql_option")] MySqlOption option, void* arg, __arglist);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_get_option([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum mysql_option")] MySqlOption option, void* arg);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long")]
        public static extern uint mysql_hex_string([NativeTypeName("char *")] byte* to, [NativeTypeName("const char *")] byte* from, [NativeTypeName("unsigned long")] uint len);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long long")]
        public static extern ulong mysql_get_socket([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint mysql_get_timeout_value([NativeTypeName("const MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint mysql_get_timeout_value_ms([NativeTypeName("const MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mariadb_reconnect([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mariadb_cancel([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern void mysql_debug([NativeTypeName("const char *")] byte* debug);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long")]
        public static extern uint mysql_net_read_packet([NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("unsigned long")]
        public static extern uint mysql_net_field_length([NativeTypeName("unsigned char **")] byte** packet);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        [return: NativeTypeName("my_bool")]
        public static extern sbyte mysql_embedded();

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern MYSQL_PARAMETERS* mysql_get_parameters();

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_close_start([NativeTypeName("MYSQL *")] nint sock);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_close_cont([NativeTypeName("MYSQL *")] nint sock, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_commit_start([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_commit_cont([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_dump_debug_info_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int ready_status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_dump_debug_info_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_rollback_start([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_rollback_cont([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_autocommit_start([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("my_bool")] sbyte auto_mode);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_list_fields_cont([NativeTypeName("MYSQL_RES **")] nint* ret, [NativeTypeName("MYSQL *")] nint mysql, int ready_status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_list_fields_start([NativeTypeName("MYSQL_RES **")] nint* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* table, [NativeTypeName("const char *")] byte* wild);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_autocommit_cont([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_next_result_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_next_result_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_select_db_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* db);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_select_db_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int ready_status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_warning_count([NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_next_result_start(int* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_next_result_cont(int* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_set_character_set_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* csname);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_set_character_set_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_change_user_start([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* user, [NativeTypeName("const char *")] byte* passwd, [NativeTypeName("const char *")] byte* db);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_change_user_cont([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_real_connect_start([NativeTypeName("MYSQL **")] nint* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* host, [NativeTypeName("const char *")] byte* user, [NativeTypeName("const char *")] byte* passwd, [NativeTypeName("const char *")] byte* db, [NativeTypeName("unsigned int")] uint port, [NativeTypeName("const char *")] byte* unix_socket, [NativeTypeName("unsigned long")] uint clientflag);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_real_connect_cont([NativeTypeName("MYSQL **")] nint* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_query_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* q);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_query_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_send_query_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* q, [NativeTypeName("unsigned long")] uint length);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_send_query_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_real_query_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("const char *")] byte* q, [NativeTypeName("unsigned long")] uint length);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_real_query_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_store_result_start([NativeTypeName("MYSQL_RES **")] nint* ret, [NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_store_result_cont([NativeTypeName("MYSQL_RES **")] nint* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_shutdown_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum mysql_enum_shutdown_level")] mysql_enum_shutdown_level shutdown_level);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_shutdown_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_refresh_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("unsigned int")] uint refresh_options);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_refresh_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_kill_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("unsigned long")] uint pid);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_kill_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_set_server_option_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum enum_mysql_set_option")] enum_mysql_set_option option);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_set_server_option_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_ping_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_ping_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stat_start([NativeTypeName("const char **")] byte** ret, [NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stat_cont([NativeTypeName("const char **")] byte** ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_free_result_start([NativeTypeName("MYSQL_RES *")] nint result);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_free_result_cont([NativeTypeName("MYSQL_RES *")] nint result, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_fetch_row_start([NativeTypeName("MYSQL_ROW *")] byte*** ret, [NativeTypeName("MYSQL_RES *")] nint result);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_fetch_row_cont([NativeTypeName("MYSQL_ROW *")] byte*** ret, [NativeTypeName("MYSQL_RES *")] nint result, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_read_query_result_start([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_read_query_result_cont([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_reset_connection_start(int* ret, [NativeTypeName("MYSQL *")] nint mysql);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_reset_connection_cont(int* ret, [NativeTypeName("MYSQL *")] nint mysql, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_session_track_get_next([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum enum_session_state_type")] enum_session_state_type type, [NativeTypeName("const char **")] byte** data, [NativeTypeName("size_t *")] nuint* length);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_session_track_get_first([NativeTypeName("MYSQL *")] nint mysql, [NativeTypeName("enum enum_session_state_type")] enum_session_state_type type, [NativeTypeName("const char **")] byte** data, [NativeTypeName("size_t *")] nuint* length);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_prepare_start(int* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("const char *")] byte* query, [NativeTypeName("unsigned long")] uint length);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_prepare_cont(int* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_execute_start(int* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_execute_cont(int* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_fetch_start(int* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_fetch_cont(int* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_store_result_start(int* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_store_result_cont(int* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_close_start([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_close_cont([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_reset_start([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_reset_cont([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_free_result_start([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_free_result_cont([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_send_long_data_start([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt, [NativeTypeName("unsigned int")] uint param_number, [NativeTypeName("const char *")] byte* data, [NativeTypeName("unsigned long")] uint len);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_stmt_send_long_data_cont([NativeTypeName("my_bool *")] byte* ret, [NativeTypeName("MYSQL_STMT *")] nint stmt, int status);

        [DllImport("libmariadb", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int mysql_reset_connection([NativeTypeName("MYSQL *")] nint mysql);

        // [NativeTypeName("#define unknown_sqlstate SQLSTATE_UNKNOWN")]
        // public static readonly byte* unknown_sqlstate = SQLSTATE_UNKNOWN;

        [NativeTypeName("#define MYSQL_COUNT_ERROR (~(unsigned long long) 0)")]
        public const ulong MYSQL_COUNT_ERROR = (~(ulong)(0));

        [NativeTypeName("#define MARIADB_FIELD_ATTR_LAST MARIADB_FIELD_ATTR_FORMAT_NAME")]
        public const int MARIADB_FIELD_ATTR_LAST = (int)MARIADB_FIELD_ATTR_FORMAT_NAME;

        [NativeTypeName("#define AUTO_SEC_PART_DIGITS 39")]
        public const int AUTO_SEC_PART_DIGITS = 39;

        [NativeTypeName("#define SEC_PART_DIGITS 6")]
        public const int SEC_PART_DIGITS = 6;

        [NativeTypeName("#define MARIADB_INVALID_SOCKET -1")]
        public const int MARIADB_INVALID_SOCKET = -1;

        [NativeTypeName("#define MYSQL_WAIT_READ 1")]
        public const int MYSQL_WAIT_READ = 1;

        [NativeTypeName("#define MYSQL_WAIT_WRITE 2")]
        public const int MYSQL_WAIT_WRITE = 2;

        [NativeTypeName("#define MYSQL_WAIT_EXCEPT 4")]
        public const int MYSQL_WAIT_EXCEPT = 4;

        [NativeTypeName("#define MYSQL_WAIT_TIMEOUT 8")]
        public const int MYSQL_WAIT_TIMEOUT = 8;

        [NativeTypeName("#define MARIADB_TLS_VERIFY_OK 0")]
        public const int MARIADB_TLS_VERIFY_OK = 0;

        [NativeTypeName("#define MARIADB_TLS_VERIFY_TRUST 1")]
        public const int MARIADB_TLS_VERIFY_TRUST = 1;

        [NativeTypeName("#define MARIADB_TLS_VERIFY_HOST 2")]
        public const int MARIADB_TLS_VERIFY_HOST = 2;

        [NativeTypeName("#define MARIADB_TLS_VERIFY_FINGERPRINT 4")]
        public const int MARIADB_TLS_VERIFY_FINGERPRINT = 4;

        [NativeTypeName("#define MARIADB_TLS_VERIFY_PERIOD 8")]
        public const int MARIADB_TLS_VERIFY_PERIOD = 8;

        [NativeTypeName("#define MARIADB_TLS_VERIFY_REVOKED 16")]
        public const int MARIADB_TLS_VERIFY_REVOKED = 16;

        [NativeTypeName("#define MARIADB_TLS_VERIFY_UNKNOWN 32")]
        public const int MARIADB_TLS_VERIFY_UNKNOWN = 32;

        [NativeTypeName("#define MARIADB_TLS_VERIFY_ERROR 128")]
        public const int MARIADB_TLS_VERIFY_ERROR = 128;

        [NativeTypeName("#define LOCAL_INFILE_ERROR_LEN 512")]
        public const int LOCAL_INFILE_ERROR_LEN = 512;

        [NativeTypeName("#define mysql_library_init mysql_server_init")]
        public static readonly delegate*<int, byte**, byte**, int> mysql_library_init = &mysql_server_init;

        [NativeTypeName("#define mysql_library_end mysql_server_end")]
        public static readonly delegate*<void> mysql_library_end = &mysql_server_end;
    }
}
