namespace CiccioSoft.MariaDb.Interop.Native
{
    internal unsafe partial struct st_list
    {
        [NativeTypeName("struct st_list *")]
        public st_list* prev;

        [NativeTypeName("struct st_list *")]
        public st_list* next;

        public void* data;
    }
}
