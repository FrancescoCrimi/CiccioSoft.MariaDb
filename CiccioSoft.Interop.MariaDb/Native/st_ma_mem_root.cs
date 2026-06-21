namespace CiccioSoft.Interop.MariaDb.Native
{
    internal unsafe partial struct st_ma_mem_root
    {
        [NativeTypeName("MA_USED_MEM *")]
        public st_ma_used_mem* free;

        [NativeTypeName("MA_USED_MEM *")]
        public st_ma_used_mem* used;

        [NativeTypeName("MA_USED_MEM *")]
        public st_ma_used_mem* pre_alloc;

        [NativeTypeName("size_t")]
        public nuint min_malloc;

        [NativeTypeName("size_t")]
        public nuint block_size;

        [NativeTypeName("unsigned int")]
        public uint block_num;

        [NativeTypeName("unsigned int")]
        public uint first_block_usage;

        [NativeTypeName("void (*)(void)")]
        public delegate* unmanaged[Cdecl]<void> error_handler;
    }
}
