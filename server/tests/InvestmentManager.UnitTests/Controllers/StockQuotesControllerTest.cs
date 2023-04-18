using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using InvestmentManager.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InvestmentManager.UnitTests.Controllers
{
    public class StockQuotesControllerTest
    {
        private readonly Mock<IFinnhubService> _finnhubServiceMock;
        private readonly Mock<IBrApiService> _brApiServiceMock;
        private readonly StockQuotesController _sut;

        public StockQuotesControllerTest()
        {
            _finnhubServiceMock = new Mock<IFinnhubService>(MockBehavior.Strict);
            _brApiServiceMock = new Mock<IBrApiService>(MockBehavior.Strict);
            _sut = new StockQuotesController(_finnhubServiceMock.Object, _brApiServiceMock.Object);
        }

        #region GetMultipleBrStockQuotes

        [Fact]
        public async Task GetMultipleBrStockQuotes_ValidData_ToBeOk()
        {
            // Arrange
            string symbols = "PETR4,MGLU3,VALE3";

            var stockQuotesMock = new Dictionary<string, double>()
            {
                { "PETR4", 10.50 },
                { "MGLU3", 2.99 },
                { "VALE3", 80.20 }
            };

            _brApiServiceMock
                .Setup(m => m.GetStocksPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(stockQuotesMock);

            // Act
            var result = await _sut.GetMultipleBrStockQuotes(symbols);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
               .Which.Value.Should().BeOfType<StockQuotesResponse>();
        }
        #endregion

        #region GetSingleUsStockQuote

        [Fact]
        public async Task GetSingleUsStockQuote_ValidData_ToBeOk()
        {
            // Arrange
            string symbol = "AMZN";
            _finnhubServiceMock.Setup(m => m.GetStockPriceQuote(It.IsAny<string>())).ReturnsAsync(100.9);

            // Act
            var result = await _sut.GetSingleUsStockQuote(symbol);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be(100.9);
            _finnhubServiceMock.Verify(m => m.GetStockPriceQuote(symbol), Times.Once);
        }
        #endregion

        #region GetMultipleUsStockQuotes

        [Fact]
        public async Task GetMultipleUsStockQuotes_ValidData_ToBeOk()
        {
            // Arrange
            string symbols = "AMZN,VOO,VNQ";
            var stockQuotesMock = new Dictionary<string, double>()
            {
                { "AMZN", 103.05 },
                { "VOO", 379.15 },
                { "VNQ", 81.68 }
            };

            string[] expectedStockSymbols = { "AMZN", "VOO", "VNQ" };

            _finnhubServiceMock
                .Setup(m => m.GetMultipleStockPriceQuote(It.IsAny<string[]>()))
                .ReturnsAsync(stockQuotesMock);

            // Act
            var result = await _sut.GetMultipleUsStockQuotes(symbols);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<StockQuotesResponse>();
            _finnhubServiceMock.Verify(m => m.GetMultipleStockPriceQuote(expectedStockSymbols), Times.Once);
        }
        #endregion
    }
}
