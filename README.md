# CiccioSoft.Data.MariaDbEmbedded

This repository contains the library for an ADO.NET provider for MariaDB Embedded, designed to work similarly to existing .NET providers for SQLite.

## Project scope

The primary goal is to provide a .NET provider for MariaDB Embedded composed of two layers:

1. `CiccioSoft.Data.MariaDbEmbedded.Interop`
   - Low-level library exposing an idiomatic, OOP wrapper for MariaDB Connector/C.
   - Implements a P/Invoke binding to MariaDB native APIs and a managed layer with .NET objects.

2. `CiccioSoft.Data.MariaDbEmbedded`
   - The actual .NET provider built on top of the `Interop` layer.
   - Provides ADO.NET integration and the typical experience of embedded .NET database providers.

## Architecture

- `CiccioSoft.Data.MariaDbEmbedded.Interop`
  - Contains the native structures, methods, and P/Invoke definitions for MariaDB.
  - Low-level library exposing an idiomatic, OOP wrapper for MariaDB Connector/C.

- `CiccioSoft.Data.MariaDbEmbedded`
  - Contains the ADO.NET provider that uses the `Interop` layer.
  - This is the layer .NET applications will use to connect to MariaDB Embedded.

## Final goals

- Build an ADO.NET provider for MariaDB Embedded that is:
  - idiomatic for .NET
  - aligned with the style of .NET providers for SQLite
  - easy to use in embedded scenarios
  - reliable and maintainable

- Provide an OOP wrapper around `MYSQL*` and the main MariaDB APIs, with .NET-style names and behavior.

## Getting started

1. Build the `CiccioSoft.Data.MariaDbEmbedded.Interop` project.
2. Use the `MySql.Open(...)` wrapper to open connections to MariaDB Embedded.
3. Extend the `CiccioSoft.Data.MariaDbEmbedded` provider to support full ADO.NET.

## Documentation

- `GUIDELINES.md` contains the development guidelines for the project.

## Notes

- This repository is still in an early stage and represents the foundation for the MariaDB Embedded provider.
- The final implementation must ensure compatibility with the native MariaDB library and a natural .NET experience.
