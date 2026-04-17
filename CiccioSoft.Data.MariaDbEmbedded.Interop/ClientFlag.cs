namespace CiccioSoft.Data.MariaDbEmbedded.Interop;

public enum ClientFlag : ulong
{
    CLIENT_MYSQL = 1,
    CLIENT_FOUND_ROWS = 2,  /* Found instead of affected rows */
    CLIENT_LONG_FLAG = 4,   /* Get all column flags */
    CLIENT_CONNECT_WITH_DB = 8, /* One can specify db on connect */
    CLIENT_NO_SCHEMA = 16,  /* Don't allow database.table.column */
    CLIENT_COMPRESS = 32,   /* Can use compression protocol */
    CLIENT_ODBC = 64,   /* Odbc client */
    CLIENT_LOCAL_FILES = 128,   /* Can use LOAD DATA LOCAL */
    CLIENT_IGNORE_SPACE = 256,  /* Ignore spaces before '(' */
    CLIENT_INTERACTIVE = 1024,  /* This is an interactive client */
    CLIENT_SSL = 2048,     /* Switch to SSL after handshake */
    CLIENT_IGNORE_SIGPIPE = 4096,    /* IGNORE sigpipes */
    CLIENT_TRANSACTIONS = 8192, /* Client knows about transactions */
    /* added in 4.x */
    CLIENT_PROTOCOL_41 = 512,
    CLIENT_RESERVED = 16384,
    CLIENT_SECURE_CONNECTION = 32768,
    CLIENT_MULTI_STATEMENTS = (1UL << 16),
    CLIENT_MULTI_RESULTS = (1UL << 17),
    CLIENT_PS_MULTI_RESULTS = (1UL << 18),
    CLIENT_PLUGIN_AUTH = (1UL << 19),
    CLIENT_CONNECT_ATTRS = (1UL << 20),
    CLIENT_PLUGIN_AUTH_LENENC_CLIENT_DATA = (1UL << 21),
    CLIENT_CAN_HANDLE_EXPIRED_PASSWORDS = (1UL << 22),
    CLIENT_SESSION_TRACKING = (1UL << 23),
    CLIENT_ZSTD_COMPRESSION = (1UL << 26),
    CLIENT_PROGRESS = (1UL << 29), /* client supports progress indicator */
    CLIENT_PROGRESS_OBSOLETE = CLIENT_PROGRESS,
    CLIENT_SSL_VERIFY_SERVER_CERT = (1UL << 30),
    CLIENT_SSL_VERIFY_SERVER_CERT_OBSOLETE = CLIENT_SSL_VERIFY_SERVER_CERT,
    CLIENT_REMEMBER_OPTIONS = (1UL << 31),
}
