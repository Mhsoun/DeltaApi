namespace DeltaApi.Repositories;
public interface ICurrencyApiRepository
{
    Task<Dictionary<string, decimal>> GetHistoricalRates(DateTime date, string baseCurrency);
}
