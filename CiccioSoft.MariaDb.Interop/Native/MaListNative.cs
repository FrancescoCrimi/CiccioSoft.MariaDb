using System.Runtime.InteropServices;

namespace CiccioSoft.MariaDb.Interop.Native
{
    internal static unsafe partial class MaListNative
    {
        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("LIST *")]
        public static extern st_list* list_add([NativeTypeName("LIST *")] st_list* root, [NativeTypeName("LIST *")] st_list* element);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("LIST *")]
        public static extern st_list* list_delete([NativeTypeName("LIST *")] st_list* root, [NativeTypeName("LIST *")] st_list* element);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("LIST *")]
        public static extern st_list* list_cons(void* data, [NativeTypeName("LIST *")] st_list* root);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("LIST *")]
        public static extern st_list* list_reverse([NativeTypeName("LIST *")] st_list* root);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void list_free([NativeTypeName("LIST *")] st_list* root, [NativeTypeName("unsigned int")] uint free_data);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("unsigned int")]
        public static extern uint list_length([NativeTypeName("LIST *")] st_list* list);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int list_walk([NativeTypeName("LIST *")] st_list* list, [NativeTypeName("list_walk_action")] delegate* unmanaged[Cdecl]<void*, void*, int> action, [NativeTypeName("char *")] byte* argument);
    }
}
