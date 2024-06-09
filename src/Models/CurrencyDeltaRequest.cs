using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeltaApi.Models;

public class CurrencyDeltaRequest
{
    [Required]
    public string Baseline { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public List<string> Currencies { get; set; } = new List<string>();

    [Required]
    public DateTime FromDate { get; set; }

    [Required]
    public DateTime ToDate { get; set; }
}