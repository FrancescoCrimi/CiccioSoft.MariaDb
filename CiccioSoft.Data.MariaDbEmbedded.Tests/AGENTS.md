---
name: CiccioSoft.Data.MariaDbEmbedded.Tests
description: "Istruzioni specifiche per il progetto di test del provider ADO.NET."
---

# Istruzioni per il progetto CiccioSoft.Data.MariaDbEmbedded.Tests

## Scopo

- Questo progetto contiene i test del provider ADO.NET.
- Verifica la corretta integrazione tra la classe provider e il layer interop.

## Cose importanti

- Scrivere test per l’uso di `MariaDbEmbeddedConnection`, `MariaDbEmbeddedCommand`, `MariaDbEmbeddedDataReader`, ecc.
- Controllare scenari di apertura/chiusura connessione, transazioni e lettura dati.

## Comandi utili

- Eseguire i test provider:
  - `dotnet test CiccioSoft.Data.MariaDbEmbedded.Tests/CiccioSoft.Data.MariaDbEmbedded.Tests.csproj`

## Lingua

- Rispondi in italiano quando discuti di questo progetto.
