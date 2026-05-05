# CiccioSoft.Data.MariaDbEmbedded.Interop

![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET Version](https://img.shields.io/badge/.NET-10.0-purple.svg)
![Language](https://img.shields.io/badge/language-C%23-brightgreen.svg)

Low-level library exposing an idiomatic, OOP wrapper for MariaDB Connector/C.

## Current scope

This project introduces the first building blocks for a native MariaDB Embedded interop package:

- native binding surface (`NativeMySqlClient`) for core C API entry points
- low-level managed wrapper (`MySql`) around `MYSQL*`
- custom exception (`MySqlInteropException`) for native failures

## Example

```csharp
using var client = MySql.Init().Connect("127.0.0.1", 3306, "root", "secret", "mydb");
client.Ping();
```

## Notes

- This is an initial scaffold and intentionally minimal.
- Runtime requires a compatible `libmysqlclient` (or equivalent MariaDB client library) on the host system.
