using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;


namespace InvestmentManager.UnitTests.Services
{
    public class FinnhubServiceTest
    {
        private readonly IFinnhubService _sut;
        private readonly Mock<IFinnhubRepository> _finnhubRepositoryMock;
        private readonly IFixture _fixture;
        private readonly Mock<ILogger<FinnhubService>> _loggerMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;


        public FinnhubServiceTest()
        {
            _finnhubRepositoryMock = new Mock<IFinnhubRepository>(MockBehavior.Strict);
            _loggerMock = new Mock<ILogger<FinnhubService>>(MockBehavior.Loose);
            _memoryCacheMock = new Mock<IMemoryCache>(MockBehavior.Loose);
            _sut = new FinnhubService(_finnhubRepositoryMock.Object, _loggerMock.Object, _memoryCacheMock.Object);
            _fixture = new Fixture();
        }

        #region GetMultipleStockPriceQuote

        [Fact]
        async public Task GetMultipleStockPriceQuote_ValidData_ToBeSuccessful()
        {
            string[] stockSymbols = { "ABC", "DEF", "GHI" };

            double ABCStockQuote = _fixture.Create<double>();
            double DEFStockQuote = _fixture.Create<double>();
            double GHIStockQuote = _fixture.Create<double>();

            List<StockQuoteResult> stockQuoteResultMock = new()
            {
                { new StockQuoteResult(){ Symbol = "ABC", Price = ABCStockQuote } },
                { new StockQuoteResult(){ Symbol = "DEF", Price = DEFStockQuote } },
                { new StockQuoteResult() { Symbol = "GHI", Price = GHIStockQuote } }
            };

            _finnhubRepositoryMock
                .SetupSequence(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(ABCStockQuote)
                .ReturnsAsync(DEFStockQuote)
                .ReturnsAsync(GHIStockQuote);

            var result = await _sut.GetMultipleStockPriceQuote(stockSymbols);

            result.Should().BeEquivalentTo(stockQuoteResultMock);
        }

        #endregion

        #region IsStockSymbolValid

        [Fact]
        public async Task IsStockSymbolValid_ValidStockSymbol_ToBeTrue()
        {
            // Arrange
            string stockSymbol = _fixture.Create<string>();
            _finnhubRepositoryMock.Setup(m => m.GetStockPriceQuote(It.IsAny<string>())).ReturnsAsync(10.9);

            // Act
            bool result = await _sut.IsStockSymbolValid(stockSymbol);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsStockSymbolValid_InvalidStockSymbol_ToBeFalse()
        {
            // Arrange
            string stockSymbol = _fixture.Create<string>();
            _finnhubRepositoryMock.Setup(m => m.GetStockPriceQuote(It.IsAny<string>())).ReturnsAsync(0);

            // Act
            bool result = await _sut.IsStockSymbolValid(stockSymbol);

            // Assert
            result.Should().BeFalse();
        }
        #endregion
    }
}
