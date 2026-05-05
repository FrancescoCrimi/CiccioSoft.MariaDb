---
name: CiccioSoft.Data.MariaDbEmbedded.Interop
description: "Istruzioni specifiche per il progetto di interoperabilità con MariaDB Connector/C."
---

# Istruzioni per il progetto CiccioSoft.Data.MariaDbEmbedded.Interop

## Scopo

- Questo progetto espone un wrapper managed attorno alle API native di MariaDB Connector/C.
- Implementa P/Invoke, tipi nativi, strutture e wrapper OOP per gli oggetti MySQL/MariaDB.

## Cose importanti

- P/Invoke generati con ClangSharpPInvokeGenerator con tipi blittabili
- Mantenere la superficie API minima e diretta; è un layer thin wrapper.
- Api OOP e idiomatica.
- I nomi delle classi devono riflettere i concetti nativi (`MySql`, `MySqlException`, `MySqlResult`, ecc.) ma con stile C#.
- Deve coprire tutte le funzionalità crud di un database rdbms generico.
- Non introdurre logica ADO.NET qui: il progetto interop deve rimanere indipendente dal provider.

## Comandi utili

- Compilare il progetto:
  - `dotnet build CiccioSoft.Data.MariaDbEmbedded.Interop/CiccioSoft.Data.MariaDbEmbedded.Interop.csproj`
- Eseguire i test interop:
  - `dotnet test CiccioSoft.Data.MariaDbEmbedded.Interop.Tests/CiccioSoft.Data.MariaDbEmbedded.Interop.Tests.csproj`

## Lingua

- Rispondi in italiano quando discuti di questo progetto.
