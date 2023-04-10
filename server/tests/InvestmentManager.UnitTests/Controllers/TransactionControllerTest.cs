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
        async public Task GetAllTransactionsByPositionId_ToBeOkWithTransactions()
        {
            // Arrange
            List<TransactionResponse> transactionResponseMock = new ()
            {
                _fixture.Build<TransactionResponse>().Create(),
                _fixture.Build<TransactionResponse>().Create(),
                _fixture.Build<TransactionResponse>().Create(),
                _fixture.Build<TransactionResponse>().Create(),
            };

            _transactionServiceMock
                .Setup(m => m.GetAllTransactions())
                .ReturnsAsync(transactionResponseMock);

            // Act
            IActionResult transactionResponse = await _sut.GetAllTransactions();

            // Assert
            transactionResponse.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(transactionResponseMock);
            _transactionServiceMock.Verify(m => m.GetAllTransactions(), Times.Once);
        }
        #endregion
    }
}
