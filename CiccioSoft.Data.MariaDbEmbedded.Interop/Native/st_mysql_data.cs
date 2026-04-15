namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct st_mysql_data
    {
        [NativeTypeName("MYSQL_ROWS *")]
        public st_mysql_rows* data;

        public void* embedded_info;

        [NativeTypeName("MA_MEM_ROOT")]
        public st_ma_mem_root alloc;

        [NativeTypeName("unsigned long long")]
        public ulong rows;

        [NativeTypeName("unsigned int")]
        public uint fields;

        public void* extension;
    }
}
