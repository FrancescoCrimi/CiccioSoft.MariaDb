using System.Runtime.CompilerServices;

namespace CiccioSoft.Interop.MariaDb.Native
{
    internal unsafe partial struct st_net
    {
        [NativeTypeName("MARIADB_PVIO *")]
        public st_ma_pvio* pvio;

        [NativeTypeName("unsigned char *")]
        public byte* buff;

        [NativeTypeName("unsigned char *")]
        public byte* buff_end;

        [NativeTypeName("unsigned char *")]
        public byte* write_pos;

        [NativeTypeName("unsigned char *")]
        public byte* read_pos;

        [NativeTypeName("unsigned long long")]
        public ulong fd;

        [NativeTypeName("unsigned long")]
        public uint remain_in_buf;

        [NativeTypeName("unsigned long")]
        public uint length;

        [NativeTypeName("unsigned long")]
        public uint buf_length;

        [NativeTypeName("unsigned long")]
        public uint where_b;

        [NativeTypeName("unsigned long")]
        public uint max_packet;

        [NativeTypeName("unsigned long")]
        public uint max_packet_size;

        [NativeTypeName("unsigned int")]
        public uint pkt_nr;

        [NativeTypeName("unsigned int")]
        public uint compress_pkt_nr;

        [NativeTypeName("unsigned int")]
        public uint write_timeout;

        [NativeTypeName("unsigned int")]
        public uint read_timeout;

        [NativeTypeName("unsigned int")]
        public uint retry_count;

        public int fcntl;

        [NativeTypeName("unsigned int *")]
        public uint* return_status;

        [NativeTypeName("unsigned char")]
        public byte reading_or_writing;

        [NativeTypeName("char")]
        public byte save_char;

        [NativeTypeName("char")]
        public byte unused_1;

        [NativeTypeName("unsigned char")]
        public byte tls_verify_status;

        [NativeTypeName("my_bool")]
        public sbyte compress;

        [NativeTypeName("my_bool")]
        public sbyte unused_2;

        [NativeTypeName("char *")]
        public byte* unused_3;

        [NativeTypeName("unsigned int")]
        public uint last_errno;

        [NativeTypeName("unsigned char")]
        public byte error;

        [NativeTypeName("my_bool")]
        public sbyte unused_5;

        [NativeTypeName("my_bool")]
        public sbyte unused_6;

        [NativeTypeName("char[512]")]
        public _last_error_e__FixedBuffer last_error;

        [NativeTypeName("char[6]")]
        public _sqlstate_e__FixedBuffer sqlstate;

        [NativeTypeName("struct st_mariadb_net_extension *")]
        public st_mariadb_net_extension* extension;

        internal partial struct st_mariadb_net_extension
        {
        }

        [InlineArray(512)]
        public partial struct _last_error_e__FixedBuffer
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
