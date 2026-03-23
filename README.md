# API WITH AI CURSOR

Implementasi API `.NET Core` berdasarkan planning:
- Microservices (`GatewayApi` + `BiodataService.Api`)
- Dependency Injection
- `record` DTO
- PostgreSQL
- Unit Test (`xUnit`, `Moq`, `FluentAssertions`)
- Rate limiting
- Dockerfile + `docker-compose`
- Tabel `biodata`

## Struktur

- `src/GatewayApi`
- `src/services/BiodataService/BiodataService.Api`
- `src/services/BiodataService/BiodataService.Application`
- `src/services/BiodataService/BiodataService.Infrastructure`
- `src/services/BiodataService/BiodataService.Domain`
- `tests/BiodataService.UnitTests`

## Menjalankan dengan Docker

```bash
docker compose up --build
```

Service:
- Gateway: `http://localhost:5000`
- Biodata API: `http://localhost:5100`
- PostgreSQL: `localhost:5432`

## Endpoint Biodata

- `GET /api/biodata`
- `GET /api/biodata/{id}`
- `POST /api/biodata`
- `PUT /api/biodata/{id}`
- `DELETE /api/biodata/{id}`

Contoh body `POST /api/biodata`:

```json
{
  "nama": "Mikel",
  "tanggalLahir": "2000-01-01",
  "alamat": "Bandung",
  "noHp": "08123456789"
}
```

## Menjalankan Test

```bash
dotnet test
```
