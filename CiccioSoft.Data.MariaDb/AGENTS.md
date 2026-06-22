---
name: CiccioSoft.Data.MariaDb
description: "Istruzioni specifiche per il progetto ADO.NET provider."
---

# Istruzioni per il progetto CiccioSoft.Data.MariaDb

## Scopo

- Questo progetto implementa il provider ADO.NET basato su `CiccioSoft.MariaDb.Interop`.
- Fornisce classi come `MariaDbConnection`, `MariaDbCommand`, `MariaDbDataReader`, `MariaDbParameter` e `MariaDbTransaction`.
- Deve offrire un’esperienza .NET idiomatica e compatibile con ADO.NET.

## Cose importanti

- Mantieni l’interfaccia pubblica fedele alle convenzioni ADO.NET.
- Evita di duplicare la logica che appartiene al livello interop; delega le operazioni native a `CiccioSoft.MariaDb.Interop`.
- Aggiorna `README.md` e `GUIDELINES.md` quando cambi l’API pubblica o il comportamento di connessioni/transazioni.

## Comandi utili

- Compilare il progetto:
  - `dotnet build CiccioSoft.Data.MariaDb/CiccioSoft.Data.MariaDb.csproj`
- Eseguire i test del provider:
  - `dotnet test CiccioSoft.Data.MariaDb.Tests/CiccioSoft.Data.MariaDb.Tests.csproj`

## Lingua

- Rispondi in italiano quando discuti di questo progetto.
