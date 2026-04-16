// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.IA
{
    /// <summary>
    /// Identifies options that can be configured on a native <c>MYSQL*</c> handle
    /// through the <c>mysql_options</c> API.
    /// </summary>
    public enum MySqlOption
    {
        // /// <summary>
        // /// Configures the connection timeout in seconds.
        // /// </summary>
        // ConnectTimeout = 0,

        // /// <summary>
        // /// Enables or disables automatic reconnect logic.
        // /// </summary>
        // Reconnect = 20,

        // /// <summary>
        // /// Configures the timeout used for socket reads in seconds.
        // /// </summary>
        // ReadTimeout = 11,

        // /// <summary>
        // /// Configures the timeout used for socket writes in seconds.
        // /// </summary>
        // WriteTimeout = 12,

        // /// <summary>
        // /// Selects the default character set used by the connection.
        // /// </summary>
        // SetCharsetName = 7,

        MYSQL_OPT_CONNECT_TIMEOUT,
        MYSQL_OPT_COMPRESS,
        MYSQL_OPT_NAMED_PIPE,
        MYSQL_INIT_COMMAND,
        MYSQL_READ_DEFAULT_FILE,
        MYSQL_READ_DEFAULT_GROUP,
        MYSQL_SET_CHARSET_DIR,
        MYSQL_SET_CHARSET_NAME,
        MYSQL_OPT_LOCAL_INFILE,
        MYSQL_OPT_PROTOCOL,
        MYSQL_SHARED_MEMORY_BASE_NAME,
        MYSQL_OPT_READ_TIMEOUT,
        MYSQL_OPT_WRITE_TIMEOUT,
        MYSQL_OPT_USE_RESULT,
        MYSQL_OPT_USE_REMOTE_CONNECTION,
        MYSQL_OPT_USE_EMBEDDED_CONNECTION,
        MYSQL_OPT_GUESS_CONNECTION,
        MYSQL_SET_CLIENT_IP,
        MYSQL_SECURE_AUTH,
        MYSQL_REPORT_DATA_TRUNCATION,
        MYSQL_OPT_RECONNECT,
        MYSQL_OPT_SSL_VERIFY_SERVER_CERT,
        MYSQL_PLUGIN_DIR,
        MYSQL_DEFAULT_AUTH,
        MYSQL_OPT_BIND,
        MYSQL_OPT_SSL_KEY,
        MYSQL_OPT_SSL_CERT,
        MYSQL_OPT_SSL_CA,
        MYSQL_OPT_SSL_CAPATH,
        MYSQL_OPT_SSL_CIPHER,
        MYSQL_OPT_SSL_CRL,
        MYSQL_OPT_SSL_CRLPATH,
        MYSQL_OPT_CONNECT_ATTR_RESET,
        MYSQL_OPT_CONNECT_ATTR_ADD,
        MYSQL_OPT_CONNECT_ATTR_DELETE,
        MYSQL_SERVER_PUBLIC_KEY,
        MYSQL_ENABLE_CLEARTEXT_PLUGIN,
        MYSQL_OPT_CAN_HANDLE_EXPIRED_PASSWORDS,
        MYSQL_OPT_SSL_ENFORCE,
        MYSQL_OPT_MAX_ALLOWED_PACKET,
        MYSQL_OPT_NET_BUFFER_LENGTH,
        MYSQL_OPT_TLS_VERSION,
        MYSQL_OPT_ZSTD_COMPRESSION_LEVEL,
        MYSQL_PROGRESS_CALLBACK = 5999,
        MYSQL_OPT_NONBLOCK,
        MYSQL_DATABASE_DRIVER = 7000,
        MARIADB_OPT_SSL_FP,
        MARIADB_OPT_SSL_FP_LIST,
        MARIADB_OPT_TLS_PASSPHRASE,
        MARIADB_OPT_TLS_CIPHER_STRENGTH,
        MARIADB_OPT_TLS_VERSION,
        MARIADB_OPT_TLS_PEER_FP,
        MARIADB_OPT_TLS_PEER_FP_LIST,
        MARIADB_OPT_CONNECTION_READ_ONLY,
        MYSQL_OPT_CONNECT_ATTRS,
        MARIADB_OPT_USERDATA,
        MARIADB_OPT_CONNECTION_HANDLER,
        MARIADB_OPT_PORT,
        MARIADB_OPT_UNIXSOCKET,
        MARIADB_OPT_PASSWORD,
        MARIADB_OPT_HOST,
        MARIADB_OPT_USER,
        MARIADB_OPT_SCHEMA,
        MARIADB_OPT_DEBUG,
        MARIADB_OPT_FOUND_ROWS,
        MARIADB_OPT_MULTI_RESULTS,
        MARIADB_OPT_MULTI_STATEMENTS,
        MARIADB_OPT_INTERACTIVE,
        MARIADB_OPT_PROXY_HEADER,
        MARIADB_OPT_IO_WAIT,
        MARIADB_OPT_SKIP_READ_RESPONSE,
        MARIADB_OPT_RESTRICTED_AUTH,
        MARIADB_OPT_RPL_REGISTER_REPLICA,
        MARIADB_OPT_STATUS_CALLBACK,
        MARIADB_OPT_SERVER_PLUGINS,
        MARIADB_OPT_BULK_UNIT_RESULTS,
        MARIADB_OPT_TLS_VERIFICATION_CALLBACK,
    }
}
