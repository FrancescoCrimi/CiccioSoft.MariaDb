namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Native
{
    internal partial struct st_mysqlnd_upsert_result
    {
        [NativeTypeName("unsigned int")]
        public uint warning_count;

        [NativeTypeName("unsigned int")]
        public uint server_status;

        [NativeTypeName("unsigned long long")]
        public ulong affected_rows;

        [NativeTypeName("unsigned long long")]
        public ulong last_insert_id;
    }
}
