namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct st_mysql_options
    {
        [NativeTypeName("unsigned int")]
        public uint connect_timeout;

        [NativeTypeName("unsigned int")]
        public uint read_timeout;

        [NativeTypeName("unsigned int")]
        public uint write_timeout;

        [NativeTypeName("unsigned int")]
        public uint port;

        [NativeTypeName("unsigned int")]
        public uint protocol;

        [NativeTypeName("unsigned long")]
        public CULong client_flag;

        [NativeTypeName("char *")]
        public byte* host;

        [NativeTypeName("char *")]
        public byte* user;

        [NativeTypeName("char *")]
        public byte* password;

        [NativeTypeName("char *")]
        public byte* unix_socket;

        [NativeTypeName("char *")]
        public byte* db;

        [NativeTypeName("struct st_dynamic_array *")]
        public st_dynamic_array* init_command;

        [NativeTypeName("char *")]
        public byte* my_cnf_file;

        [NativeTypeName("char *")]
        public byte* my_cnf_group;

        [NativeTypeName("char *")]
        public byte* charset_dir;

        [NativeTypeName("char *")]
        public byte* charset_name;

        [NativeTypeName("char *")]
        public byte* ssl_key;

        [NativeTypeName("char *")]
        public byte* ssl_cert;

        [NativeTypeName("char *")]
        public byte* ssl_ca;

        [NativeTypeName("char *")]
        public byte* ssl_capath;

        [NativeTypeName("char *")]
        public byte* ssl_cipher;

        [NativeTypeName("char *")]
        public byte* shared_memory_base_name;

        [NativeTypeName("unsigned long")]
        public CULong max_allowed_packet;

        [NativeTypeName("my_bool")]
        public sbyte use_ssl;

        [NativeTypeName("my_bool")]
        public sbyte compress;

        [NativeTypeName("my_bool")]
        public sbyte named_pipe;

        [NativeTypeName("my_bool")]
        public sbyte reconnect;

        [NativeTypeName("my_bool")]
        public sbyte unused_1;

        [NativeTypeName("my_bool")]
        public sbyte unused_2;

        [NativeTypeName("my_bool")]
        public sbyte unused_3;

        [NativeTypeName("enum mysql_option")]
        public MySqlOption methods_to_use;

        [NativeTypeName("char *")]
        public byte* bind_address;

        [NativeTypeName("my_bool")]
        public sbyte secure_auth;

        [NativeTypeName("my_bool")]
        public sbyte report_data_truncation;

        [NativeTypeName("int (*)(void **, const char *, void *)")]
        public delegate* unmanaged[Cdecl]<void**, byte*, void*, int> local_infile_init;

        [NativeTypeName("int (*)(void *, char *, unsigned int)")]
        public delegate* unmanaged[Cdecl]<void*, byte*, uint, int> local_infile_read;

        [NativeTypeName("void (*)(void *)")]
        public delegate* unmanaged[Cdecl]<void*, void> local_infile_end;

        [NativeTypeName("int (*)(void *, char *, unsigned int)")]
        public delegate* unmanaged[Cdecl]<void*, byte*, uint, int> local_infile_error;

        public void* local_infile_userdata;

        [NativeTypeName("struct st_mysql_options_extension *")]
        public st_mysql_options_extension* extension;

        internal partial struct st_dynamic_array
        {
        }

        internal partial struct st_mysql_options_extension
        {
        }
    }
}
