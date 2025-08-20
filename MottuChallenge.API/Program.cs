using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Infrastructure.Persistence;
using MottuChallenge.API.Repositories;
using MottuChallenge.API.Services;
using MottuChallenge.API.Services.UseCases.DeliveryPeople;
using MottuChallenge.API.Services.UseCases.Motorcycles;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURAÇÃO DO BANCO DE DADOS ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// --- SERVIÇOS DA APLICAÇÃO ---
builder.Services.AddControllers();

// Repositórios
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();

// UseCases - Motos
builder.Services.AddScoped<CreateMotorcycleUseCase>();
builder.Services.AddScoped<GetMotorcyclesUseCase>();
builder.Services.AddScoped<GetMotorcycleByIdUseCase>();
builder.Services.AddScoped<UpdateMotorcyclePlateUseCase>();
builder.Services.AddScoped<DeleteMotorcycleUseCase>();

// UseCases - Entregadores
builder.Services.AddScoped<CreateDeliveryPersonUseCase>();
builder.Services.AddScoped<UploadCnhUseCase>();

// Serviços Externos (Storage Local)
builder.Services.AddSingleton<IStorageService, LocalStorageService>();

// --- CONFIGURAÇÃO DO SWAGGER ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
});

// --- CONSTRUÇÃO E EXECUÇÃO DA API ---
var app = builder.Build();

// Aplica migrações do banco ao iniciar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DefaultModelsExpandDepth(-1);
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();