using System.Runtime.CompilerServices;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct st_mysql_stmt
    {
        [NativeTypeName("MA_MEM_ROOT")]
        public st_ma_mem_root mem_root;

        [NativeTypeName("MYSQL *")]
        public nint mysql;

        [NativeTypeName("unsigned long")]
        public uint stmt_id;

        [NativeTypeName("unsigned long")]
        public uint flags;

        [NativeTypeName("enum_mysqlnd_stmt_state")]
        public mysql_stmt_state state;

        [NativeTypeName("MYSQL_FIELD *")]
        public nint* fields;

        [NativeTypeName("unsigned int")]
        public uint field_count;

        [NativeTypeName("unsigned int")]
        public uint param_count;

        [NativeTypeName("unsigned char")]
        public byte send_types_to_server;

        [NativeTypeName("MYSQL_BIND *")]
        public st_mysql_bind* @params;

        [NativeTypeName("MYSQL_BIND *")]
        public st_mysql_bind* bind;

        [NativeTypeName("MYSQL_DATA")]
        public st_mysql_data result;

        [NativeTypeName("MYSQL_ROWS *")]
        public st_mysql_rows* result_cursor;

        [NativeTypeName("my_bool")]
        public sbyte bind_result_done;

        [NativeTypeName("my_bool")]
        public sbyte bind_param_done;

        [NativeTypeName("mysql_upsert_status")]
        public st_mysqlnd_upsert_result upsert_status;

        [NativeTypeName("unsigned int")]
        public uint last_errno;

        [NativeTypeName("char[513]")]
        public _last_error_e__FixedBuffer last_error;

        [NativeTypeName("char[6]")]
        public _sqlstate_e__FixedBuffer sqlstate;

        [NativeTypeName("my_bool")]
        public sbyte update_max_length;

        [NativeTypeName("unsigned long")]
        public uint prefetch_rows;

        [NativeTypeName("LIST")]
        public st_list list;

        [NativeTypeName("my_bool")]
        public sbyte cursor_exists;

        public void* extension;

        [NativeTypeName("mysql_stmt_fetch_row_func")]
        public delegate* unmanaged[Cdecl]<nint, byte**, int> fetch_row_func;

        [NativeTypeName("unsigned int")]
        public uint execute_count;

        [NativeTypeName("mysql_stmt_use_or_store_func")]
        public delegate* unmanaged[Cdecl]<nint, nint*> default_rset_handler;

        [NativeTypeName("unsigned char *")]
        public byte* request_buffer;

        [NativeTypeName("unsigned int")]
        public uint array_size;

        [NativeTypeName("size_t")]
        public nuint row_size;

        [NativeTypeName("unsigned int")]
        public uint prebind_params;

        public void* user_data;

        [NativeTypeName("ps_result_callback")]
        public delegate* unmanaged[Cdecl]<void*, uint, byte**, void> result_callback;

        [NativeTypeName("ps_param_callback")]
        public delegate* unmanaged[Cdecl]<void*, st_mysql_bind*, uint, sbyte> param_callback;

        [NativeTypeName("size_t")]
        public nuint request_length;

        [NativeTypeName("MARIADB_CONST_STRING")]
        public st_ma_const_string sql;

        [InlineArray(513)]
        public partial struct _last_error_e__FixedBuffer
        {
            public byte e0;
        }

        [InlineArray(6)]
        public partial struct _sqlstate_e__FixedBuffer
        {
            public byte e0;
        }
    }
}
