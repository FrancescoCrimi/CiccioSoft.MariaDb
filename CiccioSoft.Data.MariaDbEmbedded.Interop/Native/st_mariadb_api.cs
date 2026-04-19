namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct st_mariadb_api
    {
        [NativeTypeName("unsigned long long (*)(MYSQL_RES *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, ulong> mysql_num_rows;

        [NativeTypeName("unsigned int (*)(MYSQL_RES *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, uint> mysql_num_fields;

        [NativeTypeName("my_bool (*)(MYSQL_RES *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, sbyte> mysql_eof;

        [NativeTypeName("MYSQL_FIELD *(*)(MYSQL_RES *, unsigned int) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, uint, nint*> mysql_fetch_field_direct;

        [NativeTypeName("MYSQL_FIELD *(*)(MYSQL_RES *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, nint*> mysql_fetch_fields;

        [NativeTypeName("MYSQL_ROWS *(*)(MYSQL_RES *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, st_mysql_rows*> mysql_row_tell;

        [NativeTypeName("unsigned int (*)(MYSQL_RES *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, uint> mysql_field_tell;

        [NativeTypeName("unsigned int (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint> mysql_field_count;

        [NativeTypeName("my_bool (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, sbyte> mysql_more_results;

        [NativeTypeName("int (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, int> mysql_next_result;

        [NativeTypeName("unsigned long long (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, ulong> mysql_affected_rows;

        [NativeTypeName("my_bool (*)(MYSQL *, my_bool) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, sbyte, sbyte> mysql_autocommit;

        [NativeTypeName("my_bool (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, sbyte> mysql_commit;

        [NativeTypeName("my_bool (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, sbyte> mysql_rollback;

        [NativeTypeName("unsigned long long (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, ulong> mysql_insert_id;

        [NativeTypeName("unsigned int (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint> mysql_errno;

        [NativeTypeName("const char *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*> mysql_error;

        [NativeTypeName("const char *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*> mysql_info;

        [NativeTypeName("unsigned long (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint> mysql_thread_id;

        [NativeTypeName("const char *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*> mysql_character_set_name;

        [NativeTypeName("void (*)(MYSQL *, MY_CHARSET_INFO *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, character_set*, void> mysql_get_character_set_info;

        [NativeTypeName("int (*)(MYSQL *, const char *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, int> mysql_set_character_set;

        [NativeTypeName("my_bool (*)(MYSQL *, enum mariadb_value, void *, ...)")]
        public delegate* unmanaged[Cdecl]<nint, mariadb_value, void*, sbyte> mariadb_get_infov;

        [NativeTypeName("my_bool (*)(MYSQL *, enum mariadb_value, void *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, mariadb_value, void*, sbyte> mariadb_get_info;

        [NativeTypeName("MYSQL *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, nint> mysql_init;

        [NativeTypeName("int (*)(MYSQL *, const char *, const char *, const char *, const char *, const char *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, byte*, byte*, byte*, byte*, int> mysql_ssl_set;

        [NativeTypeName("const char *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*> mysql_get_ssl_cipher;

        [NativeTypeName("my_bool (*)(MYSQL *, const char *, const char *, const char *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, byte*, byte*, sbyte> mysql_change_user;

        [NativeTypeName("MYSQL *(*)(MYSQL *, const char *, const char *, const char *, const char *, unsigned int, const char *, unsigned long) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, byte*, byte*, byte*, uint, byte*, uint, nint> mysql_real_connect;

        [NativeTypeName("void (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, void> mysql_close;

        [NativeTypeName("int (*)(MYSQL *, const char *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, int> mysql_select_db;

        [NativeTypeName("int (*)(MYSQL *, const char *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, int> mysql_query;

        [NativeTypeName("int (*)(MYSQL *, const char *, unsigned long) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, uint, int> mysql_send_query;

        [NativeTypeName("my_bool (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, sbyte> mysql_read_query_result;

        [NativeTypeName("int (*)(MYSQL *, const char *, unsigned long) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, uint, int> mysql_real_query;

        [NativeTypeName("int (*)(MYSQL *, enum mysql_enum_shutdown_level) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, mysql_enum_shutdown_level, int> mysql_shutdown;

        [NativeTypeName("int (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, int> mysql_dump_debug_info;

        [NativeTypeName("int (*)(MYSQL *, unsigned int) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint, int> mysql_refresh;

        [NativeTypeName("int (*)(MYSQL *, unsigned long) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint, int> mysql_kill;

        [NativeTypeName("int (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, int> mysql_ping;

        [NativeTypeName("char *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*> mysql_stat;

        [NativeTypeName("char *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*> mysql_get_server_info;

        [NativeTypeName("unsigned long (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint> mysql_get_server_version;

        [NativeTypeName("char *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*> mysql_get_host_info;

        [NativeTypeName("unsigned int (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint> mysql_get_proto_info;

        [NativeTypeName("MYSQL_RES *(*)(MYSQL *, const char *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, nint*> mysql_list_dbs;

        [NativeTypeName("MYSQL_RES *(*)(MYSQL *, const char *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, nint*> mysql_list_tables;

        [NativeTypeName("MYSQL_RES *(*)(MYSQL *, const char *, const char *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, byte*, nint*> mysql_list_fields;

        [NativeTypeName("MYSQL_RES *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, nint*> mysql_list_processes;

        [NativeTypeName("MYSQL_RES *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, nint*> mysql_store_result;

        [NativeTypeName("MYSQL_RES *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, nint*> mysql_use_result;

        [NativeTypeName("int (*)(MYSQL *, enum mysql_option, const void *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, mysql_option, void*, int> mysql_options;

        [NativeTypeName("void (*)(MYSQL_RES *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, void> mysql_free_result;

        [NativeTypeName("void (*)(MYSQL_RES *, unsigned long long) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, ulong, void> mysql_data_seek;

        [NativeTypeName("MYSQL_ROW_OFFSET (*)(MYSQL_RES *, MYSQL_ROW_OFFSET) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, st_mysql_rows*, st_mysql_rows*> mysql_row_seek;

        [NativeTypeName("MYSQL_FIELD_OFFSET (*)(MYSQL_RES *, MYSQL_FIELD_OFFSET) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, uint, uint> mysql_field_seek;

        [NativeTypeName("MYSQL_ROW (*)(MYSQL_RES *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, byte**> mysql_fetch_row;

        [NativeTypeName("unsigned long *(*)(MYSQL_RES *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, uint*> mysql_fetch_lengths;

        [NativeTypeName("MYSQL_FIELD *(*)(MYSQL_RES *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint*, nint*> mysql_fetch_field;

        [NativeTypeName("unsigned long (*)(char *, const char *, unsigned long) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<byte*, byte*, uint, uint> mysql_escape_string;

        [NativeTypeName("unsigned long (*)(MYSQL *, char *, const char *, unsigned long) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, byte*, uint, uint> mysql_real_escape_string;

        [NativeTypeName("unsigned int (*)(void) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<uint> mysql_thread_safe;

        [NativeTypeName("unsigned int (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint> mysql_warning_count;

        [NativeTypeName("const char *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*> mysql_sqlstate;

        [NativeTypeName("int (*)(int, char **, char **) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<int, byte**, byte**, int> mysql_server_init;

        [NativeTypeName("void (*)(void) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<void> mysql_server_end;

        [NativeTypeName("void (*)(void) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<void> mysql_thread_end;

        [NativeTypeName("my_bool (*)(void) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<sbyte> mysql_thread_init;

        [NativeTypeName("int (*)(MYSQL *, enum enum_mysql_set_option) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, enum_mysql_set_option, int> mysql_set_server_option;

        [NativeTypeName("const char *(*)(void) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<byte*> mysql_get_client_info;

        [NativeTypeName("unsigned long (*)(void) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<uint> mysql_get_client_version;

        [NativeTypeName("my_bool (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, sbyte> mariadb_connection;

        [NativeTypeName("const char *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*> mysql_get_server_name;

        [NativeTypeName("MARIADB_CHARSET_INFO *(*)(const char *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<byte*, nint> mariadb_get_charset_by_name;

        [NativeTypeName("MARIADB_CHARSET_INFO *(*)(unsigned int) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<uint, nint> mariadb_get_charset_by_nr;

        [NativeTypeName("size_t (*)(const char *, size_t *, MARIADB_CHARSET_INFO *, char *, size_t *, MARIADB_CHARSET_INFO *, int *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<byte*, nuint*, nint, byte*, nuint*, nint, int*, nuint> mariadb_convert_string;

        [NativeTypeName("int (*)(MYSQL *, enum mysql_option, ...)")]
        public delegate* unmanaged[Cdecl]<nint, mysql_option, int> mysql_optionsv;

        [NativeTypeName("int (*)(MYSQL *, enum mysql_option, void *, ...)")]
        public delegate* unmanaged[Cdecl]<nint, mysql_option, void*, int> mysql_get_optionv;

        [NativeTypeName("int (*)(MYSQL *, enum mysql_option, void *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, mysql_option, void*, int> mysql_get_option;

        [NativeTypeName("unsigned long (*)(char *, const char *, unsigned long) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<byte*, byte*, uint, uint> mysql_hex_string;

        [NativeTypeName("unsigned long long (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, ulong> mysql_get_socket;

        [NativeTypeName("unsigned int (*)(const MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint> mysql_get_timeout_value;

        [NativeTypeName("unsigned int (*)(const MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint> mysql_get_timeout_value_ms;

        [NativeTypeName("my_bool (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, sbyte> mariadb_reconnect;

        [NativeTypeName("MYSQL_STMT *(*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, nint> mysql_stmt_init;

        [NativeTypeName("int (*)(MYSQL_STMT *, const char *, unsigned long) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, uint, int> mysql_stmt_prepare;

        [NativeTypeName("int (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, int> mysql_stmt_execute;

        [NativeTypeName("int (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, int> mysql_stmt_fetch;

        [NativeTypeName("int (*)(MYSQL_STMT *, MYSQL_BIND *, unsigned int, unsigned long) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, st_mysql_bind*, uint, uint, int> mysql_stmt_fetch_column;

        [NativeTypeName("int (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, int> mysql_stmt_store_result;

        [NativeTypeName("unsigned long (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint> mysql_stmt_param_count;

        [NativeTypeName("my_bool (*)(MYSQL_STMT *, enum enum_stmt_attr_type, const void *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, enum_stmt_attr_type, void*, sbyte> mysql_stmt_attr_set;

        [NativeTypeName("my_bool (*)(MYSQL_STMT *, enum enum_stmt_attr_type, void *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, enum_stmt_attr_type, void*, sbyte> mysql_stmt_attr_get;

        [NativeTypeName("my_bool (*)(MYSQL_STMT *, MYSQL_BIND *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, st_mysql_bind*, sbyte> mysql_stmt_bind_param;

        [NativeTypeName("my_bool (*)(MYSQL_STMT *, MYSQL_BIND *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, st_mysql_bind*, sbyte> mysql_stmt_bind_result;

        [NativeTypeName("my_bool (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, sbyte> mysql_stmt_close;

        [NativeTypeName("my_bool (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, sbyte> mysql_stmt_reset;

        [NativeTypeName("my_bool (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, sbyte> mysql_stmt_free_result;

        [NativeTypeName("my_bool (*)(MYSQL_STMT *, unsigned int, const char *, unsigned long) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint, byte*, uint, sbyte> mysql_stmt_send_long_data;

        [NativeTypeName("MYSQL_RES *(*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, nint*> mysql_stmt_result_metadata;

        [NativeTypeName("MYSQL_RES *(*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, nint*> mysql_stmt_param_metadata;

        [NativeTypeName("unsigned int (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint> mysql_stmt_errno;

        [NativeTypeName("const char *(*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*> mysql_stmt_error;

        [NativeTypeName("const char *(*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*> mysql_stmt_sqlstate;

        [NativeTypeName("MYSQL_ROW_OFFSET (*)(MYSQL_STMT *, MYSQL_ROW_OFFSET) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, st_mysql_rows*, st_mysql_rows*> mysql_stmt_row_seek;

        [NativeTypeName("MYSQL_ROW_OFFSET (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, st_mysql_rows*> mysql_stmt_row_tell;

        [NativeTypeName("void (*)(MYSQL_STMT *, unsigned long long) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, ulong, void> mysql_stmt_data_seek;

        [NativeTypeName("unsigned long long (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, ulong> mysql_stmt_num_rows;

        [NativeTypeName("unsigned long long (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, ulong> mysql_stmt_affected_rows;

        [NativeTypeName("unsigned long long (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, ulong> mysql_stmt_insert_id;

        [NativeTypeName("unsigned int (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, uint> mysql_stmt_field_count;

        [NativeTypeName("int (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, int> mysql_stmt_next_result;

        [NativeTypeName("my_bool (*)(MYSQL_STMT *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, sbyte> mysql_stmt_more_results;

        [NativeTypeName("int (*)(MYSQL_STMT *, const char *, size_t) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, byte*, nuint, int> mariadb_stmt_execute_direct;

        [NativeTypeName("int (*)(MYSQL *) __attribute__((stdcall))")]
        public delegate* unmanaged[Stdcall]<nint, int> mysql_reset_connection;
    }
}
