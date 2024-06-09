namespace DeltaApi.DTOs; 

public class CurrencyDeltaResponseDto
{
    public string Currency { get; set; } = string.Empty;
    public decimal Delta { get; set; }
}
