---
name: AGENTS
description: "Workspace-level agent instructions for CiccioSoft.MariaDb. Use this file to understand project architecture, common conventions, and the primary build/test workflow."
---

# CiccioSoft.MariaDb Agent Instructions

## What this project is

- A .NET provider for MariaDB Embedded.
- Two-layer architecture:
  - `CiccioSoft.Interop.MariaDb`: low-level P/Invoke wrapper around MariaDB Connector/C.
  - `CiccioSoft.Data.MariaDb`: ADO.NET provider layer built on top of the interop layer.
- Supporting projects:
  - `CiccioSoft.Interop.MariaDb.Example`: sample usage of the interop layer.
  - `CiccioSoft.Data.MariaDb.Tests`: provider-level tests.
  - `CiccioSoft.Interop.MariaDb.Tests`: interop-level tests.

## Lingua

- La lingua principale per le istruzioni AI è l’italiano.
- I file `AGENTS.md` possono essere in italiano; la terminologia tecnica e i nomi di codice vanno mantenuti in inglese o come appaiono nel codice.

## Key conventions

- Follow the repository’s naming guidance: use PascalCase, align wrapper class names with native MariaDB/MySQL concepts, and avoid redundant suffixes when the object is already clear.
- Keep public API names idiomatic for .NET while preserving the underlying MariaDB semantics.
- Update `README.md` and `GUIDELINES.md` when public API or sample usage changes.
- This project is early-stage; focus on correctness, maintainability, and compatibility with native MariaDB behavior.

## Common tasks

- Build the full solution:
  - `dotnet build CiccioSoft.MariaDb.slnx`
- Run provider tests:
  - `dotnet test CiccioSoft.Data.MariaDb.Tests/CiccioSoft.Data.MariaDb.Tests.csproj`
- Run interop tests:
  - `dotnet test CiccioSoft.Interop.MariaDb.Tests/CiccioSoft.Interop.MariaDb.Tests.csproj`
- Run the example project:
  - `dotnet run --project CiccioSoft.Interop.MariaDb.Example/CiccioSoft.Interop.MariaDb.Example.csproj`

## What to avoid

- Do not assume the interop layer is a complete or stable API surface; it is intended as a thin wrapper for native MariaDB APIs.
- Avoid introducing API names that drift too far from MariaDB concepts unless they improve the .NET developer experience.
- Do not add new guidelines in this file; link to or update `GUIDELINES.md` for project-specific style rules.

## Useful files

- `README.md`: project overview and goals.
- `GUIDELINES.md`: development and naming guidelines.
- `CiccioSoft.Data.MariaDb.slnx`: solution definition for build/test.
- `CiccioSoft.Data.MariaDb.Interop.csproj`: interop layer project.
- `CiccioSoft.Data.MariaDb.csproj`: provider layer project.
