using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Enums;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using Moq;
using AutoMapper;
using InvestmentManager.ApplicationCore.Mapper;

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

        // ADD TEST CASE WHEN SYMBOL IS INVALID !!!!

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
            StockPositionResponse stockPositionResponse = await _sut.CreateStockPosition(addStockPositionRequest);

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

        //#region GetAllStockPositions

        //[Fact]
        //public async Task GetAllStockPositions_NoStockPositions_ToBeEmptyList()
        //{
        //    List<StockPosition> stockPositions = new List<StockPosition>();

        //    _stockPositionRepositoryMock
        //        .Setup(temp => temp.GetAllStockPositions())
        //        .ReturnsAsync(stockPositions);

        //    List<StockPositionResponse> stockPositionResponse = await _sut.GetAllStockPositions();

        //    stockPositionResponse.Should().BeEmpty();
        //}

        //[Fact]
        //public async Task GetAllStockPosition_WithStockPositions_ToBeSuccessful()
        //{
        //    Guid positionId1 = _fixture.Create<Guid>();
        //    Guid positionId2 = _fixture.Create<Guid>();
        //    Guid positionId3 = _fixture.Create<Guid>();

        //    List<StockPosition> stockPositionsMock = new List<StockPosition>()
        //    {
        //        _fixture.Build<StockPosition>().With(temp => temp.PositionId, positionId1).Create(),
        //        _fixture.Build<StockPosition>().With(temp => temp.PositionId, positionId2).Create(),
        //        _fixture.Build<StockPosition>().With(temp => temp.PositionId, positionId3).Create()
        //    };

        //    double mockPrice1 = _fixture.Create<double>();
        //    double mockPrice2 = _fixture.Create<double>();
        //    double mockPrice3 = _fixture.Create<double>();

        //    Dictionary<string, double> stockPriceDictMock = new Dictionary<string, double>()
        //    {
        //        { stockPositionsMock[0].Symbol, mockPrice1 },
        //        { stockPositionsMock[1].Symbol, mockPrice2 },
        //        { stockPositionsMock[2].Symbol, mockPrice3 }
        //    };

        //    _stockPositionRepositoryMock
        //        .Setup(temp => temp.GetAllStockPositions())
        //        .ReturnsAsync(stockPositionsMock);

        //    _finnhubServiceMock
        //        .Setup(temp => temp.GetMultipleStockPriceQuote(It.IsAny<List<string>>()))
        //        .ReturnsAsync(stockPriceDictMock);

        //    List<StockPositionResponse> stockPositionResponseExpected = new();

        //    foreach (KeyValuePair<string, double> entry in stockPriceDictMock)
        //    {
        //        // get index of stockPosition with the given stockSymbol
        //        int stockPositionSymbolIndex = stockPositionsMock
        //            .FindIndex(stockPosition => stockPosition.Symbol == entry.Key);

        //        stockPositionResponseExpected
        //            .Add(stockPositionsMock[stockPositionSymbolIndex]
        //            .ToStockPositionResponse(entry.Value));
        //    }

        //    List<StockPositionResponse> stockPositionResponse = await _sut.GetAllStockPositions();

        //    stockPositionResponse.Should().BeEquivalentTo(stockPositionResponseExpected);
        //}

        //#endregion

        //#region UpdateStockPosition

        //[Theory]
        //[InlineData(0)]
        //[InlineData(-10)]
        //public async Task UpdateStockPosition_QuantityIsLessThanMinimum_ToBeArgumentException(int quantity)
        //{
        //    UpdateStockPositionRequest? updateStockPositionRequest = _fixture.Build<UpdateStockPositionRequest>()
        //        .With(temp => temp.Quantity, quantity)
        //        .Create();

        //    Func<Task> action = async () =>
        //    {
        //        await _sut.UpdateStockPosition(updateStockPositionRequest);
        //    };

        //    await action.Should().ThrowAsync<ArgumentException>()
        //        .WithMessage("Quantity must be greater than 0");
        //}

        //[Theory]
        //[InlineData(0.0)]
        //[InlineData(0.001)]
        //[InlineData(-1.0)]
        //public async Task UpdateStockPosition_PriceIsLessThanMinimum_ToBeArgumentException(double price)
        //{
        //    UpdateStockPositionRequest updateStockPositionRequest = _fixture.Build<UpdateStockPositionRequest>()
        //        .With(temp => temp.Price, price)
        //        .Create();

        //    Func<Task> action = async () =>
        //    {
        //        await _sut.UpdateStockPosition(updateStockPositionRequest);
        //    };

        //    await action.Should().ThrowAsync<ArgumentException>()
        //        .WithMessage("Price must be greater than 0.01");
        //}

        //[Fact]
        //public async Task UpdateStockPosition_InvalidStockPositionId_ToBeArgumentException()
        //{
        //    Guid positionId = _fixture.Create<Guid>();

        //    UpdateStockPositionRequest updateStockPositionRequest = _fixture
        //        .Build<UpdateStockPositionRequest>()
        //        .With(temp => temp.PositionId, positionId)
        //        .Create();

        //    _stockPositionRepositoryMock
        //        .Setup(temp => temp.GetSingleStockPosition(positionId))
        //        .ReturnsAsync(null as StockPosition);

        //    Func<Task> action = async () =>
        //    {
        //        await _sut.UpdateStockPosition(updateStockPositionRequest);
        //    };

        //    await action.Should().ThrowAsync<ArgumentException>()
        //        .WithMessage("Invalid position id");
        //}

        //[Theory]
        //[InlineData(100, 10.0, 100, 30.0)]
        //[InlineData(10894, 183.20, 129132, 987324.3)]
        //[InlineData(35, 631.63, 954, 345.32)]
        //public async Task UpdateStockPosition_BuyTransaction_ToBeSuccessful(
        //    int newQuantity,
        //    double newPrice,
        //    int currentQuantity,
        //    double currentPrice
        //)
        //{
        //    Guid positionId = _fixture.Create<Guid>();

        //    UpdateStockPositionRequest updateStockPositionRequest = _fixture
        //        .Build<UpdateStockPositionRequest>()
        //        .With(temp => temp.PositionId, positionId)
        //        .With(temp => temp.Quantity, newQuantity)
        //        .With(temp => temp.Price, newPrice)
        //        .With(temp => temp.TransactionType, TransactionType.Buy)
        //        .Create();

        //    StockPosition matchingStockPosition = _fixture
        //        .Build<StockPosition>()
        //        .With(temp => temp.PositionId, positionId)
        //        .With(temp => temp.Quantity, currentQuantity)
        //        .With(temp => temp.AveragePrice, currentPrice)
        //        .Create();

        //    double stockPriceMock = _fixture.Create<double>();

        //    _stockPositionRepositoryMock
        //        .Setup(temp => temp.GetSingleStockPosition(It.IsAny<Guid>()))
        //        .ReturnsAsync(matchingStockPosition);

        //    _stockPositionRepositoryMock
        //        .Setup(temp => temp.UpdateStockPosition(It.IsAny<StockPosition>()))
        //        .Returns(Task.CompletedTask);

        //    _finnhubServiceMock
        //       .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
        //       .ReturnsAsync(stockPriceMock);

        //    StockPositionResponse? updatedStockPositionResponse = await _sut.UpdateStockPosition(updateStockPositionRequest);

        //    int quantityExpected = newQuantity + currentQuantity;
        //    double averagePriceExpected = ((currentQuantity * currentPrice) + (newQuantity * newPrice)) / (currentQuantity + newQuantity);

        //    // debuggar para entender o pq nao funciona usando o metodo !!!!!!!!!!
        //    updatedStockPositionResponse?.Quantity.Should().Be(quantityExpected);
        //    updatedStockPositionResponse?.AveragePrice.Should().Be(averagePriceExpected);
        //    updatedStockPositionResponse?.Symbol.Should().Be(matchingStockPosition.Symbol);
        //    updatedStockPositionResponse?.Cost.Should().Be(matchingStockPosition.Cost);
        //    updatedStockPositionResponse?.PositionId.Should().Be(matchingStockPosition.PositionId);
        //}

        //[Theory]
        //[InlineData(5, 1.0, 15, 15.99)]
        //[InlineData(1000, 100.0, 8500, 99.42)]
        //[InlineData(10000, 784.49, 100003, 84.99)]
        //public async Task UpdateStockPosition_SellTransactionValidData_ToBeSuccessful(
        //    int newQuantity,
        //    double newPrice,
        //    int currentQuantity,
        //    double currentPrice
        //)
        //{
        //    Guid positionId = _fixture.Create<Guid>();

        //    UpdateStockPositionRequest updateStockPositionRequest = _fixture
        //        .Build<UpdateStockPositionRequest>()
        //        .With(temp => temp.PositionId, positionId)
        //        .With(temp => temp.Quantity, newQuantity)
        //        .With(temp => temp.Price, newPrice)
        //        .With(temp => temp.TransactionType, TransactionType.Sell)
        //        .Create();

        //    StockPosition matchingStockPosition = _fixture
        //        .Build<StockPosition>()
        //        .With(temp => temp.PositionId, positionId)
        //        .With(temp => temp.Quantity, currentQuantity)
        //        .With(temp => temp.AveragePrice, currentPrice)
        //        .Create();

        //    double stockPriceMock = _fixture.Create<double>();

        //    _stockPositionRepositoryMock
        //        .Setup(temp => temp.GetSingleStockPosition(It.IsAny<Guid>()))
        //        .ReturnsAsync(matchingStockPosition);

        //    _stockPositionRepositoryMock
        //        .Setup(temp => temp.UpdateStockPosition(It.IsAny<StockPosition>()))
        //        .Returns(Task.CompletedTask);

        //    _finnhubServiceMock
        //       .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
        //       .ReturnsAsync(stockPriceMock);

        //    StockPositionResponse? updatedStockPositionResponse = await _sut.UpdateStockPosition(updateStockPositionRequest);

        //    int quantityExpected = currentQuantity - newQuantity;

        //    updatedStockPositionResponse?.Quantity.Should().Be(quantityExpected);
        //    updatedStockPositionResponse?.AveragePrice.Should().Be(matchingStockPosition.AveragePrice);
        //    updatedStockPositionResponse?.Symbol.Should().Be(matchingStockPosition.Symbol);
        //    updatedStockPositionResponse?.Cost.Should().Be(matchingStockPosition.Cost);
        //    updatedStockPositionResponse?.PositionId.Should().Be(matchingStockPosition.PositionId);
        //}

        //[Theory]
        //[InlineData(100, 1.0, 5, 15.99)]
        //[InlineData(1000, 100.0, 850, 99.42)]
        //[InlineData(10000, 784.49, 1, 84.99)]
        //public async Task UpdateStockPosition_SellTransactionGreaterQuantity_ToBeSuccessful(
        //    int newQuantity,
        //    double newPrice,
        //    int currentQuantity,
        //    double currentPrice
        //)
        //{
        //    Guid positionId = _fixture.Create<Guid>();

        //    UpdateStockPositionRequest updateStockPositionRequest = _fixture
        //        .Build<UpdateStockPositionRequest>()
        //        .With(temp => temp.PositionId, positionId)
        //        .With(temp => temp.Quantity, newQuantity)
        //        .With(temp => temp.Price, newPrice)
        //        .With(temp => temp.TransactionType, TransactionType.Sell)
        //        .Create();

        //    StockPosition matchingStockPosition = _fixture
        //        .Build<StockPosition>()
        //        .With(temp => temp.PositionId, positionId)
        //        .With(temp => temp.Quantity, currentQuantity)
        //        .With(temp => temp.AveragePrice, currentPrice)
        //        .Create();

        //    double stockPriceMock = _fixture.Create<double>();

        //    _stockPositionRepositoryMock
        //        .Setup(temp => temp.GetSingleStockPosition(It.IsAny<Guid>()))
        //        .ReturnsAsync(matchingStockPosition);

        //    _stockPositionRepositoryMock
        //        .Setup(temp => temp.UpdateStockPosition(It.IsAny<StockPosition>()))
        //        .Returns(Task.CompletedTask);

        //    _finnhubServiceMock
        //       .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
        //       .ReturnsAsync(stockPriceMock);

        //    Func<Task> action = async () =>
        //    {
        //        await _sut.UpdateStockPosition(updateStockPositionRequest);
        //    };

        //    await action.Should().ThrowAsync<ArgumentException>()
        //        .WithMessage("The stock quantity to be sold is greater than the current stock position quantity.");
        //}

        //#endregion
    }

}
