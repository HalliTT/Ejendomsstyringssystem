using ESS.Domain.Owners;
using ESS.Domain.Properties;
using ESS.Infrastructure;
using ESS.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

        var property = db.Properties
                    .AsNoTracking()
                    .FirstOrDefault();

        if (property is null)
        {
            Guid ownerId = Guid.NewGuid();
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

            var newProperty = new Property
            {
                Id = Guid.NewGuid(),
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

        db.SaveChanges();
    }
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.MapControllers();

app.Run();