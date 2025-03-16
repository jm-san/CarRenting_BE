using Application.Vehicles.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace CarRenting_Tests.InfrastructureTests;

public class VehicleIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IMongoCollection<Vehicle> _vehicleCollection;

    public VehicleIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        var scope = factory.Services.CreateScope();
        var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
        var database = mongoClient.GetDatabase("TestDatabase");
        _vehicleCollection = database.GetCollection<Vehicle>("Vehicles");
    }

    [Fact]
    public async Task UpdateVehicle_ShouldModifyDatabase()
    {
        var vehicle = new Vehicle { Id = "123", Brand = "Ford", Model = "Focus" };
        await _vehicleCollection.InsertOneAsync(vehicle);

        var request = new HttpRequestMessage(HttpMethod.Put, "/api/vehicles/123");
        request.Content = new StringContent(JsonConvert.SerializeObject(new VehicleInDto
        {
            Brand = "Chevrolet"
        }), Encoding.UTF8, "application/json");

        var response = await _client.SendAsync(request);

        var updatedVehicle = await _vehicleCollection.Find(v => v.Id == "123").FirstOrDefaultAsync();

        Assert.NotNull(updatedVehicle);
        Assert.Equal("Chevrolet", updatedVehicle.Brand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}