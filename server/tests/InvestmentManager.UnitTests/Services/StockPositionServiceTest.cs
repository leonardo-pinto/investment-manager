using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.Enums;
using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Services;
using Moq;

namespace InvestmentManager.UnitTests.Services
{
    public class StockPositionServiceTest : IDisposable
    {
        private readonly IStockPositionService _sut;
        private readonly MockRepository _mockRepository;
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly Mock<IFinnhubService> _finnhubServiceMock;
        private readonly Mock<IStockPositionRepository> _stockPositionRepositoryMock;
        private readonly IFixture _fixture;

        public StockPositionServiceTest()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _transactionServiceMock = new Mock<ITransactionService>(MockBehavior.Strict);
            _finnhubServiceMock = new Mock<IFinnhubService>(MockBehavior.Strict);
            _stockPositionRepositoryMock = new Mock<IStockPositionRepository>(MockBehavior.Strict);
            _sut = new StockPositionService(
                _transactionServiceMock.Object,
                _finnhubServiceMock.Object,
                _stockPositionRepositoryMock.Object
             );
            _fixture = new Fixture();
        }

        #region CreateStockPosition
        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task CreateStockPosition_QuantityIsLessThanMinimum_ToBeArgumentException(int quantity)
        {
            AddStockPositionRequest? addStockPositionRequest = _fixture.Build<AddStockPositionRequest>()
                .With(temp => temp.Quantity, quantity)
                .Create();

            Func<Task> action = async () =>
            {
                await _sut.CreateStockPosition(addStockPositionRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Quantity must be greater than 0");
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.001)]
        [InlineData(-1.0)]
        public async Task CreateStockPosition_AveragePriceIsLessThanMinimum_ToBeArgumentException(double? averagePrice)
        {
            AddStockPositionRequest? addStockPositionRequest = _fixture.Build<AddStockPositionRequest>()
                .With(temp => temp.AveragePrice, averagePrice)
                .Create();

            Func<Task> action = async () =>
            {
                await _sut.CreateStockPosition(addStockPositionRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Average price must be greater than 0.01");
        }


        [Fact]
        public async Task CreateStockPosition_ValidData_ToBeSuccessful()
        {
            AddStockPositionRequest addStockPositionRequest =
                _fixture.Build<AddStockPositionRequest>().Create();
            TransactionResponse transactionResponseMock = _fixture.Build<TransactionResponse>().Create();

            StockPosition stockPositionExpected = addStockPositionRequest.ToStockPosition();
            

            double stockPriceMock = _fixture.Create<double>();

            _stockPositionRepositoryMock
                .Setup(temp => temp.CreateStockPosition(It.IsAny<StockPosition>()))
                .Returns(Task.CompletedTask);

            _finnhubServiceMock
                .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(stockPriceMock);

            _transactionServiceMock
                .Setup(temp => temp.CreateTransaction(It.IsAny<AddTransactionRequest>()))
                .ReturnsAsync(transactionResponseMock);

            StockPositionResponse stockPositionResponseExpected = stockPositionExpected.ToStockPositionResponse(stockPriceMock);

            StockPositionResponse? stockPositionResponse = await _sut.CreateStockPosition(addStockPositionRequest);

            stockPositionResponseExpected.PositionId = stockPositionResponse.PositionId;
            stockPositionResponse?.PositionId.Should().NotBe(Guid.Empty);
            stockPositionResponse.Should().BeEquivalentTo(stockPositionResponseExpected);
        }


        [Fact]
        public async Task CreateStockPosition_InvalidStockSymbol_ToBeArgumentException()
        {
            AddStockPositionRequest? addStockPositionRequest = _fixture.Build<AddStockPositionRequest>().Create();
            double price = 0;
            //mock finnhub return
            _finnhubServiceMock
                .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(price);


            Func<Task> action = async () =>
            {
                await _sut.CreateStockPosition(addStockPositionRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>(); ;
        }
        #endregion


        #region GetSingleStockPosition

        [Fact]
        public async Task GetSingleStockPosition_NullPositionId_ToBeArgumentNullException()
        {
            Guid? positionId = null;
            Func<Task> action = async () =>
            {
                await _sut.GetSingleStockPosition(positionId);
            };

            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'positionId')");
        }

        [Fact]
        public async Task GetSingleStockPosition_NoMatchingPosition_ToBeNull()
        {
            Guid positionId = _fixture.Create<Guid>();

            StockPosition? stockPositionExpected = null;

            _stockPositionRepositoryMock
                .Setup(temp => temp.GetSingleStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(stockPositionExpected);

            StockPositionResponse? stockPositionResponse = await _sut.GetSingleStockPosition(positionId);

            stockPositionResponse.Should().BeNull();
        }

        [Fact]
        public async Task GetSingleStockPosition_MatchingPosition_ToBeSuccessful()
        {
            StockPosition stockPosition = _fixture.Build<StockPosition>().Create();
            double stockPriceMock = _fixture.Create<double>();

            _stockPositionRepositoryMock
                .Setup(temp => temp.GetSingleStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(stockPosition);

            _finnhubServiceMock
                .Setup(temp => temp.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(stockPriceMock);

            StockPositionResponse stockPositionResponseExpected = stockPosition.ToStockPositionResponse(stockPriceMock);

            StockPositionResponse? stockPositionResponse = await _sut.GetSingleStockPosition(stockPosition.PositionId);

            stockPositionResponse.Should().BeEquivalentTo(stockPositionResponseExpected);
        }

        #endregion

        #region GetAllStockPositions

        [Fact]
        public async Task GetAllStockPositions_NoStockPositions_ToBeEmptyList()
        {
            List<StockPosition> stockPositions = new List<StockPosition>();

            _stockPositionRepositoryMock
                .Setup(temp => temp.GetAllStockPositions())
                .ReturnsAsync(stockPositions);

            List<StockPositionResponse> stockPositionResponse = await _sut.GetAllStockPositions();

            stockPositionResponse.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllStockPosition_WithStockPositions_ToBeSuccessful()
        {
            Guid positionId1 = _fixture.Create<Guid>();
            Guid positionId2 = _fixture.Create<Guid>();
            Guid positionId3 = _fixture.Create<Guid>();

            List<StockPosition> stockPositionsMock = new List<StockPosition>()
            {
                _fixture.Build<StockPosition>().With(temp => temp.PositionId, positionId1).Create(),
                _fixture.Build<StockPosition>().With(temp => temp.PositionId, positionId2).Create(),
                _fixture.Build<StockPosition>().With(temp => temp.PositionId, positionId3).Create()
            };

            double mockPrice1 = _fixture.Create<double>();
            double mockPrice2 = _fixture.Create<double>();
            double mockPrice3 = _fixture.Create<double>();

            Dictionary<string, double> stockPriceDictMock = new Dictionary<string, double>()
            {
                { stockPositionsMock[0].Symbol, mockPrice1 },
                { stockPositionsMock[1].Symbol, mockPrice2 },
                { stockPositionsMock[2].Symbol, mockPrice3 }
            };

            _stockPositionRepositoryMock
                .Setup(temp => temp.GetAllStockPositions())
                .ReturnsAsync(stockPositionsMock);

            _finnhubServiceMock
                .Setup(temp => temp.GetMultipleStockPriceQuote(It.IsAny<List<string>>()))
                .ReturnsAsync(stockPriceDictMock);

            List<StockPositionResponse> stockPositionResponseExpected = new();

            foreach (KeyValuePair<string, double> entry in stockPriceDictMock)
            {
                // get index of stockPosition with the given stockSymbol
                int stockPositionSymbolIndex = stockPositionsMock
                    .FindIndex(stockPosition => stockPosition.Symbol == entry.Key);

                stockPositionResponseExpected
                    .Add(stockPositionsMock[stockPositionSymbolIndex]
                    .ToStockPositionResponse(entry.Value));
            }

            List<StockPositionResponse> stockPositionResponse = await _sut.GetAllStockPositions();

            stockPositionResponse.Should().BeEquivalentTo(stockPositionResponseExpected);
        }

        #endregion

        public void Dispose()
        {
            _mockRepository.VerifyAll();
        }
    }

}
