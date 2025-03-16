using Application.Vehicles.Dtos;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace CarRenting_Tests.IntegrationTests;

public class VehicleControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public VehicleControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateVehicleNotFoundVehicleDoesNotExist()
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "/api/Vehicles/999");
        request.Content = new StringContent(JsonConvert.SerializeObject(new VehicleInDto
        {
            Brand = "Nissan"
        }), Encoding.UTF8, "application/json");

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}