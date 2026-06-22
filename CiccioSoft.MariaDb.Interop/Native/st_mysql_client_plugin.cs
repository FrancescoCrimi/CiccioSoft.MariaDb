using System.Runtime.CompilerServices;

namespace CiccioSoft.MariaDb.Interop.Native
{
    internal unsafe partial struct st_mysql_client_plugin
    {
        public int type;

        [NativeTypeName("unsigned int")]
        public uint interface_version;

        [NativeTypeName("const char *")]
        public byte* name;

        [NativeTypeName("const char *")]
        public byte* author;

        [NativeTypeName("const char *")]
        public byte* desc;

        [NativeTypeName("unsigned int[3]")]
        public _version_e__FixedBuffer version;

        [NativeTypeName("const char *")]
        public byte* license;

        public void* mysql_api;

        [NativeTypeName("int (*)(char *, size_t, int, va_list)")]
        public delegate* unmanaged[Cdecl]<byte*, nuint, int, byte*, int> init;

        [NativeTypeName("int (*)(void)")]
        public delegate* unmanaged[Cdecl]<int> deinit;

        [NativeTypeName("int (*)(const char *, const void *)")]
        public delegate* unmanaged[Cdecl]<byte*, void*, int> options;

        [InlineArray(3)]
        public partial struct _version_e__FixedBuffer
        {
            public uint e0;
        }
    }
}
