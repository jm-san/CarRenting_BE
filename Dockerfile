# Usar la imagen base de .NET 8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5075
EXPOSE 5001

# Usar la imagen de SDK para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo .csproj desde la subcarpeta CarRentingApi
COPY ["CarRentingApi/CarRentingApi.csproj", "CarRentingApi/"]
RUN dotnet restore "CarRentingApi/CarRentingApi.csproj"

# Copiar el resto de los archivos
COPY . .

# Establecer el directorio de trabajo y construir el proyecto
WORKDIR "/src/CarRentingApi"
RUN dotnet build "CarRentingApi.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "CarRentingApi.csproj" -c Release -o /app/publish

# Copiar la aplicación publicada a la imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarRentingApi.dll"]