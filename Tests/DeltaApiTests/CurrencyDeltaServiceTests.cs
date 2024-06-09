using DeltaApi.Models;
using DeltaApi.Repositories;
using DeltaApi.Services;
using Moq;

[TestFixture]
public class CurrencyDeltaServiceTests
{
    private Mock<ICurrencyApiRepository> _mockRepo;
    private ICurrencyDeltaService _currencyDeltaService;

    [SetUp]
    public void SetUp()
    {
        _mockRepo = new Mock<ICurrencyApiRepository>();
        _currencyDeltaService = new CurrencyDeltaService(_mockRepo.Object);
    }

    [Test]
    public async Task GetCurrencyDeltas_ValidRequest_ReturnsDeltas()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetHistoricalRates(It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(new Dictionary<string, decimal> { { "USD", 1.2m }, { "SEK", 10m } });

        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = new List<string> { "USD", "SEK" },
            FromDate = new DateTime(2021, 9, 1),
            ToDate = new DateTime(2022, 8, 30)
        };

        // Act
        var result = await _currencyDeltaService.GetCurrencyDeltas(request);

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.IsTrue(result.Exists(r => r.Currency == "USD" && r.Delta == 0m));
        Assert.IsTrue(result.Exists(r => r.Currency == "SEK" && r.Delta == 0m));
    }

    [Test]
    public void GetCurrencyDeltas_NullRequest_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<CurrencyDeltaException>(async () => await _currencyDeltaService.GetCurrencyDeltas(null));
        Assert.AreEqual("requestnull", ex.ErrorCode);
        Assert.AreEqual("Request cannot be null.", ex.Message);
    }

    [Test]
    public void GetCurrencyDeltas_NullBaseline_ThrowsException()
    {
        // Arrange
        var request = new CurrencyDeltaRequest
        {
            Baseline = null,
            Currencies = new List<string> { "USD", "SEK" },
            FromDate = new DateTime(2021, 9, 1),
            ToDate = new DateTime(2022, 8, 30)
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<CurrencyDeltaException>(async () => await _currencyDeltaService.GetCurrencyDeltas(request));
        Assert.AreEqual("baselinenull", ex.ErrorCode);
        Assert.AreEqual("Baseline currency cannot be null or empty.", ex.Message);
    }

    [Test]
    public void GetCurrencyDeltas_NullCurrencies_ThrowsException()
    {
        // Arrange
        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = null,
            FromDate = new DateTime(2021, 9, 1),
            ToDate = new DateTime(2022, 8, 30)
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<CurrencyDeltaException>(async () => await _currencyDeltaService.GetCurrencyDeltas(request));
        Assert.AreEqual("currenciesnull", ex.ErrorCode);
        Assert.AreEqual("Currencies list cannot be null or empty.", ex.Message);
    }

    [Test]
    public void GetCurrencyDeltas_EmptyCurrencies_ThrowsException()
    {
        // Arrange
        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = new List<string>(),
            FromDate = new DateTime(2021, 9, 1),
            ToDate = new DateTime(2022, 8, 30)
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<CurrencyDeltaException>(async () => await _currencyDeltaService.GetCurrencyDeltas(request));
        Assert.AreEqual("currenciesnull", ex.ErrorCode);
        Assert.AreEqual("Currencies list cannot be null or empty.", ex.Message);
    }

    [Test]
    public void GetCurrencyDeltas_DuplicateCurrencies_ThrowsException()
    {
        // Arrange
        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = new List<string> { "USD", "USD" },
            FromDate = new DateTime(2021, 9, 1),
            ToDate = new DateTime(2022, 8, 30)
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<CurrencyDeltaException>(async () => await _currencyDeltaService.GetCurrencyDeltas(request));
        Assert.AreEqual("currencyproblem", ex.ErrorCode);
        Assert.AreEqual("Currencies must be unique.", ex.Message);
    }

    [Test]
    public void GetCurrencyDeltas_InvalidDates_ThrowsException()
    {
        // Arrange
        var request = new CurrencyDeltaRequest
        {
            Baseline = "GBP",
            Currencies = new List<string> { "USD", "SEK" },
            FromDate = new DateTime(2022, 8, 30),
            ToDate = new DateTime(2021, 9, 1)
        };

        // Act & Assert
        var ex = Assert.ThrowsAsync<CurrencyDeltaException>(async () => await _currencyDeltaService.GetCurrencyDeltas(request));
        Assert.AreEqual("dateproblem", ex.ErrorCode);
        Assert.AreEqual("To date must be greater than from date.", ex.Message);
    }
}