namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal partial struct rand_struct
    {
        [NativeTypeName("unsigned long")]
        public uint seed1;

        [NativeTypeName("unsigned long")]
        public uint seed2;

        [NativeTypeName("unsigned long")]
        public uint max_value;

        public double max_value_dbl;
    }
}
