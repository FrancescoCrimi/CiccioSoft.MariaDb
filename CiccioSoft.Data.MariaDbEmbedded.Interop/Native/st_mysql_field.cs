namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct st_mysql_field
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
        public MySqlFieldTypes type;

        public void* extension;
    }
}
