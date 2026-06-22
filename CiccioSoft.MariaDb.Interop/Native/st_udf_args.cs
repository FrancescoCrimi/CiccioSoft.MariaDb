namespace CiccioSoft.MariaDb.Interop.Native
{
    internal unsafe partial struct st_udf_args
    {
        [NativeTypeName("unsigned int")]
        public uint arg_count;

        [NativeTypeName("enum Item_result *")]
        public Item_result* arg_type;

        [NativeTypeName("char **")]
        public byte** args;

        [NativeTypeName("unsigned long *")]
        public uint* lengths;

        [NativeTypeName("char *")]
        public byte* maybe_null;
    }
}
