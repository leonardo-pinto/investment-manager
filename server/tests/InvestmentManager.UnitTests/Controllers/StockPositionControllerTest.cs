using AutoFixture;
using AutoMapper;
using FluentAssertions;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Mapper;
using InvestmentManager.ApplicationCore.Services;
using InvestmentManager.UnitTests.TestHelpers;
using InvestmentManager.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InvestmentManager.UnitTests.Controllers
{
    public class StockPositionControllerTest
    {
        private readonly StockPositionController _sut;
        private readonly Mock<IStockPositionService> _stockPositionServiceMock;
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly IMapper _mapper;
        private readonly IFixture _fixture;

        public StockPositionControllerTest()
        {
            MapperConfiguration? autoMapperConfig = new(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = new Mapper(autoMapperConfig);

            _stockPositionServiceMock = new Mock<IStockPositionService>(MockBehavior.Strict);
            _transactionServiceMock = new Mock<ITransactionService>(MockBehavior.Strict);
            _sut = new StockPositionController(
                _stockPositionServiceMock.Object, _transactionServiceMock.Object, _mapper);
            _fixture = new Fixture();
        }

        #region CreateStockPosition

        [Fact]
        async public Task CreateStockPosition_InvalidModel_ToBeBadRequest() 
        {
            // Arrange
            AddStockPositionRequest addStockPositionRequest = _fixture
                .Build<AddStockPositionRequest>()
                .With(e => e.Quantity, -500)
                .Create();

            _sut.ModelState.AddModelError("Quantity", "Quantity must be greater than 0");

            // Act
            IActionResult result = await _sut.CreateStockPosition(addStockPositionRequest);

            // Assert
            result
                .Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeAssignableTo<SerializableError>()
                .Which.ContainsKey("Quantity").Should().BeTrue();
        }

        [Fact]
        async public Task CreateStockPosition_WhenStockPositionResponseIsNull_ToBeBadRequest()
        {
            // Arrange
            var addStockPositionRequest = _fixture.Build<AddStockPositionRequest>().Create();

            _stockPositionServiceMock
                .Setup(m => m.CreateStockPosition(It.IsAny<AddStockPositionRequest>()))
                .ReturnsAsync(null as StockPositionResponse);

            // Act
            IActionResult result = await _sut.CreateStockPosition(addStockPositionRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Invalid stock symbol");

            _stockPositionServiceMock.Verify(m => m.CreateStockPosition(addStockPositionRequest), Times.Once);
        }

        [Fact]
        async public Task CreateStockPosition_ValidData_ToBeCreatedAtAction()
        {
            // Arrange
            var addStockPositionRequest = _fixture.Build<AddStockPositionRequest>().Create();
            var stockPositionResponse = _fixture.Build<StockPositionResponse>().Create();

            _stockPositionServiceMock
                .Setup(m => m.CreateStockPosition(It.IsAny<AddStockPositionRequest>()))
                .ReturnsAsync(stockPositionResponse);

            _transactionServiceMock
                .Setup(m => m.CreateTransaction(It.IsAny<AddTransactionRequest>()))
                .ReturnsAsync(_fixture.Build<TransactionResponse>().Create());

            // Act
            IActionResult result = await _sut.CreateStockPosition(addStockPositionRequest);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            result.As<CreatedAtActionResult>()?.RouteValues["positionId"].Should().Be(stockPositionResponse?.PositionId);
            result.As<CreatedAtActionResult>().Value.Should().BeEquivalentTo(stockPositionResponse);
            _stockPositionServiceMock
                .Verify(m => m.CreateStockPosition(addStockPositionRequest), Times.Once);
            _transactionServiceMock
                .Verify(m => m.CreateTransaction(It.IsAny<AddTransactionRequest>()), Times.Once);
        }

        #endregion

        #region GetAllStockPositions
        [Fact]
        async public Task GetAllStockPositions_ToBeOk()
        {
            // Arrange
            List<StockPositionResponse> stockPositionResponse = new()
            {
                _fixture.Build<StockPositionResponse>().Create(),
                _fixture.Build<StockPositionResponse>().Create(),
                _fixture.Build<StockPositionResponse>().Create(),
            };

            _stockPositionServiceMock
                .Setup(m => m.GetAllStockPositions())
                .ReturnsAsync(stockPositionResponse);

            // Act
            IActionResult response = await _sut.GetAllStockPositions();

            // Assert
            response.Should().BeOfType<OkObjectResult>();
            response.As<OkObjectResult>().Value.Should().Be(stockPositionResponse);
            _stockPositionServiceMock.Verify(m => m.GetAllStockPositions(), Times.Once);
        }

        #endregion

        #region GetSingleStockPosition

        [Fact]
        async public Task GetSingleStockPosition_WhenPositionIdIsInvalid_ToBeBadRequest()
        {
            // Arrange
            string id = MockHelper.GenerateValidGuidString();

            _stockPositionServiceMock
                .Setup(m => m.GetSingleStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(null as StockPositionResponse);

            // Act
            IActionResult response = await _sut.GetSingleStockPosition(id);

            // Assert
            response.Should().BeOfType<BadRequestObjectResult>();
            response.As<BadRequestObjectResult>().Value.Should().Be("Invalid position id");
            _stockPositionServiceMock.Verify(m => m.GetSingleStockPosition(Guid.Parse(id)), Times.Once);

        }

        [Fact]
        async public Task GetSingleStockPosition_ToBeOk()
        {
            // Arrange
            string id = MockHelper.GenerateValidGuidString();
            StockPositionResponse stockPositionResponse = _fixture.Build<StockPositionResponse>().Create();

            _stockPositionServiceMock
                .Setup(m => m.GetSingleStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(stockPositionResponse);

            // Act
            IActionResult response = await _sut.GetSingleStockPosition(id);

            // Assert
            response.Should().BeOfType<OkObjectResult>();
            response.As<OkObjectResult>().Value.Should().Be(stockPositionResponse);
            _stockPositionServiceMock.Verify(m => m.GetSingleStockPosition(Guid.Parse(id)), Times.Once);
        }
        #endregion

        #region UpdateStockPosition

        [Fact]
        async public Task UpdateStockPosition_InvalidModel_ToBeBadRequest()
        {
            // Arrange
            UpdateStockPositionRequest updateStockPositionRequest = _fixture
                .Build<UpdateStockPositionRequest>()
                .With(e => e.Quantity, -500)
                .Create();

            _sut.ModelState.AddModelError("Quantity", "Quantity must be greater than 0");

            // Act
            IActionResult result = await _sut.UpdateStockPosition(updateStockPositionRequest, "any");

            // Assert
            result
                .Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeAssignableTo<SerializableError>()
                .Which.ContainsKey("Quantity").Should().BeTrue();
        }

        [Fact]
        async public Task UpdateStockPosition_WhenPositionIdIsInvalid_ToBeBadRequest()
        {
            // Arrange
            string id = MockHelper.GenerateValidGuidString();

            UpdateStockPositionRequest updateStockPositionRequest =
                _fixture.Build<UpdateStockPositionRequest>().Create();

            _stockPositionServiceMock
                .Setup(m => m.UpdateStockPosition(It.IsAny<UpdateStockPositionRequest>(), It.IsAny<Guid>()))
                .ReturnsAsync(null as StockPositionResponse);

            // Act
            IActionResult result = await _sut.UpdateStockPosition(updateStockPositionRequest, id);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            result.As<BadRequestObjectResult>().Value.Should().Be("Invalid position id");
            _stockPositionServiceMock.Verify(m => m.UpdateStockPosition(updateStockPositionRequest, Guid.Parse(id)), Times.Once);
        }

        [Fact]
        async public Task UpdateStockPosition_ToBeOk()
        {
            // Arrange
            string id = MockHelper.GenerateValidGuidString();
            UpdateStockPositionRequest updateStockPositionRequest =
                _fixture.Build<UpdateStockPositionRequest>().Create();

            StockPositionResponse stockPositionResponse =
                _fixture.Build<StockPositionResponse>().Create();

            AddTransactionRequest addTransactionRequest = _mapper.Map<AddTransactionRequest>(updateStockPositionRequest); 

            _stockPositionServiceMock
                .Setup(m => m.UpdateStockPosition(It.IsAny<UpdateStockPositionRequest>(), It.IsAny<Guid>()))
                .ReturnsAsync(stockPositionResponse);

            _transactionServiceMock
                .Setup(m => m.CreateTransaction(It.IsAny<AddTransactionRequest>()))
                .ReturnsAsync(_fixture.Build<TransactionResponse>().Create());

            // Act
            IActionResult response = await _sut.UpdateStockPosition(updateStockPositionRequest, id);

            // Assert
            response.Should().BeOfType<OkObjectResult>();
            response.As<OkObjectResult>().Value.Should().Be(stockPositionResponse);

            _stockPositionServiceMock.Verify(m => m.UpdateStockPosition(updateStockPositionRequest, Guid.Parse(id)), Times.Once);
            _transactionServiceMock.Verify(m => m.CreateTransaction(It.IsAny<AddTransactionRequest>()), Times.Once);
        }
        #endregion
    }
}
