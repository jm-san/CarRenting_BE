using Application.Costumers.Dtos;
using Application.Costumers.MappingProfiles;
using Application.Costumers.Services;
using Application.Costumers.Validators;
using Application.Rents.Dtos;
using Application.Rents.MappingProfiles;
using Application.Rents.Services;
using Application.Rents.Validators;
using Application.Vehicles.Dtos;
using Application.Vehicles.MappingProfiles;
using Application.Vehicles.Services;
using Application.Vehicles.Validators;
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

        //Add Validators
        builder.Services.AddScoped<IValidator<CustomerInDto>, CustomerInDtoValidator>();
        builder.Services.AddScoped<IValidator<RentInDto>, RentInDtoValidator>();
        builder.Services.AddScoped<IValidator<VehicleInDto>, VehicleInDtoValidator>();

        // Register repositories and services
        builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
        builder.Services.AddScoped<IRentRepository, RentRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<VehicleService>();
        builder.Services.AddScoped<RentService>();
        builder.Services.AddScoped<CustomerService>();

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