namespace CiccioSoft.MariaDb.Interop.Native
{
    internal enum mysql_stmt_state
    {
        MYSQL_STMT_INITTED = 0,
        MYSQL_STMT_PREPARED,
        MYSQL_STMT_EXECUTED,
        MYSQL_STMT_WAITING_USE_OR_STORE,
        MYSQL_STMT_USE_OR_STORE_CALLED,
        MYSQL_STMT_USER_FETCHING,
        MYSQL_STMT_FETCH_DONE,
    }
}
