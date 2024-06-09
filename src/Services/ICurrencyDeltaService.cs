using DeltaApi.DTOs;
using DeltaApi.Models;

namespace DeltaApi.Services;
public interface ICurrencyDeltaService
{
    Task<List<CurrencyDeltaResponseDto>> GetCurrencyDeltas(CurrencyDeltaRequest request);
}
