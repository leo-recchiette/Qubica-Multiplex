# Qubica Multiplex

Backend di un sistema di prenotazione posti per un cinema, a microservizi, in .NET 10.

## Architettura

| Servizio | Responsabilità | Tabelle |
|---|---|---|
| **Catalog** | film, sale, programmazione | `movie`, `director`, `genre`, `hall`, `showtime` |
| **Booking** | disponibilità, prenotazioni, cancellazioni | `booking`, `booking_seat` |
| **Customer** | registrazione degli utenti | `customer` |

I servizi usano lo stesso database, ognuno con il proprio schema (`catalog`, `booking`, `customer`). Booking comunica con gli altri due servizi via HTTP: chiede al Catalog la piantina della sala di uno spettacolo (`GET api/Catalog/showtime/{showId}`) per confrontarla con i posti già prenotati, e a Customer se il cliente esiste (`GET api/Customer/{customerId}`).

## Avvio

Serve solo Docker.

```bash
docker compose up -d --build
```

| | URL |
|---|---|
| Catalog (Swagger) | http://localhost:8080/swagger |
| Booking (Swagger) | http://localhost:8081/swagger |
| Customer (Swagger) | http://localhost:8082/swagger |
| PostgreSQL | `localhost:5432`, utente e password `postgres` |

Al primo avvio Postgres esegue gli script in `db/`, che creano gli schemi e caricano i dati di esempio: 9 film, 3 sale e 18 spettacoli tra il 20 e il 22 ottobre 2026. Gli script girano solo con il database vuoto. Per ripartire da zero:

```bash
docker compose down -v
docker compose up -d --build
```

## Struttura

```
db/       script SQL di inizializzazione (schema e dati)
src/      QubicaMultiplex.Catalog.Api, QubicaMultiplex.Booking.Api, QubicaMultiplex.Customer.Api
tests/    un progetto di test per ogni servizio
```

Struttura a tre livelli. Ogni servizio è organizzato in `Controllers` → `Services` → `Repositories`.

## API

### Catalog — `:8080`

| Metodo | Rotta | Descrizione |
|---|---|---|
| GET | `api/Catalog/allMovies` | elenco dei film (`id`, `title`) |
| GET | `api/Catalog/details/{id}` | dettaglio di un film |
| GET | `api/Catalog/shows/{movieId}` | spettacoli di un film, con orario e sala |
| GET | `api/Catalog/showtime/{showId}` | uno spettacolo con la piantina della sala |

### Booking — `:8081`

| Metodo | Rotta | Descrizione |
|---|---|---|
| GET | `api/Seat/availability/{showId}` | posti liberi di uno spettacolo |
| POST | `api/Booking/book` | prenota uno o più spettacoli |
| GET | `api/Booking/customer/{customerId}` | prenotazioni di un cliente |
| DELETE | `api/Booking/remove/{bookingId}` | cancella una prenotazione |
| DELETE | `api/Booking/remove/{bookingId}/show/{showId}` | cancella un solo spettacolo di una prenotazione |

### Prenotare

```json
POST api/Booking/book
{
  "customerId": 1,
  "shows": [
    { "showId": 13, "seats": { "chosenSeats": [{ "row": "F", "number": 12 }, { "row": "F", "number": 13 }], "quantityToBook": 2 } },
    { "showId": 7,  "seats": { "chosenSeats": [], "quantityToBook": 3 } }
  ]
}
```

- **`chosenSeats` piena:** si prenotano quei posti, che devono essere tanti quanto `quantityToBook`.
- **`chosenSeats` vuota:** il sistema assegna i primi `quantityToBook` posti liberi, a partire da A1.
- **Tutto o niente:** se anche un solo spettacolo della richiesta non può essere prenotato, non si salva niente.

La risposta contiene l'id della prenotazione e i posti assegnati per ogni spettacolo.

| Codice | Quando |
|---|---|
| 200 | prenotazione creata |
| 400 | richiesta non valida (es. fila diversa da una lettera A–Z, posti ripetuti, numero di posti diverso da `quantityToBook`) |
| 404 | il cliente o lo spettacolo non esiste |
| 409 | posto non disponibile (già preso o inesistente nella sala), oppure posti liberi insufficienti |

### Customer — `:8082`

| Metodo | Rotta | Descrizione |
|---|---|---|
| GET | `api/Customer/{customerId}` | dettaglio di un utente |
| POST | `api/Customer/register` | registra un utente: `{ "email": "user@example.com" }` |

La registrazione restituisce `id`, `email` e data di creazione. Risponde 400 se l'email non è nel formato `nome@dominio.estensione` e 409 se è già registrata.

## Test

```bash
dotnet test
```

Senza .NET SDK installato si usa la [pipeline di build](#pipeline-di-build), che richiede solo Docker.

Test unitari con MSTest, NSubstitute e AwesomeAssertions su service, extension method e validator.

## Pipeline di build

Build e test girano in un container con il .NET 10 SDK, creato all'inizio e cancellato alla fine. Jenkins e GitHub Actions eseguono gli stessi target del Makefile.

| File | Cosa fa |
|---|---|
| `pipeline.dockerfile` | immagine di build: .NET 10 SDK su Alpine |
| `docker-compose.pipeline.yml` | avvia il container di build `qubica-pipeline`, con il repo montato su `/work` |
| `Makefile` | target della pipeline: ognuno esegue un comando nel container |
| `Jenkinsfile` | pipeline Jenkins: `setup` → `build` → `test`, e `teardown` sempre alla fine |
| `.github/workflows/ci.yml` | pipeline GitHub Actions, a ogni push e pull request: `setup` → `build` → `test` (niente `teardown`: la macchina di GitHub viene distrutta a fine esecuzione) |


Per eseguire tutta la pipeline in locale:

```bash
make setup build test teardown
```