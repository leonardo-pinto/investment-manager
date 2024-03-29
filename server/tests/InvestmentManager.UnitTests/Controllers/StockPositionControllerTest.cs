﻿using AutoFixture;
using AutoMapper;
using FluentAssertions;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Enums;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Mapper;
using InvestmentManager.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

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
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.NameIdentifier, "1")
           }, "mock"));
            _sut.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        #region CreateStockPosition

        [Fact]
        async public Task CreateStockPosition_WhenStockPositionResponseIsNull_ToBeBadRequest()
        {
            // Arrange
            var addStockPositionRequest = _fixture.Build<AddStockPositionRequest>().Create();

            _stockPositionServiceMock
                .Setup(m => m.CreateStockPosition(It.IsAny<AddStockPositionRequest>()))
                .ReturnsAsync(null as StockPositionResponse);

            // Act
            var result = await _sut.CreateStockPosition(addStockPositionRequest);

            // Assert
            var badRequestObject = result.Result as BadRequestObjectResult;
            badRequestObject.As<BadRequestObjectResult>().Value.Should().BeEquivalentTo(new ErrorResponse() { Error = "Invalid stock symbol" });

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
            var result = await _sut.CreateStockPosition(addStockPositionRequest);

            // Assert
            result.Should().BeOfType<ActionResult<StockPositionResponse>>();
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult?.RouteValues["id"].Should().Be(stockPositionResponse?.PositionId);
            createdAtActionResult?.Value.Should().BeEquivalentTo(stockPositionResponse);
            _stockPositionServiceMock
                .Verify(m => m.CreateStockPosition(addStockPositionRequest), Times.Once);
            _transactionServiceMock
                .Verify(m => m.CreateTransaction(It.IsAny<AddTransactionRequest>()), Times.Once);
        }

        #endregion

        #region GetAllStockPositionsByTradingCountry
        [Fact]
        async public Task GetAllStockPositionsByTradingCountry_ToBeOk()
        {
            // Arrange
            List<StockPositionResponse> stockPositionResponse = new()
            {
                _fixture.Build<StockPositionResponse>().Create(),
                _fixture.Build<StockPositionResponse>().Create(),
                _fixture.Build<StockPositionResponse>().Create(),
            };

            _stockPositionServiceMock
                .Setup(m => m.GetAllStockPositionsByUserIdAndTradingCountry(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(stockPositionResponse);

            // Act
            var result = await _sut.GetAllStockPositionsByTradingCountry("BR");

            // Assert
            result.Should().BeOfType<ActionResult<StockPositionsResponse>>();
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult?.Value.Should().BeEquivalentTo(new StockPositionsResponse() { StockPositions = stockPositionResponse });
            _stockPositionServiceMock.Verify(m => m.GetAllStockPositionsByUserIdAndTradingCountry("1", "BR"), Times.Once);

        }

        #endregion

        #region GetSingleStockPosition

        [Fact]
        async public Task GetSingleStockPosition_WhenPositionIdIsInvalid_ToBeNotFound()
        {
            // Arrange
            Guid id = _fixture.Create<Guid>();

            _stockPositionServiceMock
                .Setup(m => m.GetSingleStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(null as StockPositionResponse);

            // Act
            var result = await _sut.GetSingleStockPosition(id);

            // Assert
            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            notFoundObjectResult?.Value.Should().BeEquivalentTo(new ErrorResponse() { Error = "Stock position not found" });
            _stockPositionServiceMock.Verify(m => m.GetSingleStockPosition(id), Times.Once);

        }

        [Fact]
        async public Task GetSingleStockPosition_ToBeOk()
        {
            // Arrange
            Guid id = _fixture.Create<Guid>();
            StockPositionResponse stockPositionResponse = _fixture.Build<StockPositionResponse>().Create();

            _stockPositionServiceMock
                .Setup(m => m.GetSingleStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(stockPositionResponse);

            // Act
            var result = await _sut.GetSingleStockPosition(id);

            // Assert
            result.Should().BeOfType<ActionResult<StockPositionResponse>>();
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult?.Value.Should().Be(stockPositionResponse);
            _stockPositionServiceMock.Verify(m => m.GetSingleStockPosition(id), Times.Once);
        }
        #endregion

        #region UpdateStockPosition
        [Fact]
        async public Task UpdateStockPosition_WhenPositionIdIsInvalid_ToBeNotFound()
        {
            // Arrange
            Guid id = _fixture.Create<Guid>();

            UpdateStockPositionRequest updateStockPositionRequest =
                _fixture.Build<UpdateStockPositionRequest>().Create();

            _stockPositionServiceMock
                .Setup(m => m.UpdateStockPosition(It.IsAny<UpdateStockPositionRequest>()))
                .ReturnsAsync(null as StockPositionResponse);

            // Act
            var result = await _sut.UpdateStockPosition(updateStockPositionRequest);

            // Assert
            var notFoundObject = result.Result as NotFoundObjectResult;
            notFoundObject?.Value.Should().BeEquivalentTo(new ErrorResponse() { Error = "Stock position not found" });
            _stockPositionServiceMock.Verify(m => m.UpdateStockPosition(updateStockPositionRequest), Times.Once);
        }

        [Fact]
        async public Task UpdateStockPosition_ToBeOk()
        {
            // Arrange
            Guid id = _fixture.Create<Guid>();
            UpdateStockPositionRequest updateStockPositionRequest =
                _fixture
                .Build<UpdateStockPositionRequest>()
                .With(e => e.TransactionType, TransactionType.Buy)
                .Create();

            StockPositionResponse stockPositionResponse =
                _fixture.Build<StockPositionResponse>().Create();

            _stockPositionServiceMock
                .Setup(m => m.UpdateStockPosition(It.IsAny<UpdateStockPositionRequest>()))
                .ReturnsAsync(stockPositionResponse);

            _transactionServiceMock
                .Setup(m => m.CreateTransaction(It.IsAny<AddTransactionRequest>()))
                .ReturnsAsync(_fixture.Build<TransactionResponse>().Create());

            // Act
            var result = await _sut.UpdateStockPosition(updateStockPositionRequest);

            // Assert
            result.Should().BeOfType<ActionResult<StockPositionResponse>>();
            var okObjectresult = result.Result as OkObjectResult;
            okObjectresult?.Value.Should().Be(stockPositionResponse);

            _stockPositionServiceMock.Verify(m => m.UpdateStockPosition(updateStockPositionRequest), Times.Once);
            _transactionServiceMock.Verify(m => m.CreateTransaction(It.IsAny<AddTransactionRequest>()), Times.Once);
        }
        #endregion

        #region DeleteStockPosition

        [Fact]
        public async Task DeleteStockPosition_ValidId_ToBeNoContent()
        {
            // Arrange
            Guid id = _fixture.Create<Guid>();
            _stockPositionServiceMock
                .Setup(m => m.DeleteStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteStockPosition(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteStockPosition_InvalidId_ToBeNotFound()
        {
            // Arrange
            Guid id = _fixture.Create<Guid>();
            _stockPositionServiceMock
                .Setup(m => m.DeleteStockPosition(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteStockPosition(id);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            result.As<NotFoundObjectResult>().Value.Should().BeEquivalentTo(new ErrorResponse() { Error = "Stock position to be deleted was not found" });
        }
        #endregion
    }
}
