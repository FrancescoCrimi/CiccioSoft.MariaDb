using System;

namespace CiccioSoft.Interop.MariaDb.Native
{
    internal static partial class MariadbVersionNative
    {
        [NativeTypeName("#define PROTOCOL_VERSION 10")]
        public const int PROTOCOL_VERSION = 10;

        [NativeTypeName("#define MARIADB_CLIENT_VERSION_STR \"10.8.8\"")]
        public static ReadOnlySpan<byte> MARIADB_CLIENT_VERSION_STR => "10.8.8"u8;

        [NativeTypeName("#define MARIADB_BASE_VERSION \"mariadb-10.8\"")]
        public static ReadOnlySpan<byte> MARIADB_BASE_VERSION => "mariadb-10.8"u8;

        [NativeTypeName("#define MARIADB_VERSION_ID 100808")]
        public const int MARIADB_VERSION_ID = 100808;

        [NativeTypeName("#define MARIADB_PORT 3306")]
        public const int MARIADB_PORT = 3306;

        [NativeTypeName("#define MARIADB_UNIX_ADDR \"/tmp/mysql.sock\"")]
        public static ReadOnlySpan<byte> MARIADB_UNIX_ADDR => "/tmp/mysql.sock"u8;

        [NativeTypeName("#define MYSQL_UNIX_ADDR MARIADB_UNIX_ADDR")]
        public static ReadOnlySpan<byte> MYSQL_UNIX_ADDR => "/tmp/mysql.sock"u8;

        [NativeTypeName("#define MYSQL_PORT MARIADB_PORT")]
        public const int MYSQL_PORT = 3306;

        [NativeTypeName("#define MYSQL_CONFIG_NAME \"my\"")]
        public static ReadOnlySpan<byte> MYSQL_CONFIG_NAME => "my"u8;

        [NativeTypeName("#define MYSQL_VERSION_ID 100808")]
        public const int MYSQL_VERSION_ID = 100808;

        [NativeTypeName("#define MYSQL_SERVER_VERSION \"10.8.8-MariaDB\"")]
        public static ReadOnlySpan<byte> MYSQL_SERVER_VERSION => "10.8.8-MariaDB"u8;

        [NativeTypeName("#define MARIADB_PACKAGE_VERSION \"3.4.8\"")]
        public static ReadOnlySpan<byte> MARIADB_PACKAGE_VERSION => "3.4.8"u8;

        [NativeTypeName("#define MARIADB_PACKAGE_VERSION_ID 30408")]
        public const int MARIADB_PACKAGE_VERSION_ID = 30408;

        [NativeTypeName("#define MARIADB_SYSTEM_TYPE \"Windows\"")]
        public static ReadOnlySpan<byte> MARIADB_SYSTEM_TYPE => "Windows"u8;

        [NativeTypeName("#define MARIADB_MACHINE_TYPE \"AMD64\"")]
        public static ReadOnlySpan<byte> MARIADB_MACHINE_TYPE => "AMD64"u8;

        [NativeTypeName("#define MARIADB_PLUGINDIR \"C:/Program Files/mariadb-connector-c/lib/mariadb/plugin\"")]
        public static ReadOnlySpan<byte> MARIADB_PLUGINDIR => "C:/Program Files/mariadb-connector-c/lib/mariadb/plugin"u8;

        [NativeTypeName("#define MYSQL_CHARSET \"\"")]
        public static ReadOnlySpan<byte> MYSQL_CHARSET => ""u8;

        [NativeTypeName("#define CC_SOURCE_REVISION \"46880b003653a000e9588bd73c8b1dd65088c686\"")]
        public static ReadOnlySpan<byte> CC_SOURCE_REVISION => "46880b003653a000e9588bd73c8b1dd65088c686"u8;
    }
}
