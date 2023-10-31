
using FluentAssertions;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Exceptions;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using InvestmentManager.Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace InvestmentManager.UnitTests.Services
{
    public class StockQuoteServiceTest
    {
        private readonly Mock<IRepositoryFactory> _repositoryFactoryMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly Mock<ILogger<StockQuoteService>> _loggerMock;
        private readonly Mock<IStockQuoteRepository> _stockQuoteRepositoryMock;
        private readonly StockQuoteService _sut;

        public StockQuoteServiceTest()
        {
            _repositoryFactoryMock = new Mock<IRepositoryFactory>(MockBehavior.Loose);
            _memoryCacheMock = new Mock<IMemoryCache>(MockBehavior.Loose);
            _loggerMock = new Mock<ILogger<StockQuoteService>>(MockBehavior.Loose);
            _sut = new StockQuoteService(_repositoryFactoryMock.Object, _loggerMock.Object, _memoryCacheMock.Object);
            _stockQuoteRepositoryMock = new Mock<IStockQuoteRepository>(MockBehavior.Loose);
        }

        #region GetStockPriceQuote
        [Fact]
        async public Task GetStockPriceQuote_ValidInput_ToBeSuccessful()
        {
            // Arrange
            string stockSymbols = "AAPL,AMZN,META";
            string tradingCountry = "US";

            _repositoryFactoryMock
               .Setup(m => m.CreateRepository("US"))
               .Returns(_stockQuoteRepositoryMock.Object);

            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            _stockQuoteRepositoryMock
                .SetupSequence(m => m.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(170.44)
                .ReturnsAsync(125.78)
                .ReturnsAsync(200.56);

            List<StockQuoteResult> stockQuoteResultMock = new()
            {
                { new StockQuoteResult(){ Symbol = "AAPL", Price = 170.44 } },
                { new StockQuoteResult(){ Symbol = "AMZN", Price = 125.78 } },
                { new StockQuoteResult(){ Symbol = "META", Price = 200.56 } }
            };

            // Act
            var result = await _sut.GetStockPriceQuote(stockSymbols, tradingCountry);

            // Assert
            result.Should().BeEquivalentTo(stockQuoteResultMock);
        }
        #endregion

        #region IsStockSymbolValid
        [Fact]
        public async Task IsStockSymbolValid_ValidSymbol_ToBeTrue()
        {
            // Arrange
            string symbol = "AAPL";
            string tradingCountry = "US";

            _repositoryFactoryMock
                .Setup(m => m.CreateRepository("US"))
                .Returns(_stockQuoteRepositoryMock.Object);

            _stockQuoteRepositoryMock
                .Setup(m => m.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(35.66);

            // Act
            bool result = await _sut.IsStockSymbolValid(symbol, tradingCountry);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsStockSymbolValid_InvalidSymbol_ToBeTrue()
        {
            // Arrange
            string symbol = "AAPL";
            string tradingCountry = "US";

            _repositoryFactoryMock
                .Setup(m => m.CreateRepository("US"))
                .Returns(_stockQuoteRepositoryMock.Object);

            _stockQuoteRepositoryMock
                .Setup(m => m.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(0);

            // Act
            bool result = await _sut.IsStockSymbolValid(symbol, tradingCountry);

            // Assert
            result.Should().BeFalse();
        }


        [Fact]
        public async Task IsStockSymbolValid_WhenRepositoryThrows_ToThrow()
        {
            // Arrange
            string symbol = "AAPL";
            string tradingCountry = "US";

            _repositoryFactoryMock
                .Setup(m => m.CreateRepository("US"))
                .Returns(_stockQuoteRepositoryMock.Object);

            _stockQuoteRepositoryMock
                .Setup(m => m.GetStockPriceQuote(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Repository Error!"));

            // Act
            Func<Task> action = async () =>
            {
                await _sut.IsStockSymbolValid(symbol, tradingCountry);
            };

            // Assert
            await action.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Unable to retrieve stock price quote.");
        }
        #endregion
    }
}
