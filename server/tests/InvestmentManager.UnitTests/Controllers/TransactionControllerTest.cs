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

        #region GetAllTransactionsByUserId

        [Fact]
        async public Task GetAllTransactionsByUserId_ToBeOk()
        {
            // Arrange
            var userId = _fixture.Create<string>();

            List<TransactionResponse> transactionResponseMock = new()
            {
                _fixture.Build<TransactionResponse>().Create(),
                _fixture.Build<TransactionResponse>().Create(),
                _fixture.Build<TransactionResponse>().Create(),
                _fixture.Build<TransactionResponse>().Create(),
            };

            _transactionServiceMock
                .Setup(m => m.GetAllTransactionsByUserId(userId))
                .ReturnsAsync(transactionResponseMock);

            // Act
            var result = await _sut.GetAllTransactionsByUserId(userId);

            // Assert
            result.Should().BeOfType<ActionResult<TransactionsResponse>>();
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult?.Value.Should().BeEquivalentTo(new TransactionsResponse() { Transactions = transactionResponseMock });
            _transactionServiceMock.Verify(m => m.GetAllTransactionsByUserId(userId), Times.Once);
        }
        #endregion
    }
}
