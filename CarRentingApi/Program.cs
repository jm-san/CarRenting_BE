using Application.Common.Behaviors;
using Application.Costumers.Commands.CreateCustomer;
using Application.Costumers.Commands.DeleteCustomer;
using Application.Costumers.Commands.UpdateCustomer;
using Application.Costumers.MappingProfiles;
using Application.Costumers.Queries.GetCustomer;
using Application.Rents.Dtos;
using Application.Rents.MappingProfiles;
using Application.Rents.Services;
using Application.Rents.Validators;
using Application.Vehicles.Commands.CreateVehicle;
using Application.Vehicles.Commands.DeleteVehicle;
using Application.Vehicles.Commands.UpdateVehicle;
using Application.Vehicles.MappingProfiles;
using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Services;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add controllers to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Add AutoMappers
        builder.Services.AddAutoMapper(typeof(CustomerMappingProfile));
        builder.Services.AddAutoMapper(typeof(VehicleMappingProfile));
        builder.Services.AddAutoMapper(typeof(RentMappingProfile));

        //Add MediatR (Customers y Vehicles usan CQRS)
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        //Add Validators
        builder.Services.AddScoped<IValidator<CreateCustomerCommand>, CreateCustomerCommandValidator>();
        builder.Services.AddScoped<IValidator<UpdateCustomerCommand>, UpdateCustomerCommandValidator>();
        builder.Services.AddScoped<IValidator<DeleteCustomerCommand>, DeleteCustomerCommandValidator>();
        builder.Services.AddScoped<IValidator<GetCustomerQuery>, GetCustomerQueryValidator>();
        builder.Services.AddScoped<IValidator<CreateVehicleCommand>, CreateVehicleCommandValidator>();
        builder.Services.AddScoped<IValidator<UpdateVehicleCommand>, UpdateVehicleCommandValidator>();
        builder.Services.AddScoped<IValidator<DeleteVehicleCommand>, DeleteVehicleCommandValidator>();
        builder.Services.AddScoped<IValidator<RentInDto>, RentInDtoValidator>();

        // Register repositories and services
        builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
        builder.Services.AddScoped<IRentRepository, RentRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<RentService>();

        //MongoDB configurations
        builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
        builder.Services.AddSingleton<MongoDBService>();

        //No transform attributes to lowercase
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //Allow CORS policy
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}