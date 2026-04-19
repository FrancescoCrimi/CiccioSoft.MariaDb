namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct ma_charset_info_st
    {
        [NativeTypeName("unsigned int")]
        public uint nr;

        [NativeTypeName("unsigned int")]
        public uint state;

        [NativeTypeName("const char *")]
        public byte* csname;

        [NativeTypeName("const char *")]
        public byte* name;

        [NativeTypeName("const char *")]
        public byte* dir;

        [NativeTypeName("unsigned int")]
        public uint codepage;

        [NativeTypeName("const char *")]
        public byte* encoding;

        [NativeTypeName("unsigned int")]
        public uint char_minlen;

        [NativeTypeName("unsigned int")]
        public uint char_maxlen;

        [NativeTypeName("unsigned int (*)(unsigned int)")]
        public delegate* unmanaged[Cdecl]<uint, uint> mb_charlen;

        [NativeTypeName("unsigned int (*)(const char *, const char *)")]
        public delegate* unmanaged[Cdecl]<byte*, byte*, uint> mb_valid;
    }
}
