# CarRenting_BE

Backend del proyecto de alquiler de coches. API REST en ASP.NET Core 10 con PostgreSQL (EF Core) como persistencia. Repo Git independiente (ejecuta comandos `dotnet`/`git` desde esta carpeta).

## Arquitectura

El README declara Arquitectura Hexagonal (Ports & Adapters) + DDD. En la práctica es un layering clásico con 5 proyectos en `CarRenting.sln`:

- **Domain** — entidades (`Customer`, `Rent`, `Vehicle`, con `Id` de tipo `int`) y filtros de consulta (`CustomerFilter`, `RentFilter`, `VehicleFilter`). Sin dependencias a otros proyectos ni a ningún ORM.
- **Infrastructure** — `CarRentingDbContext` (EF Core, `Infrastructure/Persistence/`) y las migraciones (`Infrastructure/Persistence/Migrations/`). Repositorios específicos por entidad (`ICustomerRepository`, `IVehicleRepository`, `IRentRepository` en `Interfaces/`, implementados en `Repositories/`) — no hay un repositorio genérico, cada uno usa LINQ tipado (`RentRepository` hace `.Include(Customer)`/`.Include(Vehicle)`). Depende de Domain.
- **Application** — capa de servicios por feature (`Costumers/`, `Rents/`, `Vehicles/`), cada una con `Dtos/`, `MappingProfiles/` (AutoMapper), `Commands/`/`Queries/` (MediatR, patrón CQRS) y `Validators/` (FluentValidation). **Depende directamente de Infrastructure** (no solo de sus interfaces), lo cual rompe la regla estricta de Clean Architecture pese a lo que dice el README.
- **CarRentingApi** — composition root: controllers, `Program.cs`, DI, Swagger. En Development aplica las migraciones EF automáticamente al arrancar (`Database.Migrate()`).
- **CarRenting-Tests** — xUnit + Moq + `Microsoft.AspNetCore.Mvc.Testing`.

## Convenciones a seguir

- **Nombre "Costumers"**: el feature de clientes está deliberadamente nombrado `Costumers` (mal escrito, debería ser `Customers`) en carpetas y namespaces (`Application.Costumers.*`). Es un typo histórico ya extendido por todo el código — mantén la consistencia y no lo corrijas de forma aislada sin coordinarlo (renombrar rompe namespaces en varios proyectos).
- **Respuestas uniformes**: todos los métodos de servicio y acciones de controller devuelven `ApiResponse<T>` (`Application/Common/Models/ApiResponse.cs`) con un código `ETypeApiResponse` (`OK`, `VALIDATION_ERROR`, `INTERNAL_ERROR`, `CUSTOMER_WITH_ACTIVE_RENT`, `ENTITY_NOT_FOUND`) en vez de usar códigos de estado HTTP o excepciones. No hay middleware global de excepciones. Los `Create`/`Update`/`Delete` devuelven `ApiResponse<int>` (el Id de la entidad), no `ApiResponse<string>`.
- **CQRS con MediatR**: cada operación (Command o Query) vive en su propia carpeta bajo `Commands/`/`Queries/` con su record, su handler y (si aplica) su validator. El pipeline de MediatR ejecuta `ValidationBehavior<,>` (`Application/Common/Behaviors/ValidationBehavior.cs`) antes de cada handler.
- **Repositorios específicos por entidad, no genéricos**: `ICustomerRepository`/`IVehicleRepository`/`IRentRepository` en vez de un `IRepository<TEntity, TFilter>` compartido. `UpdateAsync(entity)` no recibe el `id` por separado — como la entidad viene trackeada por el `DbContext` (scoped), basta con mutarla y llamar a `SaveChangesAsync`.
- **DI manual**: registros uno a uno en `Program.cs` (repos como `Scoped`, `AddDbContext<CarRentingDbContext>`, un `AddAutoMapper` y una registración de `IValidator<T>` por DTO). No hay escaneo de ensamblados.
- **Async**: sufijo `Async` y `Task`/`Task<T>` en toda la capa de I/O.
- **JSON**: `PropertyNamingPolicy = null` en `Program.cs` — las respuestas usan PascalCase, no camelCase.
- Namespaces con `file-scoped namespace` en el código nuevo; algunos ficheros antiguos aún usan el estilo de bloque — sigue el estilo del fichero que edites.

## Persistencia (PostgreSQL + EF Core)

- Levantar Postgres local: `docker compose up -d` desde esta carpeta (usa `docker-compose.yml`, Postgres 16, `RentingDB`/`postgres`/`postgres` en el puerto 5432).
- Connection string en `appsettings.json` → `ConnectionStrings:Default`. Son credenciales de desarrollo únicamente (igual que pasaba antes con Mongo); para credenciales reales usa user-secrets (`UserSecretsId` ya configurado) o variables de entorno.
- En Development, `Program.cs` aplica las migraciones pendientes automáticamente al arrancar. No hace falta `dotnet ef database update` manualmente en local.
- Para generar una nueva migración tras cambiar una entidad o el `OnModelCreating` de `CarRentingDbContext`:
  ```
  dotnet ef migrations add NombreDeLaMigracion --project Infrastructure --startup-project CarRentingApi --output-dir Persistence/Migrations
  ```
- Requiere la herramienta `dotnet-ef` instalada (`dotnet tool install --global dotnet-ef`).

## Puntos a tener en cuenta (inconsistencias conocidas)

- **Dos Dockerfiles distintos**: `CarRenting_BE/Dockerfile` (raíz, probablemente el usado en CI/despliegue) y `CarRentingApi/Dockerfile` (generado por Visual Studio, usado solo por el perfil de depuración "Container (Dockerfile)"). Ambos en `net10.0`; no los confundas entre sí. Ninguno de los dos levanta Postgres — para eso está `docker-compose.yml`.
- **Sin autenticación/autorización**: no hay `AddAuthentication`/JWT/Identity configurados. `UseAuthorization()` no se llama; todos los endpoints están abiertos.
- **CORS totalmente abierto**: `AllowAnyOrigin/AllowAnyMethod/AllowAnyHeader`.
- `IntegrationTests/VehicleControllerTests.cs` y `UnitTest/VehicleServiceTest.cs` solo cubren el feature de Vehicles; Rents y Customers no tienen tests todavía.
- `IntegrationTests/VehicleControllerTests.cs` espera `HttpStatusCode.NotFound` al actualizar un vehículo inexistente, pero la app nunca usa códigos de estado HTTP (todo va en `ApiResponse<T>.ApiResponseMessage`) — ese test está roto y no se ha corregido su aserción, no lo tomes como ejemplo a copiar.
- Los tests de integración (`CarRenting-Tests/IntegrationTests`, `CarRenting-Tests/InfrastructureTests`) necesitan Postgres accesible (`docker compose up -d`) para pasar; sin él fallan con `Npgsql.NpgsqlException: Failed to connect`.
- Rutas de controllers ligeramente inconsistentes: Vehicles usa `api/[controller]/[action]/` con barra final, Rents/Customers no.

## Comandos

Ejecutar siempre desde `CarRenting_BE/`:

```
docker compose up -d
dotnet restore
dotnet build
dotnet test
dotnet run --project CarRentingApi
```

API en desarrollo: `https://localhost:7127` o `http://localhost:5075` (Swagger UI habilitado solo en Development).

## Stack

.NET 10, ASP.NET Core Web API, PostgreSQL + EF Core 9 (`Npgsql.EntityFrameworkCore.PostgreSQL`), MediatR (CQRS), AutoMapper 14, FluentValidation 11, Swashbuckle (Swagger), xUnit + Moq + Mvc.Testing para tests.
