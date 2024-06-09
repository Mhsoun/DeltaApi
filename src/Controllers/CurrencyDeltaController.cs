using DeltaApi.DTOs;
using DeltaApi.Models;
using DeltaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeltaApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CurrencyDeltaController : ControllerBase
{
    private readonly ICurrencyDeltaService _currencyDeltaService;

    public CurrencyDeltaController(ICurrencyDeltaService currencyDeltaService)
    {
        _currencyDeltaService = currencyDeltaService;
    }

    [HttpPost]
    public async Task<IActionResult> GetCurrencyDelta([FromBody] CurrencyDeltaRequest request)
    {
        try
        {
            var result = await _currencyDeltaService.GetCurrencyDeltas(request);
            return Ok(result);
        }
        catch (CurrencyDeltaException ex)
        {
            return BadRequest(new ErrorResponse { ErrorCode = ex.ErrorCode, ErrorDetails = ex.Message });
        }
    }
}