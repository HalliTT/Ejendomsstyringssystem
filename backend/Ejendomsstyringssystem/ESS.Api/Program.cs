using ESS.Api.Security;
using ESS.Domain.Owners;
using ESS.Domain.Properties;
using ESS.Infrastructure;
using ESS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("AuthService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuthService:BaseUrl"]!);
});

builder.Services.AddAuthentication("EssBearer").AddScheme<AuthenticationSchemeOptions, EssBearerAuthenticationHandler>("EssBearer", _ => { });
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ESS.Application.Properties.List.Query).Assembly));

builder.Services.AddInfrastructure(builder.Configuration);

// Config CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3001")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var db = scope.ServiceProvider.GetRequiredService<EssDbContext>();
        db.Database.Migrate();

        var owner = db.Owners
                        .AsNoTracking()
                        .FirstOrDefault();

        Guid ownerId = Guid.Parse("aebf22f2-b076-493c-bda0-e27ac102187d");
        if (owner is null)
        {
            var newOwner = new Owner
            {
                Id = ownerId,
                Name = "Anton",
                Email = "Anton@Anton.com",
                IsEnabled = true,
                SoftDeletedAt = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            db.Owners.Add(newOwner);
        }
        else
        {
            ownerId = owner.Id;
        }

        var property = db.Properties
                    .AsNoTracking()
                    .FirstOrDefault();

        Guid propertyId = Guid.NewGuid();
        if (property is null)
        {
            var newProperty = new Property
            {
                Id = propertyId,
                OwnerId = ownerId,
                Name = "Synstrup",
                Address = "Langå vej 33",
                City = "Langå",
                Country = "Denmark",
                Description = "My Property",
                IsEnabled = true,
                SoftDeletedAt = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            db.Properties.Add(newProperty);
        }
        else
        {
            propertyId = property.Id;
        }

        var unit = db.Unit
                    .AsNoTracking()
                    .FirstOrDefault();

        Guid unitId = Guid.NewGuid();
        if (unit is null)
        {
            var newUnit = new ESS.Domain.Units.Unit
            {
                Id = unitId,
                PropertyId = propertyId,
                Name = "First Floor",
                Description = "This is a floor",
                IsEnabled = true,
                SoftDeletedAt = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            db.Unit.Add(newUnit);
        }
        db.SaveChanges();
    }
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();