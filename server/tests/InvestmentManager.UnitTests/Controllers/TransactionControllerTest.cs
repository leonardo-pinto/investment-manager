using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InvestmentManager.UnitTests.Controllers
{
    public class TransactionControllerTest
    {
        private readonly TransactionController _sut;
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly IFixture _fixture;

        public TransactionControllerTest()
        {
            _transactionServiceMock = new Mock<ITransactionService>(MockBehavior.Strict);
            _sut = new TransactionController(_transactionServiceMock.Object);
            _fixture = new Fixture();
        }

        #region GetAllTransactionsByPositionId

        [Fact]
        async public Task GetAllTransactionsByPositionId_NonMatchingPositionId_ToBeOkEmpty()
        {
            // Arrange
            Guid positionId = _fixture.Create<Guid>();
            _transactionServiceMock
                .Setup(m => m.GetTransactionHistory(positionId))
                .ReturnsAsync(new List<TransactionResponse>());

            // Act
            IActionResult transactions = await _sut.GetAllTransactionsByPositionId(positionId);

            // Assert
            transactions.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new List<TransactionResponse>());

            _transactionServiceMock.Verify(m => m.GetTransactionHistory(positionId), Times.Once());
        }

        [Fact]
        async public Task GetAllTransactionsByPositionId_MatchingPositionId_ToBeOkWithTransactions()
        {
            // Arrange
            Guid positionId = _fixture.Create<Guid>();
            List<TransactionResponse> transactionResponseMock = new ()
            {
                _fixture.Build<TransactionResponse>().Create(),
                _fixture.Build<TransactionResponse>().Create(),
                _fixture.Build<TransactionResponse>().Create(),
                _fixture.Build<TransactionResponse>().Create(),
            };

            _transactionServiceMock
                .Setup(m => m.GetTransactionHistory(It.IsAny<Guid>()))
                .ReturnsAsync(transactionResponseMock);

            // Act
            IActionResult transactionResponse = await _sut.GetAllTransactionsByPositionId(positionId);

            // Assert
            transactionResponse.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(transactionResponseMock);
            _transactionServiceMock.Verify(m => m.GetTransactionHistory(positionId), Times.Once);
        }

        #endregion
    }
}
