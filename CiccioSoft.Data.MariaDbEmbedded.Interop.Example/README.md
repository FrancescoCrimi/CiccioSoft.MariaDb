# CiccioSoft.Data.MariaDbEmbedded.Interop.Example

This project contains a simple example that demonstrates how to use the `CiccioSoft.Data.MariaDbEmbedded.Interop` layer.

## Purpose

- Show how to initialize and connect to MariaDB using the `MySql` wrapper.
- Execute example queries and print results to the console.
- Provide a starting point for exploring the interop API.

## Running the example

1. Ensure a compatible MariaDB client library (`libmysqlclient` or equivalent) is available on the system.
2. Update the connection credentials in `Program.cs` if necessary.
3. Run the example:

```bash
dotnet run --project CiccioSoft.Data.MariaDbEmbedded.Interop.Example/CiccioSoft.Data.MariaDbEmbedded.Interop.Example.csproj
```

## Notes

- This project is intended as a demo, not as a reusable library.
- The sample queries are illustrative; adjust table names and output to match your database.
