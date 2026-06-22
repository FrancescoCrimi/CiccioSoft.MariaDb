namespace CiccioSoft.MariaDb.Interop.Native
{
    internal enum mysql_status
    {
        MYSQL_STATUS_READY,
        MYSQL_STATUS_GET_RESULT,
        MYSQL_STATUS_USE_RESULT,
        MYSQL_STATUS_QUERY_SENT,
        MYSQL_STATUS_SENDING_LOAD_DATA,
        MYSQL_STATUS_FETCHING_DATA,
        MYSQL_STATUS_NEXT_RESULT_PENDING,
        MYSQL_STATUS_QUIT_SENT,
        MYSQL_STATUS_STMT_RESULT,
    }
}
