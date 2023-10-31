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
        private readonly Mock<IStockQuoteService> _stockQuoteServiceMock;
        private readonly StockQuotesController _sut;

        public StockQuotesControllerTest()
        {
            _stockQuoteServiceMock = new Mock<IStockQuoteService>(MockBehavior.Strict);
            _sut = new StockQuotesController(_stockQuoteServiceMock.Object);
        }

        #region GetMultipleBrStockQuotes

        [Fact]
        public async Task GetStockQuotes_ValidData_ToBeOk()
        {
            // Arrange
            string symbols = "PETR4,MGLU3,VALE3";

            var stockQuotesMock = new List<StockQuoteResult>()
            {
                { new StockQuoteResult(){ Symbol = "PETR4", Price = 10.50 } },
                { new StockQuoteResult(){ Symbol = "MGLU3", Price = 2.99 } },
                { new StockQuoteResult() { Symbol = "VALE3", Price = 80.20 } }
            };

            _stockQuoteServiceMock
                .Setup(m => m.GetStockPriceQuote(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(stockQuotesMock);

            // Act
            var result = await _sut.GetStockQuotes(symbols, "BR");

            // Assert
            result.Should().BeOfType<ActionResult<StockQuotesResponse>>();
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult?.Value.Should().BeOfType<StockQuotesResponse>();
        }
        #endregion
    }
}
