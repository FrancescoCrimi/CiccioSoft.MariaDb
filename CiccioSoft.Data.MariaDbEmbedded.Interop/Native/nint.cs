namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct nint
    {
        [NativeTypeName("char *")]
        public byte* name;

        [NativeTypeName("char *")]
        public byte* org_name;

        [NativeTypeName("char *")]
        public byte* table;

        [NativeTypeName("char *")]
        public byte* org_table;

        [NativeTypeName("char *")]
        public byte* db;

        [NativeTypeName("char *")]
        public byte* catalog;

        [NativeTypeName("char *")]
        public byte* def;

        [NativeTypeName("unsigned long")]
        public uint length;

        [NativeTypeName("unsigned long")]
        public uint max_length;

        [NativeTypeName("unsigned int")]
        public uint name_length;

        [NativeTypeName("unsigned int")]
        public uint org_name_length;

        [NativeTypeName("unsigned int")]
        public uint table_length;

        [NativeTypeName("unsigned int")]
        public uint org_table_length;

        [NativeTypeName("unsigned int")]
        public uint db_length;

        [NativeTypeName("unsigned int")]
        public uint catalog_length;

        [NativeTypeName("unsigned int")]
        public uint def_length;

        [NativeTypeName("unsigned int")]
        public uint flags;

        [NativeTypeName("unsigned int")]
        public uint decimals;

        [NativeTypeName("unsigned int")]
        public uint charsetnr;

        [NativeTypeName("enum enum_field_types")]
        public enum_field_types type;

        public void* extension;
    }

    internal unsafe partial struct nint
    {
        [NativeTypeName("unsigned long long")]
        public ulong row_count;

        [NativeTypeName("unsigned int")]
        public uint field_count;

        [NativeTypeName("unsigned int")]
        public uint current_field;

        [NativeTypeName("MYSQL_FIELD *")]
        public nint* fields;

        [NativeTypeName("MYSQL_DATA *")]
        public st_mysql_data* data;

        [NativeTypeName("MYSQL_ROWS *")]
        public st_mysql_rows* data_cursor;

        [NativeTypeName("MA_MEM_ROOT")]
        public st_ma_mem_root field_alloc;

        [NativeTypeName("MYSQL_ROW")]
        public byte** row;

        [NativeTypeName("MYSQL_ROW")]
        public byte** current_row;

        [NativeTypeName("unsigned long *")]
        public uint* lengths;

        [NativeTypeName("MYSQL *")]
        public nint handle;

        [NativeTypeName("my_bool")]
        public sbyte eof;

        [NativeTypeName("my_bool")]
        public sbyte is_ps;
    }
}
