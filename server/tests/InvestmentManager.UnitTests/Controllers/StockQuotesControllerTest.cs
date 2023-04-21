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

            var stockQuotesMock = new List<StockQuoteResult>()
            {
                { new StockQuoteResult(){ Symbol = "PETR4", Price = 10.50 } },
                { new StockQuoteResult(){ Symbol = "MGLU3", Price = 2.99 } },
                { new StockQuoteResult() { Symbol = "VALE3", Price = 80.20 } }
            };

            _brApiServiceMock
                .Setup(m => m.GetStocksPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(stockQuotesMock);

            // Act
            var result = await _sut.GetBrStockQuotes(symbols);

            // Assert
            result.Should().BeOfType<ActionResult<StockQuotesResponse>>();
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult?.Value.Should().BeOfType<StockQuotesResponse>();
        }
        #endregion

        #region GetUsStockQuotes

        [Fact]
        public async Task GetUsStockQuotes_ValidData_ToBeOk()
        {
            // Arrange
            string symbols = "AMZN,VOO,VNQ";
            var stockQuotesMock = new List<StockQuoteResult>()
            {
                { new StockQuoteResult(){ Symbol = "AMZN", Price = 100.50 } },
                { new StockQuoteResult(){ Symbol = "VOO", Price = 372.99 } },
                { new StockQuoteResult() { Symbol = "BK", Price = 40.20 } }
            };

            string[] expectedStockSymbols = { "AMZN", "VOO", "VNQ" };

            _finnhubServiceMock
                .Setup(m => m.GetMultipleStockPriceQuote(It.IsAny<string[]>()))
                .ReturnsAsync(stockQuotesMock);

            // Act
            var result = await _sut.GetUsStockQuotes(symbols);

            // Assert
            result.Should().BeOfType<ActionResult<StockQuotesResponse>>();
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult?.Value.Should().BeOfType<StockQuotesResponse>();
            _finnhubServiceMock.Verify(m => m.GetMultipleStockPriceQuote(expectedStockSymbols), Times.Once);
        }
        #endregion
    }
}
