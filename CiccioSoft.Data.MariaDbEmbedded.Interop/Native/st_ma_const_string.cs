namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct st_ma_const_string
    {
        [NativeTypeName("const char *")]
        public byte* str;

        [NativeTypeName("size_t")]
        public nuint length;
    }
}
