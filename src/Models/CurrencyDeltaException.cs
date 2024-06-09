namespace DeltaApi.Models;

public class CurrencyDeltaException : Exception
{
    public string ErrorCode { get; }

    public CurrencyDeltaException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}
