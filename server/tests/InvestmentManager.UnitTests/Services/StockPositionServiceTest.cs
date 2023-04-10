using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using InvestmentManager.ApplicationCore.Enums;
using Moq;
using AutoMapper;
using InvestmentManager.ApplicationCore.Mapper;
using InvestmentManager.ApplicationCore.Exceptions;
using InvestmentManager.UnitTests.TestHelpers;

namespace InvestmentManager.UnitTests.Services
{
    public class StockPositionServiceTest
    {
        private readonly IStockPositionService _sut;
        private readonly Mock<IFinnhubService> _finnhubServiceMock;
        private readonly Mock<IStockPositionRepository> _stockPositionRepositoryMock;
        private readonly IFixture _fixture;

        public StockPositionServiceTest()
        {
            MapperConfiguration? autoMapperConfig = new(cfg => cfg.AddProfile(new MappingProfile()));
            IMapper mapper = new Mapper(autoMapperConfig);
            _finnhubServiceMock = new Mock<IFinnhubService>(MockBehavior.Strict);
            _stockPositionRepositoryMock = new Mock<IStockPositionRepository>(MockBehavior.Strict);
            _sut = new StockPositionService(
                _finnhubServiceMock.Object,
                _stockPositionRepositoryMock.Object,
                mapper
             );
            _fixture = new Fixture();
        }

        #region CreateStockPosition

        [Fact]
        async public Task CreateStockPosition_InvalidStockSymbol_ToBeNull()
        {
            // Arrange
            AddStockPositionRequest addStockPositionRequest =
                _fixture.Build<AddStockPositionRequest>().Create();

            _finnhubServiceMock
                .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(0);

            // Act
            StockPositionResponse? result = await _sut.CreateStockPosition(addStockPositionRequest);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateStockPosition_ValidData_ToBeSuccessful()
        {
            // Arrage
            AddStockPositionRequest addStockPositionRequest =
                _fixture.Build<AddStockPositionRequest>().Create();

            double stockPriceMock = _fixture.Create<double>();

            _finnhubServiceMock
                .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(stockPriceMock);

            _stockPositionRepositoryMock
                  .Setup(temp => temp.CreateStockPosition(It.IsAny<StockPosition>()))
                  .Returns(Task.CompletedTask);

            // Act
            StockPositionResponse? stockPositionResponse = await _sut.CreateStockPosition(addStockPositionRequest);

            // Assert
            stockPositionResponse?.Symbol.Should().Be(addStockPositionRequest.Symbol);
            stockPositionResponse?.Quantity.Should().Be(addStockPositionRequest.Quantity);
            stockPositionResponse?.AveragePrice.Should().Be(addStockPositionRequest.AveragePrice);

            _finnhubServiceMock.Verify(m => m.GetStockPriceQuote(addStockPositionRequest.Symbol), Times.Once);
            _stockPositionRepositoryMock.Verify(m => m.CreateStockPosition(It.IsAny<StockPosition>()), Times.Once);
        }

        #endregion

        #region GetSingleStockPosition

        [Fact]
        public async Task GetSingleStockPosition_NoMatchingPosition_ToBeNull()
        {
            // Arrange
            Guid positionId = _fixture.Create<Guid>();

            _stockPositionRepositoryMock
                .Setup(temp => temp.GetSingleStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(null as StockPosition);

            // Act
            StockPositionResponse? stockPositionResponse = await _sut.GetSingleStockPosition(positionId);

            // Assert
            stockPositionResponse.Should().BeNull();
            _stockPositionRepositoryMock.Verify(m => m.GetSingleStockPosition(positionId), Times.Once);
        }

        [Fact]
        public async Task GetSingleStockPosition_MatchingPosition_ToBeSuccessful()
        {
            // Arrange
            Guid positionId = _fixture.Create<Guid>();

            StockPosition stockPositionMock = _fixture
                .Build<StockPosition>()
                .With(e => e.PositionId, positionId)
                .Create();

            double stockPriceMock = _fixture.Create<double>();

            _stockPositionRepositoryMock
                .Setup(temp => temp.GetSingleStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(stockPositionMock);

            _finnhubServiceMock
                .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(stockPriceMock);

            // Act
            StockPositionResponse? stockPositionResponse = await _sut.GetSingleStockPosition(positionId);

            // Assert
            stockPositionResponse?.PositionId.Should().Be(positionId);
            stockPositionResponse?.Symbol.Should().Be(stockPositionMock.Symbol);
            stockPositionResponse?.Quantity.Should().Be(stockPositionMock.Quantity);
            stockPositionResponse?.AveragePrice.Should().Be(stockPositionMock.AveragePrice);

            _stockPositionRepositoryMock.Verify(m => m.GetSingleStockPosition(positionId), Times.Once);
            _finnhubServiceMock.Verify(m => m.GetStockPriceQuote(stockPositionMock.Symbol), Times.Once);
        }

        #endregion

        #region GetAllStockPositions

        [Fact]
        public async Task GetAllStockPositions_NoStockPositions_ToBeEmptyList()
        {
            // Arrange
            _stockPositionRepositoryMock
                .Setup(temp => temp.GetAllStockPositions())
                .ReturnsAsync(new List<StockPosition>());

            // Act
            List<StockPositionResponse> stockPositionResponse = await _sut.GetAllStockPositions();

            // Assert
            stockPositionResponse.Should().BeEmpty();
            _stockPositionRepositoryMock.Verify(m => m.GetAllStockPositions(), Times.Once);
        }

        [Fact]
        public async Task GetAllStockPositions_WithStockPositions_ToBeSuccessful()
        {
            // Arrange
            var stockPositions = new List<StockPosition>()
            {
                _fixture.Build<StockPosition>().Create(),
                _fixture.Build<StockPosition>().Create(),
            };

            _stockPositionRepositoryMock
                .Setup(m => m.GetAllStockPositions())
                .ReturnsAsync(stockPositions);

            Dictionary<string, double> stockPriceDictMock = MockHelper.GenerateStockPriceDict(stockPositions);

            _finnhubServiceMock
                .Setup(m => m.GetMultipleStockPriceQuote(It.IsAny<List<string>>()))
                .ReturnsAsync(stockPriceDictMock);

            List<string> expectedSymbols = new()
            { stockPositions[0].Symbol, stockPositions[1].Symbol };

            // Act
            List<StockPositionResponse> stockPositionResponse = await _sut.GetAllStockPositions();

            // Assert
            stockPositionResponse[0]
                .CurrentPrice.Should().Be(stockPriceDictMock[stockPositions[0].Symbol]);
            stockPositionResponse[1]
                .CurrentPrice.Should().Be(stockPriceDictMock[stockPositions[1].Symbol]);
            _finnhubServiceMock.Verify(m => m.GetMultipleStockPriceQuote(expectedSymbols), Times.Once);
        }

        #endregion

        #region UpdateStockPriceListBySymbol

        [Fact]
        public void UpdateStockPriceListBySymbol_ToBeSuccessful()
        {
            // Arrange
            List<StockPosition> stockPositionList = new()
            {
                _fixture.Build<StockPosition>().Create(),
                _fixture.Build<StockPosition>().Create(),
                _fixture.Build<StockPosition>().Create(),
            };

            List<string> stockSymbols = new()
            {
                stockPositionList[0].Symbol,
                stockPositionList[1].Symbol,
                stockPositionList[2].Symbol,
            };

            Dictionary<string, double> stockPriceDict = new()
            {
                { stockSymbols[0], _fixture.Create<double>() },
                { stockSymbols[1], _fixture.Create<double>() },
                { stockSymbols[2], _fixture.Create<double>() }
            };

            // Act
            var result = _sut.UpdateStockPriceListBySymbol(stockPriceDict, stockPositionList);

            // Assert
            result[0].CurrentPrice.Should().Be(stockPriceDict[result[0].Symbol]);
            result[1].CurrentPrice.Should().Be(stockPriceDict[result[1].Symbol]);
            result[2].CurrentPrice.Should().Be(stockPriceDict[result[2].Symbol]);

        }
        #endregion

        #region UpdateStockPosition

        [Fact]
        public async Task UpdateStockPosition_InvalidStockPositionId_ToBeNull()
        {
            // Arrange
            UpdateStockPositionRequest updateStockPositionRequest =
                _fixture.Build<UpdateStockPositionRequest>().Create();
            Guid positionId = _fixture.Create<Guid>();

            _stockPositionRepositoryMock
                .Setup(m => m.GetSingleStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(null as StockPosition);

            // Act
            StockPositionResponse? result = await _sut.UpdateStockPosition(updateStockPositionRequest, positionId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateStockPosition_BuyTransaction_ToBeSuccessful()
        {
            // Arrange
            UpdateStockPositionRequest updateStockPositionRequest = _fixture
                .Build<UpdateStockPositionRequest>()
                .With(e => e.TransactionType, TransactionType.Buy)
                .Create();
            Guid positionId = _fixture.Create<Guid>();
            StockPosition matchingStock = _fixture
                .Build<StockPosition>()
                .Create();

            _stockPositionRepositoryMock
                .Setup(m => m.GetSingleStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(matchingStock);

            _finnhubServiceMock
                .Setup(m => m.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(_fixture.Create<double>());
            _stockPositionRepositoryMock
                .Setup(m => m.UpdateStockPosition(It.IsAny<StockPosition>()))
                .Returns(Task.CompletedTask);

            int expectedQuantity = updateStockPositionRequest.Quantity + matchingStock.Quantity;
            double expectedAvgPrice = matchingStock
                .UpdateAveragePrice(updateStockPositionRequest.Quantity, updateStockPositionRequest.Price);
            double expectedCost = expectedQuantity * expectedAvgPrice;

            // Act
            StockPositionResponse? result = await _sut.UpdateStockPosition(updateStockPositionRequest, positionId);

            // Assert
            result?.Quantity.Should().Be(expectedQuantity);
            result?.AveragePrice.Should().Be(expectedAvgPrice);
            result?.Cost.Should().Be(expectedCost);
        }

        [Fact]
        public async Task UpdateStockPosition_SellTransaction_ToBeSuccessful()
        {
            // Arrange
            UpdateStockPositionRequest updateStockPositionRequest = _fixture
                .Build<UpdateStockPositionRequest>()
                .With(e => e.TransactionType, TransactionType.Sell)
                .With(e => e.Quantity, 100)
                .Create();
            Guid positionId = _fixture.Create<Guid>();
            StockPosition matchingStock = _fixture
                .Build<StockPosition>()
                .With(e => e.Quantity, 200)
                .Create();

            _stockPositionRepositoryMock
                .Setup(m => m.GetSingleStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(matchingStock);

            _finnhubServiceMock
                .Setup(m => m.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(_fixture.Create<double>());
            _stockPositionRepositoryMock
                .Setup(m => m.UpdateStockPosition(It.IsAny<StockPosition>()))
                .Returns(Task.CompletedTask);

            int expectedQuantity = matchingStock.Quantity - updateStockPositionRequest.Quantity;
            double expectedCost = expectedQuantity * matchingStock.AveragePrice;

            // Act
            StockPositionResponse? result = await _sut.UpdateStockPosition(updateStockPositionRequest, positionId);

            // Assert
            result?.Quantity.Should().Be(expectedQuantity);
            result?.AveragePrice.Should().Be(matchingStock.AveragePrice);
            result?.Cost.Should().Be(expectedCost);
        }

        [Fact]
        async public Task UpdateStockPosition_InvalidSellQuantity_ToBeInvalidStockQuantityException()
        {
            // Arrange
            UpdateStockPositionRequest updateStockPositionRequest = _fixture
                .Build<UpdateStockPositionRequest>()
                .With(e => e.TransactionType, TransactionType.Sell)
                .With(e => e.Quantity, 200)
                .Create();
            Guid positionId = _fixture.Create<Guid>();
            StockPosition matchingStock = _fixture
                .Build<StockPosition>()
                .With(e => e.Quantity, 100)
                .Create();

            _stockPositionRepositoryMock
                .Setup(m => m.GetSingleStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(matchingStock);

            // Act
            Func<Task> action = async () =>
            {
                await _sut.UpdateStockPosition(updateStockPositionRequest, positionId);
            };

            // Assert
            await action.Should().ThrowAsync<InvalidStockQuantityException>()
                .WithMessage("The stock quantity to be sold is greater than the current stock position quantity.");
        }
        #endregion
    }

}
