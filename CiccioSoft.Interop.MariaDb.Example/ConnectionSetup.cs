// Copyright (c) 2026 Francesco Crimi
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using CiccioSoft.Interop.MariaDb.Native;

namespace CiccioSoft.Interop.MariaDb.Example;

internal static class ConnectionSetup
{
    internal static MySql OpenDefaultConnection()
    {
        MySql mysql = MySql.Init();
        mysql.SetOption(MySqlOption.MYSQL_OPT_SSL_VERIFY_SERVER_CERT, false);
        mysql.SetOption(MySqlOption.MARIADB_OPT_MULTI_STATEMENTS, true);
        mysql.Connect("localhost", 3306, "root", "password", "test");

        ConsoleOutput.Section("Connessione aperta");
        ConsoleOutput.KeyValue("Server", mysql.GetServerInfo());
        ConsoleOutput.KeyValue("Client", MySql.GetClientInfo());
        ConsoleOutput.KeyValue("Host", mysql.GetHostInfo());
        ConsoleOutput.KeyValue("Thread", mysql.ThreadId());
        ConsoleOutput.KeyValue("Proto", mysql.GetProtoInfo());

        return mysql;
    }
}
