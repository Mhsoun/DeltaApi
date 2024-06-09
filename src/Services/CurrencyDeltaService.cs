using DeltaApi.DTOs;
using DeltaApi.Models;
using DeltaApi.Repositories;

namespace DeltaApi.Services;


public class CurrencyDeltaService : ICurrencyDeltaService
{
    private readonly ICurrencyApiRepository _currencyApiRepository;

    public CurrencyDeltaService(ICurrencyApiRepository currencyApiRepository)
    {
        _currencyApiRepository = currencyApiRepository;
    }

    public async Task<List<CurrencyDeltaResponseDto>> GetCurrencyDeltas(CurrencyDeltaRequest request)
    {
        ValidateRequest(request);

        var fromRates = await _currencyApiRepository.GetHistoricalRates(request.FromDate, request.Baseline);
        var toRates = await _currencyApiRepository.GetHistoricalRates(request.ToDate, request.Baseline);

        var deltas = new List<CurrencyDeltaResponseDto>();
        foreach (var currency in request.Currencies)
        {
            if (fromRates.TryGetValue(currency, out var fromRate) && toRates.TryGetValue(currency, out var toRate))
            {
                var delta = toRate - fromRate;
                deltas.Add(new CurrencyDeltaResponseDto { Currency = currency, Delta = Math.Round(delta, 3) });
            }
            else
            {
                throw new CurrencyDeltaException("currencyproblem", $"Currency {currency} does not exist.");
            }
        }
        return deltas;
    }

    private static void ValidateRequest(CurrencyDeltaRequest request)
    {
        if (request == null)
        {
            throw new CurrencyDeltaException("requestnull", "Request cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(request.Baseline))
        {
            throw new CurrencyDeltaException("baselinenull", "Baseline currency cannot be null or empty.");
        }

        if (request.Currencies == null || !request.Currencies.Any())
        {
            throw new CurrencyDeltaException("currenciesnull", "Currencies list cannot be null or empty.");
        }

        if (request.Currencies.Contains(request.Baseline))
        {
            throw new CurrencyDeltaException("currencyproblem", "Currencies must not contain the baseline currency.");
        }

        if (request.Currencies.Distinct().Count() != request.Currencies.Count)
        {
            throw new CurrencyDeltaException("currencyproblem", "Currencies must be unique.");
        }

        if (request.FromDate == default || request.ToDate == default)
        {
            throw new CurrencyDeltaException("dateproblem", "Dates must be in correct format and not default values.");
        }

        if (request.FromDate >= request.ToDate)
        {
            throw new CurrencyDeltaException("dateproblem", "To date must be greater than from date.");
        }
    }
}