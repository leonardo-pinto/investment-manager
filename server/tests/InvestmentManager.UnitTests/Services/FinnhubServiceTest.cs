using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Exceptions;
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
        async public Task GetStockPriceQuote_WhenFinnhubRepositoryThrows_ToBeFinnhubException()
        {
            string stockSymbol = _fixture.Create<string>();

            _finnhubRepositoryMock
                .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("No response from server"));

            Func<Task> action = async () =>
            {
                await _sut.GetStockPriceQuote(stockSymbol);
            };

            await action
                .Should()
                .ThrowAsync<FinnhubException>()
                .WithMessage($"Unable to retrieve stock price quote.");
        }


        [Fact]
        async public Task GetStockPriceQuote_ValidData_ToBeSuccessful()
        {
            string stockSymbol = _fixture.Create<string>();
            double validSymbolReturn = _fixture.Create<double>();

            _finnhubRepositoryMock
                .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(validSymbolReturn);

            double stockQuote = await _sut.GetStockPriceQuote(stockSymbol);

            stockQuote.Should().Be(validSymbolReturn);
        }

        #endregion 

        #region GetMultipleStockPriceQuote

        [Fact]
        async public Task GetMultipleStockPriceQuote_ValidData_ToBeSuccessful()
        {
            List<string> stockSymbols = new()
            {
                "ABC", "DEF", "GHI"
            };

            double ABCStockQuote = _fixture.Create<double>();
            double DEFStockQuote = _fixture.Create<double>();
            double GHIStockQuote = _fixture.Create<double>();

            Dictionary<string, double> stockPricesDictExpected = new()
            {
                { "ABC", ABCStockQuote },
                { "DEF", DEFStockQuote },
                { "GHI", GHIStockQuote }
            };

            _finnhubRepositoryMock
                .SetupSequence(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(ABCStockQuote)
                .ReturnsAsync(DEFStockQuote)
                .ReturnsAsync(GHIStockQuote);

            Dictionary<string, double> stockPricesDict = await _sut.GetMultipleStockPriceQuote(stockSymbols);

            stockPricesDict.Should().BeEquivalentTo(stockPricesDictExpected);
        }

        #endregion
        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }
    }
}
