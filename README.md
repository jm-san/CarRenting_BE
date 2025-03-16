# Car Rental Service - Backend README

This backend service is designed for a car rental company, structured around Hexagonal Architecture and Domain-Driven Design (DDD) principles. The core domain logic is fully decoupled from infrastructure and application layers, ensuring flexibility and ease of maintenance. The project includes:

- Domain, Application, and Infrastructure layers clearly separated.
- Unit and functional tests covering business logic and service behavior.
- Infrastructure implementations with MongoDb.

The system is ready for production use, supporting future scalability and integration with external services.

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MongoDB](https://www.mongodb.com/try/download/community)
- Git

### Steps to run locally

  1. Clone the repository:
  ```bash
  git clone https://github.com/your-username/your-repo.git
  cd your-repo
  ```
  
  2. Restore dependencies:
  ```bash
  dotnet restore
  ```
  
  3. Update the appsettings.json with your local MongoDB connection string.
  
  4. Run database migrations or seed data if necessary.
  
  5. Start the API:
  ```bash
  dotnet run
  ```

  6. The API will be available at https://localhost:7127 or http://localhost:5075 by default.
