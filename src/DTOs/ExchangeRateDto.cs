namespace DeltaApi.DTOs;

public class ExchangeRateDto
{
    public string Result { get; set; } = string.Empty;
    public string BaseCode { get; set; } = string.Empty;
    public Dictionary<string, decimal> ConversionRates { get; set; } = new Dictionary<string, decimal>();
}