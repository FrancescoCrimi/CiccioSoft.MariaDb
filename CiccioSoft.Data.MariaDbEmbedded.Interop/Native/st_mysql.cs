using System.Runtime.CompilerServices;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct st_mysql
    {
        [NativeTypeName("NET")]
        public st_net net;

        public void* unused_0;

        [NativeTypeName("char *")]
        public byte* host;

        [NativeTypeName("char *")]
        public byte* user;

        [NativeTypeName("char *")]
        public byte* passwd;

        [NativeTypeName("char *")]
        public byte* unix_socket;

        [NativeTypeName("char *")]
        public byte* server_version;

        [NativeTypeName("char *")]
        public byte* host_info;

        [NativeTypeName("char *")]
        public byte* info;

        [NativeTypeName("char *")]
        public byte* db;

        [NativeTypeName("const struct ma_charset_info_st *")]
        public nint charset;

        [NativeTypeName("MYSQL_FIELD *")]
        public nint* fields;

        [NativeTypeName("MA_MEM_ROOT")]
        public st_ma_mem_root field_alloc;

        [NativeTypeName("unsigned long long")]
        public ulong affected_rows;

        [NativeTypeName("unsigned long long")]
        public ulong insert_id;

        [NativeTypeName("unsigned long long")]
        public ulong extra_info;

        [NativeTypeName("unsigned long")]
        public uint thread_id;

        [NativeTypeName("unsigned long")]
        public uint packet_length;

        [NativeTypeName("unsigned int")]
        public uint port;

        [NativeTypeName("unsigned long")]
        public uint client_flag;

        [NativeTypeName("unsigned long")]
        public uint server_capabilities;

        [NativeTypeName("unsigned int")]
        public uint protocol_version;

        [NativeTypeName("unsigned int")]
        public uint field_count;

        [NativeTypeName("unsigned int")]
        public uint server_status;

        [NativeTypeName("unsigned int")]
        public uint server_language;

        [NativeTypeName("unsigned int")]
        public uint warning_count;

        [NativeTypeName("struct st_mysql_options")]
        public st_mysql_options options;

        [NativeTypeName("enum mysql_status")]
        public mysql_status status;

        [NativeTypeName("my_bool")]
        public sbyte free_me;

        [NativeTypeName("my_bool")]
        public sbyte unused_1;

        [NativeTypeName("char[21]")]
        public _scramble_buff_e__FixedBuffer scramble_buff;

        [NativeTypeName("my_bool")]
        public sbyte unused_2;

        public void* unused_3;

        public void* unused_4;

        public void* unused_5;

        public void* unused_6;

        [NativeTypeName("LIST *")]
        public st_list* stmts;

        [NativeTypeName("const struct st_mariadb_methods *")]
        public st_mariadb_methods* methods;

        public void* thd;

        [NativeTypeName("my_bool *")]
        public byte* unbuffered_fetch_owner;

        [NativeTypeName("char *")]
        public byte* info_buffer;

        [NativeTypeName("struct st_mariadb_extension *")]
        public st_mariadb_extension* extension;

        internal partial struct st_mariadb_extension
        {
        }

        [InlineArray(21)]
        public partial struct _scramble_buff_e__FixedBuffer
        {
            public byte e0;
        }
    }
}
