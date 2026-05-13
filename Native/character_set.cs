namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct character_set
    {
        [NativeTypeName("unsigned int")]
        public uint number;

        [NativeTypeName("unsigned int")]
        public uint state;

        [NativeTypeName("const char *")]
        public byte* csname;

        [NativeTypeName("const char *")]
        public byte* name;

        [NativeTypeName("const char *")]
        public byte* comment;

        [NativeTypeName("const char *")]
        public byte* dir;

        [NativeTypeName("unsigned int")]
        public uint mbminlen;

        [NativeTypeName("unsigned int")]
        public uint mbmaxlen;
    }
}
