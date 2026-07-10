using Application.Common.Behaviors;
using Application.Costumers.Commands.CreateCustomer;
using Application.Costumers.Commands.DeleteCustomer;
using Application.Costumers.Commands.UpdateCustomer;
using Application.Costumers.MappingProfiles;
using Application.Costumers.Queries.GetCustomer;
using Application.Rents.Commands.CreateRent;
using Application.Rents.Commands.DeleteRent;
using Application.Rents.Commands.UpdateRentActivity;
using Application.Rents.MappingProfiles;
using Application.Vehicles.Commands.CreateVehicle;
using Application.Vehicles.Commands.DeleteVehicle;
using Application.Vehicles.Commands.UpdateVehicle;
using Application.Vehicles.MappingProfiles;
using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Add AutoMappers
        builder.Services.AddAutoMapper(typeof(CustomerMappingProfile));
        builder.Services.AddAutoMapper(typeof(VehicleMappingProfile));
        builder.Services.AddAutoMapper(typeof(RentMappingProfile));

        //Add MediatR
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
        builder.Services.AddScoped<IValidator<CreateRentCommand>, CreateRentCommandValidator>();
        builder.Services.AddScoped<IValidator<UpdateRentActivityCommand>, UpdateRentActivityCommandValidator>();
        builder.Services.AddScoped<IValidator<DeleteRentCommand>, DeleteRentCommandValidator>();

        //PostgreSQL / EF Core configuration
        builder.Services.AddDbContext<CarRentingDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

        // Register repositories and services
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
        builder.Services.AddScoped<IRentRepository, RentRepository>();

        // Add controllers to the container. No transform attributes to lowercase.
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

            using var scope = app.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<CarRentingDbContext>().Database.Migrate();
        }

        //Allow CORS policy
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}
