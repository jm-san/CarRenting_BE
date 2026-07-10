using Application.Common.Enums;
using Application.Vehicles.Commands.UpdateVehicle;
using Application.Vehicles.Dtos;
using Domain.Entities;
using Infrastructure.Interfaces;
using Moq;

namespace CarRenting_Tests.UnitTest;

public class VehicleServiceTest
{
    private readonly Mock<IVehicleRepository> _mockVehicleRepository;
    private readonly UpdateVehicleCommandHandler _handler;

    public VehicleServiceTest()
    {
        _mockVehicleRepository = new Mock<IVehicleRepository>();
        _handler = new UpdateVehicleCommandHandler(_mockVehicleRepository.Object);
    }

    [Fact]
    public async Task UpdateVehicleWhenVehicleDoesNotExist()
    {
        _mockVehicleRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Vehicle)null);

        var result = await _handler.Handle(new UpdateVehicleCommand(123, new UpdateVehicleDto()), CancellationToken.None);

        Assert.Equal(ETypeApiResponse.ENTITY_NOT_FOUND.ToString(), result.ApiResponseMessage);
    }

    [Fact]
    public async Task UpdateVehicleWhenValidInput()
    {
        var existingVehicle = new Vehicle { Id = 123, Brand = "Toyota", Model = "Corolla" };
        var vehicleDto = new UpdateVehicleDto { Brand = "Honda" };

        _mockVehicleRepository.Setup(repo => repo.GetByIdAsync(123)).ReturnsAsync(existingVehicle);
        _mockVehicleRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Vehicle>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(new UpdateVehicleCommand(123, vehicleDto), CancellationToken.None);

        Assert.Equal(ETypeApiResponse.OK.ToString(), result.ApiResponseMessage);
        Assert.Equal("Honda", existingVehicle.Brand);
    }
}
