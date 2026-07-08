# CarRenting_BE

Backend del proyecto de alquiler de coches. API REST en ASP.NET Core 10 con MongoDB como persistencia. Repo Git independiente (ejecuta comandos `dotnet`/`git` desde esta carpeta).

## Arquitectura

El README declara Arquitectura Hexagonal (Ports & Adapters) + DDD. En la práctica es un layering clásico con 5 proyectos en `CarRenting.sln`:

- **Domain** — entidades (`Customer`, `Rent`, `Vehicle`) y filtros de consulta (`CustomerFilter`, `RentFilter`, `VehicleFilter`). Sin dependencias a otros proyectos, pero acoplado a `MongoDB.Bson` (atributos `[BsonId]` en las entidades).
- **Infrastructure** — implementación de repositorios sobre `MongoDB.Driver` (sin EF Core, sin migraciones: no aplica con Mongo). Depende de Domain.
- **Application** — capa de servicios por feature (`Costumers/`, `Rents/`, `Vehicles/`), cada una con `Dtos/`, `MappingProfiles/` (AutoMapper), `Services/` y `Validators/` (FluentValidation). **Depende directamente de Infrastructure** (no solo de sus interfaces), lo cual rompe la regla estricta de Clean Architecture pese a lo que dice el README.
- **CarRentingApi** — composition root: controllers, `Program.cs`, DI, Swagger.
- **CarRenting-Tests** — xUnit + Moq + `Microsoft.AspNetCore.Mvc.Testing`.

Los repositorios (interfaces `I*Repository`) viven en **Infrastructure**, no en Application.

## Convenciones a seguir

- **Nombre "Costumers"**: el feature de clientes está deliberadamente nombrado `Costumers` (mal escrito, debería ser `Customers`) en carpetas y namespaces (`Application.Costumers.*`). Es un typo histórico ya extendido por todo el código — mantén la consistencia y no lo corrijas de forma aislada sin coordinarlo (renombrar rompe namespaces en varios proyectos).
- **Respuestas uniformes**: todos los métodos de servicio y acciones de controller devuelven `ApiResponse<T>` (`Application/Common/Models/ApiResponse.cs`) con un código `ETypeApiResponse` (`OK`, `VALIDATION_ERROR`, `INTERNAL_ERROR`, `CUSTOMER_WITH_ACTIVE_RENT`, `ENTITY_NOT_FOUND`) en vez de usar códigos de estado HTTP o excepciones. No hay middleware global de excepciones.
- **Validación manual**: FluentValidation se invoca a mano dentro de cada método `Create*` (`_validator.ValidateAsync(...)`), no vía pipeline/filtros automáticos de ASP.NET.
- **DI manual**: registros uno a uno en `Program.cs` (repos y servicios como `Scoped`, `MongoDBService` como `Singleton`, un `AddAutoMapper` y una registración de `IValidator<T>` por DTO). No hay escaneo de ensamblados.
- **Async**: sufijo `Async` y `Task`/`Task<T>` en toda la capa de I/O.
- **JSON**: `PropertyNamingPolicy = null` en `Program.cs` — las respuestas usan PascalCase, no camelCase.
- Namespaces con `file-scoped namespace` en el código nuevo; algunos ficheros antiguos aún usan el estilo de bloque — sigue el estilo del fichero que edites.

## Puntos a tener en cuenta (inconsistencias conocidas)

- **Nombre de base de datos duplicado**: `Infrastructure/Services/MongoDBService.cs` hardcodea `"CarRentingDb"` en el constructor, ignorando el valor `MongoDB:DatabaseName` = `"RentingDB"` configurado en `appsettings.json`. Si tocas la conexión a Mongo, revisa cuál de los dos nombres es el real antes de asumir el de configuración.
- **Dos Dockerfiles distintos**: `CarRenting_BE/Dockerfile` (raíz, target `net8.0`, probablemente el usado en CI/despliegue) y `CarRentingApi/Dockerfile` (generado por Visual Studio, target `net6.0` desactualizado, usado solo por el perfil de depuración "Container (Dockerfile)"). No los confundas.
- **Sin autenticación/autorización**: no hay `AddAuthentication`/JWT/Identity configurados. `UseAuthorization()` se llama pero no tiene efecto real — todos los endpoints están abiertos.
- **CORS totalmente abierto**: `AllowAnyOrigin/AllowAnyMethod/AllowAnyHeader`.
- **Credenciales en claro**: `appsettings.json` trae `mongodb://admin:admin@localhost:27017` committeado. El proyecto ya tiene `UserSecretsId` configurado — usa user-secrets o variables de entorno en vez de tocar ese fichero para credenciales reales.
- **`Microsoft.EntityFrameworkCore.InMemory`** está referenciado en `CarRenting-Tests` pero no se usa (no hay ningún `DbContext` en el repo) — no asumas que hay EF Core en algún sitio.
- `IntegrationTests/VehicleControllerTests.cs` y `UnitTest/VehicleServiceTest.cs` solo cubren el feature de Vehicles; Rents y Customers no tienen tests todavía. Ten cuidado además con `VehicleServiceTest`: el mock de `IVehicleRepository` se configura pero el servicio bajo test se inyecta ya construido por DI, así que el mock no afecta realmente el resultado — revisa antes de confiar en ese test como ejemplo a copiar.
- Rutas de controllers ligeramente inconsistentes: Vehicles usa `api/[controller]/[action]/` con barra final, Rents/Customers no.

## Comandos

Ejecutar siempre desde `CarRenting_BE/`:

```
dotnet restore
dotnet build
dotnet test
dotnet run --project CarRentingApi
```

API en desarrollo: `https://localhost:7127` o `http://localhost:5075` (Swagger UI habilitado solo en Development).

## Stack

.NET 10, ASP.NET Core Web API, MongoDB.Driver 3.2 (sin ORM/EF Core), AutoMapper 14, FluentValidation 11, Swashbuckle (Swagger), xUnit + Moq + Mvc.Testing para tests.
