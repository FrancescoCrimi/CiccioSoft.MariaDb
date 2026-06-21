using System.Runtime.CompilerServices;

namespace CiccioSoft.Interop.MariaDb.Native
{
    internal partial struct st_mysql_error_info
    {
        [NativeTypeName("unsigned int")]
        public uint error_no;

        [NativeTypeName("char[513]")]
        public _error_e__FixedBuffer error;

        [NativeTypeName("char[6]")]
        public _sqlstate_e__FixedBuffer sqlstate;

        [InlineArray(513)]
        public partial struct _error_e__FixedBuffer
        {
            public byte e0;
        }

        [InlineArray(6)]
        public partial struct _sqlstate_e__FixedBuffer
        {
            public byte e0;
        }
    }
}
