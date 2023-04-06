using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using Moq;

namespace InvestmentManager.UnitTests.Services
{
    public class FinnhubServiceTest : IDisposable
    {
        private readonly IFinnhubService _sut;
        private readonly MockRepository _mockRepository;
        private readonly Mock<IFinnhubRepository> _finnhubRepositoryMock;
        private readonly IFixture _fixture;

        public FinnhubServiceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _finnhubRepositoryMock = new Mock<IFinnhubRepository>(MockBehavior.Strict);
            _sut = new FinnhubService(_finnhubRepositoryMock.Object);
            _fixture = new Fixture();
        }

        #region GetStockPriceQuote

        [Fact]
        async public Task GetStockPriceQuote_InvalidStockSymbol_ToBeArgumentException()
        {
            string stockSymbol = _fixture.Create<string>();
            Dictionary<string, object> invalidSymbolReturn = new Dictionary<string, object>()
            {
                { "c", 0 }
            };

            _finnhubRepositoryMock
                .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(invalidSymbolReturn);

            Func<Task> action = async () =>
            {
                await _sut.GetStockPriceQuote(stockSymbol);
            };

            await action.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid stock symbol");
        }

        [Fact]
        async public Task GetStockPriceQuote_ValidData_ToBeSuccessful()
        {
            string stockSymbol = _fixture.Create<string>();
            Dictionary<string, object> validSymbolReturn = new Dictionary<string, object>()
            {
                { "c", 199.90 }
            };

            _finnhubRepositoryMock
                .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(validSymbolReturn);

            double stockQuote = await _sut.GetStockPriceQuote(stockSymbol);

            stockQuote.Should().Be((double)validSymbolReturn["c"]);
        }

        #endregion
  

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }
    }
}
