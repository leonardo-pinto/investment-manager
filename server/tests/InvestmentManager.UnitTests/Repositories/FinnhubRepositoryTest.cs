//using FluentAssertions;
//using InvestmentManager.ApplicationCore.Interfaces;
//using InvestmentManager.Infrastructure.Repositories;
//using Microsoft.Extensions.Configuration;
//using Moq;
//using Moq.Protected;
//using Newtonsoft.Json;
//using System.Net;
//using System.Net.Http;

//namespace InvestmentManager.UnitTests.Repositories
//{
//    public class FinnhubRepositoryTest
//    {
//        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
//        private readonly IFinnhubRepository _sut;

//        public FinnhubRepositoryTest()
//        {
//            var _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
//            var _httpClientMock = new HttpClient(_mockHttpMessageHandler.Object);
//            var configurationMock = new Mock<IConfiguration>();

//            _sut = new FinnhubRepository(_httpClientMock, configurationMock.Object);
//        }

//        #region GetStockPriceQuote

//        [Fact]
//        async public Task GetStockPriceQuote_ToBeSuccessful()
//        {
//            // Arrange

//            Dictionary<string, object> responseDict = new()
//            {
//                { "c", 73.94 },
//                { "d", -0.5 },
//                { "dp", -0.6717 },
//                { "h", 74.047 },
//                { "l", 73.81 }
//            };

//            string responseDictJson = JsonConvert.SerializeObject(responseDict);

//            _mockHttpMessageHandler.Protected()
//            .Setup<Task<HttpResponseMessage>>("GetAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
//            .ReturnsAsync(new HttpResponseMessage
//            {
//                StatusCode = HttpStatusCode.OK,
//                Content = new StringContent(responseDictJson)
//            });

//            // Act
//            double stockPriceQuote = await _sut.GetStockPriceQuote("any");

//            // Assert
//            stockPriceQuote.Should().Be(73.94);
//        }

//        #endregion
//    }
//}
