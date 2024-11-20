using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using QuantityOnHand.Components;
using QuantityOnHand.Data;
using QuantityOnHand.Data.Repositories.HospitalItems;
using QuantityOnHand.Data.Utilities;
using QuantityOnHand.Domain.HospitalItem.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("QuantityOnHand.Data")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddMudServices();

builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(GetHospitalItemsPageQuery).Assembly); });

builder.Services.AddTransient<IHospitalItemRepository, HospitalItemRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    var seeder = new DatabaseSeeder(context, "../QuantityOnHand.Data/SeedData/example.txt");
    await seeder.SeedAsync();
}

app.Run();