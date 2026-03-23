# API Planning .NET Core

Dokumen ini adalah rencana pengembangan API menggunakan `.NET Core` dengan fokus pada arsitektur modern, mudah diuji, dan siap deploy.

## 1) Arsitektur Microservices

### Tujuan
- Memecah sistem menjadi service kecil yang punya tanggung jawab spesifik.
- Memudahkan scaling per service.
- Meminimalkan dampak perubahan antar modul.

### Service yang disarankan
- `Gateway API` (opsional): routing, auth, rate limit.
- `Biodata Service`: CRUD data biodata.
- `Auth Service` (opsional): autentikasi dan otorisasi.

### Komunikasi
- HTTP REST antar client dan service.
- Gunakan event bus (RabbitMQ/Kafka) bila nantinya butuh komunikasi async.

## 2) Dependency Injection (DI)

### Prinsip
- Gunakan built-in DI dari ASP.NET Core (`IServiceCollection`).
- Terapkan pola `Interface -> Implementation`.
- Pisahkan layer:
  - API (Controller/Minimal API)
  - Application (Use Case/Service)
  - Infrastructure (Repository, DB)

### Registrasi umum
- `AddScoped` untuk service bisnis dan repository.
- `AddSingleton` untuk komponen stateless jangka panjang.
- `AddTransient` untuk object ringan yang sering dibuat.

## 3) Menggunakan `record`

Gunakan `record` untuk DTO/response model agar immutable dan ringkas.

Contoh:

```csharp
public record BiodataRequest(string Nama, DateOnly TanggalLahir, string Alamat, string NoHp);
public record BiodataResponse(Guid Id, string Nama, DateOnly TanggalLahir, string Alamat, string NoHp);
```

Manfaat:
- Sintaks lebih bersih.
- Cocok untuk data transfer object.
- Mendukung value-based equality.

## 4) Koneksi ke PostgreSQL

### Stack yang disarankan
- ORM: `Entity Framework Core`.
- Provider: `Npgsql.EntityFrameworkCore.PostgreSQL`.

### Konfigurasi
- Simpan connection string di `appsettings.json`.
- Inject `DbContext` via DI.
- Gunakan migration untuk versioning schema database.

Contoh connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=biodata_db;Username=postgres;Password=postgres"
}
```

## 5) Unit Test

### Framework
- `xUnit` untuk test framework.
- `Moq` untuk mocking dependency.
- `FluentAssertions` untuk assertion yang lebih readable.

### Target pengujian
- Service layer (business rules).
- Repository abstraction (mocked saat unit test).
- Endpoint behavior (opsional: integration test terpisah).

### Struktur folder test
- `tests/BiodataService.UnitTests`
- `tests/BiodataService.IntegrationTests` (opsional)

## 6) Rate Limit

Gunakan middleware rate limiting bawaan ASP.NET Core (pada .NET 7+).

### Tujuan
- Mencegah abuse API.
- Menjaga stabilitas service saat traffic tinggi.

### Strategi awal
- Fixed window: contoh 100 request/menit per IP.
- Response HTTP `429 Too Many Requests` saat limit tercapai.

## 7) Dockerfile

Siapkan Dockerfile multi-stage untuk build image yang ringan.

Contoh:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "BiodataService.dll"]
```

## 8) Desain Table `biodata`

### Kolom utama
- `id` UUID (PK)
- `nama` VARCHAR(150) NOT NULL
- `tanggal_lahir` DATE NOT NULL
- `alamat` TEXT
- `no_hp` VARCHAR(20)
- `created_at` TIMESTAMP NOT NULL DEFAULT now()
- `updated_at` TIMESTAMP NOT NULL DEFAULT now()

Contoh SQL:

```sql
CREATE TABLE biodata (
    id UUID PRIMARY KEY,
    nama VARCHAR(150) NOT NULL,
    tanggal_lahir DATE NOT NULL,
    alamat TEXT,
    no_hp VARCHAR(20),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);
```

## Rencana Implementasi Bertahap

1. Inisialisasi solution `.NET Core` dan project per service.
2. Setup DI + layering (API, Application, Infrastructure).
3. Integrasi PostgreSQL + migration pertama (`biodata` table).
4. Implement endpoint CRUD biodata.
5. Tambahkan rate limiting.
6. Tulis unit test utama.
7. Tambahkan Dockerfile dan validasi build container.

## Kriteria Sukses

- API CRUD biodata berjalan normal.
- Koneksi PostgreSQL stabil.
- Unit test utama lulus.
- Rate limit aktif dan mengembalikan status `429` saat melebihi kuota.
- Service dapat dijalankan via Docker.
