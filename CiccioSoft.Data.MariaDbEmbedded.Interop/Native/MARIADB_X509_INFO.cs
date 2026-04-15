using System.Runtime.CompilerServices;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal unsafe partial struct MARIADB_X509_INFO
    {
        public int version;

        [NativeTypeName("char *")]
        public byte* issuer;

        [NativeTypeName("char *")]
        public byte* subject;

        [NativeTypeName("char[129]")]
        public _fingerprint_e__FixedBuffer fingerprint;

        [NativeTypeName("struct tm")]
        public tm not_before;

        [NativeTypeName("struct tm")]
        public tm not_after;

        [InlineArray(129)]
        public partial struct _fingerprint_e__FixedBuffer
        {
            public byte e0;
        }
    }
}
