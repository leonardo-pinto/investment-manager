using FluentAssertions;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using Moq;

namespace InvestmentManager.UnitTests.Services
{
    public class BrApiServiceTest
    {
        private readonly BrApiService _sut;
        private readonly Mock<IBrApiRepository> _brApiRepositoryMock;

        public BrApiServiceTest()
        {
            _brApiRepositoryMock = new Mock<IBrApiRepository>(MockBehavior.Strict);
            _sut = new BrApiService(_brApiRepositoryMock.Object);
        }

        #region IsStockSymbolValid
        [Fact]
        public async Task IsStockSymbolValid_ValidSymbol_ToBeTrue()
        {
            // Arrange
            string stockSymbol = "BBAS3";

            var brApiResponseMock = new BrApiResponse()
            {
                Results = new Result[]
                {
                    new Result() { Symbol = stockSymbol, RegularMarketPrice = 50.1 }
                }
            };

            _brApiRepositoryMock
                .Setup(m => m.GetStocksPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(brApiResponseMock);

            // Act
            bool result = await _sut.IsStockSymbolValid(stockSymbol);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsStockSymbolValid_InvalidSymbol_ToBeFalse()
        {
            // Arrange
            string stockSymbol = "BBAS3";

            var brApiResponseMock = new BrApiResponse()
            {
                Error = "any error"
            };

            _brApiRepositoryMock
                .Setup(m => m.GetStocksPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(brApiResponseMock);

            // Act
            bool result = await _sut.IsStockSymbolValid(stockSymbol);

            // Assert
            result.Should().BeFalse();
        }
        #endregion

        #region GetStocksPriceQuote
        [Fact]

        public async Task GetStocksPriceQuote_ValidData_ToBeSuccessful()
        {
            // Arrange
            string concatenatedStockSymbols = "PETR4,MGLU3,VALE3";
            var brApiResponseMock = new BrApiResponse()
            {
                Results = new Result[]
                {
                    new Result() { Symbol = "PETR4", RegularMarketPrice = 10.50 },
                    new Result() { Symbol = "MGLU3", RegularMarketPrice = 2.99 },
                    new Result() { Symbol = "VALE3", RegularMarketPrice = 80.20 }
                }
            };

            _brApiRepositoryMock
               .Setup(m => m.GetStocksPriceQuote(It.IsAny<string>()))
               .ReturnsAsync(brApiResponseMock);

            // Act
            var result = await _sut.GetStocksPriceQuote(concatenatedStockSymbols);

            // Assert
            result.Should().HaveCount(3);
            result.Should().BeOfType<List<StockQuoteResult>>();
            result[0].Symbol.Should().Be("PETR4");
            result[1].Symbol.Should().Be("MGLU3");
            result[2].Symbol.Should().Be("VALE3");
            result[0].Price.Should().Be(10.50);
            result[1].Price.Should().Be(2.99);
            result[2].Price.Should().Be(80.20);
        }
        #endregion
    }
}
