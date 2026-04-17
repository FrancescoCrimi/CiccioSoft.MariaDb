namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct st_mariadb_methods
    {
        [NativeTypeName("MYSQL *(*)(MYSQL *, const char *, const char *, const char *, const char *, unsigned int, const char *, unsigned long)")]
        public delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, byte*, uint, byte*, CULong, nint> db_connect;

        [NativeTypeName("void (*)(MYSQL *)")]
        public delegate* unmanaged[Cdecl]<nint, void> db_close;

        [NativeTypeName("int (*)(MYSQL *, enum enum_server_command, const char *, size_t, my_bool, void *)")]
        public delegate* unmanaged[Cdecl]<nint, enum_server_command, byte*, nuint, sbyte, void*, int> db_command;

        [NativeTypeName("void (*)(MYSQL *)")]
        public delegate* unmanaged[Cdecl]<nint, void> db_skip_result;

        [NativeTypeName("int (*)(MYSQL *)")]
        public delegate* unmanaged[Cdecl]<nint, int> db_read_query_result;

        [NativeTypeName("MYSQL_DATA *(*)(MYSQL *, MYSQL_FIELD *, unsigned int)")]
        public delegate* unmanaged[Cdecl]<nint, nint*, uint, st_mysql_data*> db_read_rows;

        [NativeTypeName("int (*)(MYSQL *, unsigned int, MYSQL_ROW, unsigned long *)")]
        public delegate* unmanaged[Cdecl]<nint, uint, byte**, CULong*, int> db_read_one_row;

        [NativeTypeName("my_bool (*)(enum enum_field_types)")]
        public delegate* unmanaged[Cdecl]<enum_field_types, sbyte> db_supported_buffer_type;

        [NativeTypeName("my_bool (*)(MYSQL_STMT *)")]
        public delegate* unmanaged[Cdecl]<nint, sbyte> db_read_prepare_response;

        [NativeTypeName("int (*)(MYSQL *)")]
        public delegate* unmanaged[Cdecl]<nint, int> db_read_stmt_result;

        [NativeTypeName("my_bool (*)(MYSQL_STMT *)")]
        public delegate* unmanaged[Cdecl]<nint, sbyte> db_stmt_get_result_metadata;

        [NativeTypeName("my_bool (*)(MYSQL_STMT *)")]
        public delegate* unmanaged[Cdecl]<nint, sbyte> db_stmt_get_param_metadata;

        [NativeTypeName("int (*)(MYSQL_STMT *)")]
        public delegate* unmanaged[Cdecl]<nint, int> db_stmt_read_all_rows;

        [NativeTypeName("int (*)(MYSQL_STMT *, unsigned char **)")]
        public delegate* unmanaged[Cdecl]<nint, byte**, int> db_stmt_fetch;

        [NativeTypeName("int (*)(MYSQL_STMT *, unsigned char *)")]
        public delegate* unmanaged[Cdecl]<nint, byte*, int> db_stmt_fetch_to_bind;

        [NativeTypeName("void (*)(MYSQL_STMT *)")]
        public delegate* unmanaged[Cdecl]<nint, void> db_stmt_flush_unbuffered;

        [NativeTypeName("void (*)(MYSQL *, unsigned int, const char *, const char *, ...)")]
        public delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void> set_error;

        [NativeTypeName("void (*)(MYSQL *, const char *)")]
        public delegate* unmanaged[Cdecl]<nint, byte*, void> invalidate_stmts;

        [NativeTypeName("struct st_mariadb_api *")]
        public st_mariadb_api* api;

        [NativeTypeName("int (*)(MYSQL_STMT *)")]
        public delegate* unmanaged[Cdecl]<nint, int> db_read_execute_response;

        [NativeTypeName("unsigned char *(*)(MYSQL_STMT *, size_t *, my_bool)")]
        public delegate* unmanaged[Cdecl]<nint, nuint*, sbyte, byte*> db_execute_generate_request;
    }
}
