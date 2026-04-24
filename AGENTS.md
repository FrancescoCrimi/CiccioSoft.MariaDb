---
name: AGENTS
description: "Workspace-level agent instructions for CiccioSoft.Data.MariaDbEmbedded. Use this file to understand project architecture, common conventions, and the primary build/test workflow."
---

# CiccioSoft.Data.MariaDbEmbedded Agent Instructions

## What this project is

- A .NET provider for MariaDB Embedded.
- Two-layer architecture:
  - `CiccioSoft.Data.MariaDbEmbedded.Interop`: low-level P/Invoke wrapper around MariaDB Connector/C.
  - `CiccioSoft.Data.MariaDbEmbedded`: ADO.NET provider layer built on top of the interop layer.
- Supporting projects:
  - `CiccioSoft.Data.MariaDbEmbedded.Interop.Example`: sample usage of the interop layer.
  - `CiccioSoft.Data.MariaDbEmbedded.Tests`: provider-level tests.
  - `CiccioSoft.Data.MariaDbEmbedded.Interop.Tests`: interop-level tests.

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
  - `dotnet build CiccioSoft.Data.MariaDbEmbedded.slnx`
- Run provider tests:
  - `dotnet test CiccioSoft.Data.MariaDbEmbedded.Tests/CiccioSoft.Data.MariaDbEmbedded.Tests.csproj`
- Run interop tests:
  - `dotnet test CiccioSoft.Data.MariaDbEmbedded.Interop.Tests/CiccioSoft.Data.MariaDbEmbedded.Interop.Tests.csproj`
- Run the example project:
  - `dotnet run --project CiccioSoft.Data.MariaDbEmbedded.Interop.Example/CiccioSoft.Data.MariaDbEmbedded.Interop.Example.csproj`

## What to avoid

- Do not assume the interop layer is a complete or stable API surface; it is intended as a thin wrapper for native MariaDB APIs.
- Avoid introducing API names that drift too far from MariaDB concepts unless they improve the .NET developer experience.
- Do not add new guidelines in this file; link to or update `GUIDELINES.md` for project-specific style rules.

## Useful files

- `README.md`: project overview and goals.
- `GUIDELINES.md`: development and naming guidelines.
- `CiccioSoft.Data.MariaDbEmbedded.slnx`: solution definition for build/test.
- `CiccioSoft.Data.MariaDbEmbedded.Interop.csproj`: interop layer project.
- `CiccioSoft.Data.MariaDbEmbedded.csproj`: provider layer project.
