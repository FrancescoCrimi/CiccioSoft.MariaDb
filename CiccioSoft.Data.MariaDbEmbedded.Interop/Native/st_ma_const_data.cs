namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct st_ma_const_data
    {
        [NativeTypeName("const unsigned char *")]
        public byte* data;

        [NativeTypeName("size_t")]
        public nuint length;
    }
}
