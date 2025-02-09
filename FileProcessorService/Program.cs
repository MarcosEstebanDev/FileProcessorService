using FileProcessorService.Data;  // Importamos la capa de datos
using FileProcessorService.Services; // Importamos el espacio de nombres donde está la clase del servicio de procesamiento de archivos
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Conexión a SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 🔹 Configuración de Hangfire
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

// 🔹 Registro del servicio de procesamiento de archivos
builder.Services.AddTransient<FileProcessorService.Services.FileProcessorService>(); // Asegúrate de usar la ruta completa

// Servicios básicos
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Aplicar migraciones automáticas al iniciar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// 🔹 Configuración de Hangfire Dashboard
app.UseHangfireDashboard();

// 🔹 Programar tarea de procesamiento cada 5 minutos
RecurringJob.AddOrUpdate<FileProcessorService.Services.FileProcessorService>(
    "file-processing-job",
    service => service.ProcessFiles(),
    "*/5 * * * *"  // Expresión Cron para cada 5 minutos
);

// Configuración de Swagger y HTTPS
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
