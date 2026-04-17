namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct st_mysql_rows
    {
        [NativeTypeName("struct st_mysql_rows *")]
        public st_mysql_rows* next;

        [NativeTypeName("MYSQL_ROW")]
        public byte** data;

        [NativeTypeName("unsigned long")]
        public CULong length;
    }
}
