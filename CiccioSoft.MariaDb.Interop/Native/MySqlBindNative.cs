using System.Runtime.InteropServices;

namespace CiccioSoft.MariaDb.Interop.Native
{
    internal unsafe partial struct MySqlBindNative
    {
        [NativeTypeName("unsigned long *")]
        public uint* length;

        [NativeTypeName("my_bool *")]
        public byte* is_null;

        public void* buffer;

        [NativeTypeName("my_bool *")]
        public byte* error;

        [NativeTypeName("__AnonymousRecord_mariadb_stmt_L118_C3")]
        public _u_e__Union u;

        [NativeTypeName("void (*)(NET *, struct st_mysql_bind *)")]
        public delegate* unmanaged[Cdecl]<st_net*, MySqlBindNative*, void> store_param_func;

        [NativeTypeName("void (*)(struct st_mysql_bind *, MYSQL_FIELD *, unsigned char **)")]
        public delegate* unmanaged[Cdecl]<MySqlBindNative*, MySqlFieldNative*, byte**, void> fetch_result;

        [NativeTypeName("void (*)(struct st_mysql_bind *, MYSQL_FIELD *, unsigned char **)")]
        public delegate* unmanaged[Cdecl]<MySqlBindNative*, MySqlFieldNative*, byte**, void> skip_result;

        [NativeTypeName("unsigned long")]
        public uint buffer_length;

        [NativeTypeName("unsigned long")]
        public uint offset;

        [NativeTypeName("unsigned long")]
        public uint length_value;

        [NativeTypeName("unsigned int")]
        public uint flags;

        [NativeTypeName("unsigned int")]
        public uint pack_length;

        [NativeTypeName("enum enum_field_types")]
        public MySqlFieldTypes buffer_type;

        [NativeTypeName("my_bool")]
        public sbyte error_value;

        [NativeTypeName("my_bool")]
        public sbyte is_unsigned;

        [NativeTypeName("my_bool")]
        public sbyte long_data_used;

        [NativeTypeName("my_bool")]
        public sbyte is_null_value;

        public void* extension;

        [StructLayout(LayoutKind.Explicit)]
        internal unsafe partial struct _u_e__Union
        {
            [FieldOffset(0)]
            [NativeTypeName("unsigned char *")]
            public byte* row_ptr;

            [FieldOffset(0)]
            [NativeTypeName("char *")]
            public byte* indicator;
        }
    }
}
