# CiccioSoft.MariaDb.Interop.Example

This project contains a didactic example that demonstrates how to use the `CiccioSoft.Data.MariaDbEmbedded.Interop` layer.

## Purpose

- Show how to initialize and connect to MariaDB using the `MySql` wrapper.
- Execute example queries and print results to the console.
- Provide a starting point for exploring the interop API.

## Internal structure

To keep the sample maintainable, responsibilities are separated:

- `Program.cs`: entry point and high-level orchestration.
- `ConnectionSetup.cs`: connection initialization and default options.
- `DemoScenarios.cs`: SQL and statement scenarios (DDL/DML/prepared/transaction/ping).
- `ConsoleOutput.cs`: output formatting utilities for console logs.
- `ErrorHandlingPolicy.cs`: centralized error handling and process exit code policy.

## Running the example

1. Ensure a compatible MariaDB client library (`libmysqlclient` or equivalent) is available on the system.
2. Update the default connection credentials in `ConnectionSetup.cs` if necessary.
3. Run the example:

```bash
dotnet run --project CiccioSoft.MariaDb.Interop.Example/CiccioSoft.MariaDb.Interop.Example.csproj
```

## Notes

- This project is intended as a demo, not as a reusable library.
- The sample queries are illustrative; adjust table names and output to match your database.
