namespace CiccioSoft.MariaDb.Interop.Native
{
    internal unsafe partial struct st_ma_used_mem
    {
        [NativeTypeName("struct st_ma_used_mem *")]
        public st_ma_used_mem* next;

        [NativeTypeName("size_t")]
        public nuint left;

        [NativeTypeName("size_t")]
        public nuint size;
    }
}
