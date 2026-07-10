using Application.Vehicles.Dtos;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace CarRenting_Tests.InfrastructureTests;

public class VehicleIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly CarRentingDbContext _dbContext;

    public VehicleIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<CarRentingDbContext>();
    }

    [Fact]
    public async Task UpdateVehicle_ShouldModifyDatabase()
    {
        var vehicle = new Vehicle { Brand = "Ford", Model = "Focus" };
        _dbContext.Vehicles.Add(vehicle);
        await _dbContext.SaveChangesAsync();

        var request = new HttpRequestMessage(HttpMethod.Put, $"/api/vehicles/{vehicle.Id}");
        request.Content = new StringContent(JsonConvert.SerializeObject(new UpdateVehicleDto
        {
            Brand = "Chevrolet"
        }), Encoding.UTF8, "application/json");

        var response = await _client.SendAsync(request);

        var updatedVehicle = await _dbContext.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.Id == vehicle.Id);

        Assert.NotNull(updatedVehicle);
        Assert.Equal("Chevrolet", updatedVehicle.Brand);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
