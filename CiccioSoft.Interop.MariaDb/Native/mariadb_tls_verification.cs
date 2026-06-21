namespace CiccioSoft.Interop.MariaDb.Native
{
    internal enum mariadb_tls_verification
    {
        MARIADB_VERIFY_NONE = 0,
        MARIADB_VERIFY_PIPE,
        MARIADB_VERIFY_UNIXSOCKET,
        MARIADB_VERIFY_LOCALHOST,
        MARIADB_VERIFY_FINGERPRINT,
        MARIADB_VERIFY_PEER_CERT,
    }
}
