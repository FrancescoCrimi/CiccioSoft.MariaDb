# CiccioSoft.Data.MariaDbEmbedded Guidelines

This document captures development guidelines for the `CiccioSoft.Data.MariaDbEmbedded` provider project.

## 1. Async-first provider

- The provider must be designed as an async-first implementation.
- Public APIs should expose asynchronous entrypoints for I/O operations.
- Use `OpenAsync`, `ExecuteReaderAsync`, `ExecuteNonQueryAsync`, `ExecuteScalarAsync`, `ReadAsync`, and other async patterns instead of blocking calls.
- Ensure that the async APIs are implemented correctly and do not simply wrap blocking calls on the thread pool.

## 2. ADO.NET compliance

- The provider should implement the standard ADO.NET base classes:
  - `DbConnection`
  - `DbCommand`
  - `DbDataReader`
  - `DbParameter`
  - `DbParameterCollection`
  - `DbTransaction`
  - `DbProviderFactory`
- Keep the semantic behavior consistent with other .NET providers.

## 3. Separation of concerns

- High-level provider logic lives in `CiccioSoft.Data.MariaDbEmbedded`.
- Native interop details live in `CiccioSoft.Data.MariaDbEmbedded.Interop`.
- The provider should depend on the interop layer, not on native APIs directly.

## 4. Naming and idiomatic design

- Use clear and concise C# names that reflect the underlying MariaDB objects and ADO.NET concepts.
- Prefer `MariaDbEmbeddedConnection`, `MariaDbEmbeddedCommand`, `MariaDbEmbeddedDataReader`, etc.

## 5. Future additions

- Add specific guidance for connection string formats, transaction support, and parameter handling as development proceeds.
- Document any provider-specific behaviors or deviations from standard ADO.NET patterns.
