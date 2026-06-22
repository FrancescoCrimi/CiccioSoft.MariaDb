# CiccioSoft.Data.MariaDb.Tests

This project contains automated tests for the MariaDB Embedded ADO.NET provider.

## Purpose

- Validate the behavior of the provider classes (`MariaDbConnection`, `MariaDbCommand`, `MariaDbDataReader`, etc.).
- Test connection, transaction, and data reading scenarios.
- Ensure functional integration between the provider and the interop layer.

## Running the tests

```bash
dotnet test CiccioSoft.Data.MariaDb.Tests/CiccioSoft.Data.MariaDb.Tests.csproj
```

## Notes

- This project uses xUnit as the test framework.
- It is intended to verify provider behavior, not direct native interop.
