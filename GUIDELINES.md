# CiccioSoft.Data.MariaDbEmbedded Development Guidelines

Questo documento raccoglie le linee guida per lo sviluppo della libreria. Serve come riferimento centrale per mantenere coerenza, qualità e stile nel codice.

## 1. Naming Guidelines

- I nomi delle classi devono rispecchiare i nomi degli oggetti della libreria nativa (ad esempio quelli definiti in `mysql.h`), adattati allo stile C#.
- Preferire nomi concisi ma descrittivi.
- Usare PascalCase per nomi di classi, enum e metodi.
- Usare nomi come `MySql` per wrapper di oggetti MySQL nativi, seguendo la terminologia della libreria nativa.
- Evitare suffissi superflui come `Client` quando il nome dell'oggetto nativo è già chiaro e il contesto è allineato.

## 2. Struttura del repository

_Questa sezione sarà aggiornata con le linee guida sulla struttura dei progetti e sugli artefatti condivisi._

## 3. Gestione delle dipendenze native

_Questa sezione sarà aggiornata con le linee guida per il packaging e il caricamento delle librerie native._

## 4. Documentazione e esempio

- Aggiornare sempre la documentazione di esempio quando si rinominano classi o metodi pubblici.
- Il file `README.md` deve mostrare i casi d'uso più rilevanti e funzionanti per le API pubbliche.

## 5. Aggiunta di nuove linee guida

- Aggiungere ogni nuova regola come sezione numerata in questo documento.
- Mantenere il linguaggio semplice, diretto e focalizzato su decisioni di design e stile.
