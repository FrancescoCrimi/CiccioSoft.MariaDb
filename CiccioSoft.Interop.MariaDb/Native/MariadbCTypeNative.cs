using System;
using System.Runtime.InteropServices;

namespace CiccioSoft.Interop.MariaDb.Native
{
    internal static unsafe partial class MariadbCTypeNative
    {
        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("MARIADB_CHARSET_INFO *")]
        public static extern nint find_compiled_charset([NativeTypeName("unsigned int")] uint cs_number);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("MARIADB_CHARSET_INFO *")]
        public static extern nint find_compiled_charset_by_name([NativeTypeName("const char *")] byte* name);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("size_t")]
        public static extern nuint mysql_cset_escape_quotes([NativeTypeName("const MARIADB_CHARSET_INFO *")] nint cset, [NativeTypeName("char *")] byte* newstr, [NativeTypeName("const char *")] byte* escapestr, [NativeTypeName("size_t")] nuint escapestr_len);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("size_t")]
        public static extern nuint mysql_cset_escape_slashes([NativeTypeName("const MARIADB_CHARSET_INFO *")] nint cset, [NativeTypeName("char *")] byte* newstr, [NativeTypeName("const char *")] byte* escapestr, [NativeTypeName("size_t")] nuint escapestr_len);

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern byte* madb_get_os_character_set();

        [DllImport("libmariadb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int madb_get_windows_cp([NativeTypeName("const char *")] byte* charset);

        [NativeTypeName("#define CHARSET_DIR \"charsets/\"")]
        public static ReadOnlySpan<byte> CHARSET_DIR => "charsets/"u8;

        [NativeTypeName("#define MY_CS_NAME_SIZE 32")]
        public const int MY_CS_NAME_SIZE = 32;

        [NativeTypeName("#define MADB_DEFAULT_CHARSET_NAME \"latin1\"")]
        public static ReadOnlySpan<byte> MADB_DEFAULT_CHARSET_NAME => "latin1"u8;

        [NativeTypeName("#define MADB_DEFAULT_COLLATION_NAME \"latin1_swedish_ci\"")]
        public static ReadOnlySpan<byte> MADB_DEFAULT_COLLATION_NAME => "latin1_swedish_ci"u8;

        [NativeTypeName("#define MADB_AUTODETECT_CHARSET_NAME \"auto\"")]
        public static ReadOnlySpan<byte> MADB_AUTODETECT_CHARSET_NAME => "auto"u8;
    }
}
