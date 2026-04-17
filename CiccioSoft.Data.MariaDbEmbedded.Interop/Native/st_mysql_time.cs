namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal partial struct st_mysql_time
    {
        [NativeTypeName("unsigned int")]
        public uint year;

        [NativeTypeName("unsigned int")]
        public uint month;

        [NativeTypeName("unsigned int")]
        public uint day;

        [NativeTypeName("unsigned int")]
        public uint hour;

        [NativeTypeName("unsigned int")]
        public uint minute;

        [NativeTypeName("unsigned int")]
        public uint second;

        [NativeTypeName("unsigned long")]
        public CULong second_part;

        [NativeTypeName("my_bool")]
        public sbyte neg;

        [NativeTypeName("enum enum_mysql_timestamp_type")]
        public enum_mysql_timestamp_type time_type;
    }
}
