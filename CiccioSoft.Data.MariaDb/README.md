# CiccioSoft.Data.MariaDb

![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET Version](https://img.shields.io/badge/.NET-10.0-purple.svg)
![Language](https://img.shields.io/badge/language-C%23-brightgreen.svg)

ADO.NET provider for MariaDB Embedded, built on top of the CiccioSoft.Data.MariaDbEmbedded.Interop layer.

## Project scope

This project provides the high-level ADO.NET provider for MariaDB Embedded, offering a familiar .NET database experience similar to other embedded providers like SQLite.

- Implements standard ADO.NET interfaces (DbConnection, DbCommand, etc.)
- Uses the CiccioSoft.Data.MariaDbEmbedded.Interop layer for native operations
- Provides connection pooling, transaction support, and data reader functionality
- Designed as an async-first provider with full async entrypoints

## Architecture

This provider is built on top of `CiccioSoft.Data.MariaDbEmbedded.Interop`, which handles the low-level P/Invoke interactions with MariaDB Connector/C. The provider translates .NET ADO.NET calls into native MariaDB operations.

## Getting started

1. Ensure MariaDB Embedded libraries are available on the system.
2. Reference this package in your .NET project.
3. Use standard ADO.NET patterns:

```csharp
using var connection = new MariaDbEmbeddedConnection("Server=localhost;Database=mydb;User=root;Password=secret;");
await connection.OpenAsync();
// Execute queries asynchronously, etc.
```

## Notes

- This is an early implementation and may not support all ADO.NET features yet.
- Requires compatible MariaDB Embedded libraries at runtime.
- Designed for embedded scenarios where MariaDB runs in-process.