---
name: CiccioSoft.Data.MariaDbEmbedded
description: "Istruzioni specifiche per il progetto ADO.NET provider."
---

# Istruzioni per il progetto CiccioSoft.Data.MariaDbEmbedded

## Scopo

- Questo progetto implementa il provider ADO.NET basato su `CiccioSoft.Data.MariaDbEmbedded.Interop`.
- Fornisce classi come `MariaDbEmbeddedConnection`, `MariaDbEmbeddedCommand`, `MariaDbEmbeddedDataReader`, `MariaDbEmbeddedParameter` e `MariaDbEmbeddedTransaction`.
- Deve offrire un’esperienza .NET idiomatica e compatibile con ADO.NET.

## Cose importanti

- Mantieni l’interfaccia pubblica fedele alle convenzioni ADO.NET.
- Evita di duplicare la logica che appartiene al livello interop; delega le operazioni native a `CiccioSoft.Data.MariaDbEmbedded.Interop`.
- Aggiorna `README.md` e `GUIDELINES.md` quando cambi l’API pubblica o il comportamento di connessioni/transazioni.

## Comandi utili

- Compilare il progetto:
  - `dotnet build CiccioSoft.Data.MariaDbEmbedded/CiccioSoft.Data.MariaDbEmbedded.csproj`
- Eseguire i test del provider:
  - `dotnet test CiccioSoft.Data.MariaDbEmbedded.Tests/CiccioSoft.Data.MariaDbEmbedded.Tests.csproj`

## Lingua

- Rispondi in italiano quando discuti di questo progetto.
