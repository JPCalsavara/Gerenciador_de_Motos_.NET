using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Infrastructure.Persistence;
using MottuChallenge.API.Repositories;
using MottuChallenge.API.Services;
using MottuChallenge.API.Application.UseCases.DeliveryPeople;
using MottuChallenge.API.Services.UseCases.Motorcycles;
using MottuChallenge.API.Services.UseCases.Rentals;
using MottuChallenge.API.Workers;
using System.Reflection;
using MottuChallenge.API.Services.UseCases.DeliveryPeople;


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
builder.Services.AddScoped<IRentalRepository, RentalRepository>();

// UseCases
builder.Services.AddScoped<CreateMotorcycleUseCase>();
builder.Services.AddScoped<GetMotorcyclesUseCase>();
builder.Services.AddScoped<GetMotorcycleByIdUseCase>();
builder.Services.AddScoped<UpdateMotorcyclePlateUseCase>();
builder.Services.AddScoped<DeleteMotorcycleUseCase>();
builder.Services.AddScoped<CreateDeliveryPersonUseCase>();
builder.Services.AddScoped<GetDeliveryPeopleUseCase>();
builder.Services.AddScoped<UploadCnhUseCase>();
builder.Services.AddScoped<CreateRentalUseCase>();
builder.Services.AddScoped<UpdateRentalReturnDateUseCase>();
builder.Services.AddScoped<GetRentalByIdUseCase>();

// --- ATUALIZAÇÃO: Serviços Externos ---
// Lê as configurações da secção "AWS" do appsettings.json
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
// Registra o cliente S3
builder.Services.AddAWSService<IAmazonS3>();
// Troca a implementação do IStorageService para a versão S3
builder.Services.AddSingleton<IStorageService, S3StorageService>();
builder.Services.AddSingleton<IMessagingService, RabbitMqService>();

// Workers / Consumidores
builder.Services.AddHostedService<MotorcycleCreatedConsumer>();

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
