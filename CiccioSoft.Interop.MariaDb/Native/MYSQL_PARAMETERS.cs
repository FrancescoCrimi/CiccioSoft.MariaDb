namespace CiccioSoft.Interop.MariaDb.Native
{
    internal unsafe partial struct MYSQL_PARAMETERS
    {
        [NativeTypeName("unsigned long *")]
        public uint* p_max_allowed_packet;

        [NativeTypeName("unsigned long *")]
        public uint* p_net_buffer_length;

        public void* extension;
    }
}
