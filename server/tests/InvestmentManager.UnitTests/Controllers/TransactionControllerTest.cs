using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

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
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            _sut.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        #region GetAllTransactions

        [Fact]
        async public Task GetAllTransactions_ToBeOk()
        {
            // Arrange
            List<TransactionResponse> transactionResponseMock = new()
            {
                _fixture.Build<TransactionResponse>().Create(),
                _fixture.Build<TransactionResponse>().Create(),
                _fixture.Build<TransactionResponse>().Create(),
                _fixture.Build<TransactionResponse>().Create(),
            };

            _transactionServiceMock
                .Setup(m => m.GetAllTransactionsByUserId("1"))
                .ReturnsAsync(transactionResponseMock);

            // Act
            var result = await _sut.GetAllTransactions();

            // Assert
            result.Should().BeOfType<ActionResult<TransactionsResponse>>();
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult?.Value.Should().BeEquivalentTo(new TransactionsResponse() { Transactions = transactionResponseMock });
            _transactionServiceMock.Verify(m => m.GetAllTransactionsByUserId("1"), Times.Once);

        }
        #endregion
    }
}
