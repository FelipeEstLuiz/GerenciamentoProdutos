using Application.Api.Extensions;
using Application.Infraestructure.Data.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Application.Infraestructure.IOC;
using Hangfire;
using Hangfire.SqlServer;
using Application.Core.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
          {
              CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
              SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
              QueuePollInterval = TimeSpan.Zero,
              UseRecommendedIsolationLevel = true,
              DisableGlobalLocks = true
          });
});

builder.Services.AddHangfireServer();

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
       builder =>
       {
           builder.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
       });
});

builder.Services.ConfigureExtensions(builder.Configuration);
builder.Services.AddInfrastructure();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ProdutoJobService>();

WebApplication app = builder.Build();

if (!app.Environment.IsProduction())
    DatabaseInitializer.InitializeAsync(builder.Configuration.GetConnectionString("DefaultConnection")!).GetAwaiter().GetResult();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression();

app.UseGlobalExceptionMiddleware();

app.UseRouting()
    .UseEndpoints(r =>
    {
        r.MapControllers();
    });

// Configurar o painel do Hangfire
app.UseHangfireDashboard();

// Agendar o job
string? jobTime = builder.Configuration.GetSection("JobSettings:DailyJobTime").Value;

if (TimeSpan.TryParse(jobTime, out TimeSpan dailyJobTime))
{
    RecurringJob.AddOrUpdate<ProdutoJobService>(
        "AtualizarStatusProdutos",
        service => service.AtualizarStatusProdutos(),
        Cron.Daily(dailyJobTime.Hours, dailyJobTime.Minutes) // Executa no horário configurado
    );
}

app.Run();
