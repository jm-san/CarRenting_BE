using Application.Common.Enums;
using Application.Vehicles.Commands.UpdateVehicle;
using Application.Vehicles.Dtos;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Interfaces;
using Moq;

namespace CarRenting_Tests.UnitTest;

public class VehicleServiceTest
{
    private readonly Mock<IVehicleRepository> _mockVehicleRepository;
    private readonly Mock<IValidator<UpdateVehicleCommand>> _mockValidator;
    private readonly UpdateVehicleCommandHandler _handler;

    public VehicleServiceTest()
    {
        _mockVehicleRepository = new Mock<IVehicleRepository>();
        _mockValidator = new Mock<IValidator<UpdateVehicleCommand>>();
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateVehicleCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _handler = new UpdateVehicleCommandHandler(_mockVehicleRepository.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task UpdateVehicleWhenVehicleDoesNotExist()
    {
        _mockVehicleRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Vehicle)null);

        var result = await _handler.Handle(new UpdateVehicleCommand("123", new VehicleInDto()), CancellationToken.None);

        Assert.Equal(ETypeApiResponse.ENTITY_NOT_FOUND.ToString(), result.ApiResponseMessage);
    }

    [Fact]
    public async Task UpdateVehicleWhenValidInput()
    {
        var existingVehicle = new Vehicle { Id = "123", Brand = "Toyota", Model = "Corolla" };
        var vehicleDto = new VehicleInDto { Brand = "Honda" };

        _mockVehicleRepository.Setup(repo => repo.GetByIdAsync("123")).ReturnsAsync(existingVehicle);
        _mockVehicleRepository.Setup(repo => repo.UpdateAsync("123", It.IsAny<Vehicle>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(new UpdateVehicleCommand("123", vehicleDto), CancellationToken.None);

        Assert.Equal(ETypeApiResponse.OK.ToString(), result.ApiResponseMessage);
        Assert.Equal("Honda", existingVehicle.Brand);
    }
}
