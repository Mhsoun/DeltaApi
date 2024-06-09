using DeltaApi.Models;
using DeltaApi.Repositories;
using DeltaApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ExchangeRateApiSettings>(builder.Configuration.GetSection("ExchangeRateApi"));

builder.Services.AddScoped<ICurrencyDeltaService, CurrencyDeltaService>();
builder.Services.AddScoped<ICurrencyApiRepository, CurrencyApiRepository>();

builder.Services.AddHttpClient<ICurrencyApiRepository, CurrencyApiRepository>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var settings = configuration.GetSection("ExchangeRateApi").Get<ExchangeRateApiSettings>();

    if (settings == null)
    {
        throw new InvalidOperationException("ExchangeRateApi settings not found.");
    }

    if (string.IsNullOrWhiteSpace(settings.BaseUrl))
    {
        throw new InvalidOperationException("BaseUrl for ExchangeRateApi is not configured.");
    }

    if (string.IsNullOrWhiteSpace(settings.ApiKey))
    {
        throw new InvalidOperationException("ApiKey for ExchangeRateApi is not configured.");
    }

    client.BaseAddress = new Uri(settings.BaseUrl);
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.ApiKey}");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
