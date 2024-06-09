using DeltaApi.DTOs;
using DeltaApi.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace DeltaApi.Repositories;

public class CurrencyApiRepository : ICurrencyApiRepository
{
    private readonly HttpClient _httpClient;
    private readonly ExchangeRateApiSettings _settings;

    public CurrencyApiRepository(HttpClient httpClient, IOptions<ExchangeRateApiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<Dictionary<string, decimal>> GetHistoricalRates(DateTime date, string baseCurrency)
    {
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/{_settings.ApiKey}/history/{baseCurrency}/{date.Year}/{date.Month}/{date.Day}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var exchangeRate = JsonConvert.DeserializeObject<ExchangeRateDto>(content);

        if (exchangeRate == null || exchangeRate.ConversionRates == null)
        {
            throw new InvalidOperationException("Failed to retrieve exchange rates from the API.");
        }

        return exchangeRate.ConversionRates;
    }
}