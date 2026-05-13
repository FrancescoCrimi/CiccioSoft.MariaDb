namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct st_udf_init
    {
        [NativeTypeName("my_bool")]
        public sbyte maybe_null;

        [NativeTypeName("unsigned int")]
        public uint decimals;

        [NativeTypeName("unsigned int")]
        public uint max_length;

        [NativeTypeName("char *")]
        public byte* ptr;

        [NativeTypeName("my_bool")]
        public sbyte const_item;
    }
}
