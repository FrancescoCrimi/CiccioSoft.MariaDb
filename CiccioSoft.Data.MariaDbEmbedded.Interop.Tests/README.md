# CiccioSoft.Data.MariaDbEmbedded.Interop.Tests

This project contains automated tests for the MariaDB Connector/C interop layer.

## Purpose

- Validate the behavior of the `CiccioSoft.Data.MariaDbEmbedded.Interop` wrapper.
- Test native API support, exception handling, and result operations.
- Ensure the interop layer maintains predictable behavior.

## Running the tests

```bash
dotnet test CiccioSoft.Data.MariaDbEmbedded.Interop.Tests/CiccioSoft.Data.MariaDbEmbedded.Interop.Tests.csproj
```

## Notes

- This project uses xUnit as the test framework.
- It depends on `CiccioSoft.Data.MariaDbEmbedded.Interop` as a referenced project.
