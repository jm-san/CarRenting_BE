using Application.Common.Enums;
using Application.Vehicles.Dtos;
using Application.Vehicles.Services;
using Domain.Entities;
using Infrastructure.Interfaces;
using Moq;

namespace CarRenting_Tests.UnitTest;

public class VehicleServiceTest
{
    private readonly Mock<IVehicleRepository> _mockVehicleRepository;
    private readonly VehicleService _vehicleService;

    public VehicleServiceTest(IVehicleRepository vehicleRepository,
                            VehicleService vehicleService)
    {
        _mockVehicleRepository = new Mock<IVehicleRepository>();
        _vehicleService = vehicleService;
    }

    [Fact]
    public async Task UpdateVehicleWhenVehicleDoesNotExist()
    {
        _mockVehicleRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Vehicle)null);

        var result = await _vehicleService.UpdateVehicle("123", new VehicleInDto());

        Assert.Equal(ETypeApiResponse.ENTITY_NOT_FOUND.ToString(), result.ApiResponseMessage);
    }

    [Fact]
    public async Task UpdateVehicleWhenValidInput()
    {
        var existingVehicle = new Vehicle { Id = "123", Brand = "Toyota", Model = "Corolla" };
        var vehicleDto = new VehicleInDto { Brand = "Honda" };

        _mockVehicleRepository.Setup(repo => repo.GetByIdAsync("123")).ReturnsAsync(existingVehicle);
        _mockVehicleRepository.Setup(repo => repo.UpdateAsync("123", It.IsAny<Vehicle>())).Returns(Task.CompletedTask);

        var result = await _vehicleService.UpdateVehicle("123", vehicleDto);

        Assert.Equal(ETypeApiResponse.OK.ToString(), result.ApiResponseMessage);
        Assert.Equal("Honda", existingVehicle.Brand);
    }
}
