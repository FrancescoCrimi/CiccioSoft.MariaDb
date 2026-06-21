namespace CiccioSoft.Interop.MariaDb.Native
{
    internal unsafe partial struct st_mysql_cmd_buffer
    {
        [NativeTypeName("unsigned char *")]
        public byte* buffer;

        [NativeTypeName("size_t")]
        public nuint length;
    }
}
