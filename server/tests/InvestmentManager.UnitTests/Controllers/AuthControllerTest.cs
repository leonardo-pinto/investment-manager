using AutoFixture;
using FluentAssertions;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InvestmentManager.UnitTests.Controllers
{
    public class AuthControllerTest
    {
        private readonly AuthController _sut;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly IFixture _fixture;

        public AuthControllerTest()
        {
            _authServiceMock = new Mock<IAuthService>(MockBehavior.Strict);
            _tokenServiceMock = new Mock<ITokenService>(MockBehavior.Strict);
            _sut = new AuthController(_authServiceMock.Object, _tokenServiceMock.Object);
            _fixture = new Fixture();
        }


        #region Register

        [Fact]
        public async Task Register_InvalidModel_ToBeBadRequest()
        {
            // Arrange
            var registerMock = _fixture.Build<RegisterRequest>().Create();

            _sut.ModelState.AddModelError("Password", "Password and confirm password do not match");

            // Act
            var result = await _sut.Register(registerMock);

            // Assert
            result
           .Should().BeOfType<BadRequestObjectResult>()
           .Which.Value.Should().BeEquivalentTo(new SerializableError
           {
                   { "Password", new []{ "Password and confirm password do not match" } }
           });
        }

        [Fact]
        public async Task Register_WhenRegisterFails_ToBeBadRequest()
        {
            // Arrange
            var registerRequest = _fixture.Build<RegisterRequest>().Create();

            var error = new IdentityError { Code = "Error", Description = "Test error" };

            _authServiceMock
                .Setup(m => m.Register(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(IdentityResult.Failed(error));

            // Act
            var result = await _sut.Register(registerRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new SerializableError
                   {
                           { "Register", new []{ "Test error" } }
                   });

        }

        [Fact]
        public async Task Register_WhenRegisterSucceedsButUserNotFound_ToBeUnauthorized()
        {
            // Arrange
            var registerRequest = _fixture.Build<RegisterRequest>().Create();

            _authServiceMock
                .Setup(m => m.Register(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(IdentityResult.Success);

            _authServiceMock
                .Setup(m => m.FindUserByUserName(It.IsAny<string>()))
                .ReturnsAsync(null as IdentityUser);

            // Act
            var result = await _sut.Register(registerRequest);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
            _authServiceMock.Verify(m => m.Register(registerRequest), Times.Once);
            _authServiceMock.Verify(m => m.FindUserByUserName(registerRequest.UserName), Times.Once);
        }

        [Fact]
        public async Task Register_ValidData_ToBeOk()
        {
            // Arrange
            var registerRequest = _fixture.Build<RegisterRequest>().Create();

            var userMock = _fixture
                .Build<IdentityUser>()
                .With(e => e.UserName, registerRequest.UserName)
                .Create();

            _authServiceMock
                .Setup(m => m.Register(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(IdentityResult.Success);

            _authServiceMock
                .Setup(m => m.FindUserByUserName(It.IsAny<string>()))
                .ReturnsAsync(userMock);

            string tokenMock = _fixture.Create<string>();

            _tokenServiceMock
                .Setup(m => m.GenerateToken(It.IsAny<IdentityUser>()))
                .Returns(tokenMock);

            var expectedResult = new AuthResponse()
            {
                Id = userMock.Id,
                Username = userMock.UserName,
                AccessToken = tokenMock
            };

            // Act
            var result = await _sut.Register(registerRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedResult);
            _tokenServiceMock.Verify(m => m.GenerateToken(userMock), Times.Once);
        }

        #endregion

        #region Login

        [Fact]
        public async Task Login_InvalidModel_ToBeBadRequest() 
        {
            // Arrange
            var loginMock = _fixture.Build<LoginRequest>().Create();

            _sut.ModelState.AddModelError("UserName", "User name can't be blank");

            // Act
            var result = await _sut.Login(loginMock);

            // Assert
            result
           .Should().BeOfType<BadRequestObjectResult>()
           .Which.Value.Should().BeEquivalentTo(new SerializableError
           {
                   { "UserName", new []{ "User name can't be blank" } }
           });
        }

        [Fact]
        public async Task Login_InvalidCredentials_ToBeUnauthorized() 
        {
            // Arrange
            var loginMock = _fixture.Build<LoginRequest>().Create();

            _authServiceMock
                .Setup(m => m.Login(It.IsAny<LoginRequest>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _sut.Login(loginMock);

            // Assert
            result.Should().BeOfType<UnauthorizedObjectResult>()
                .Which.Value.Should().BeEquivalentTo("Invalid credentials");
        }

        [Fact]
        public async Task Login_WhenLoginSucceedsButUserNotFound_ToBeUnauthorized() 
        {
            // Arrange
            var loginMock = _fixture.Build<LoginRequest>().Create();

            _authServiceMock
                .Setup(m => m.Login(It.IsAny<LoginRequest>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _authServiceMock
              .Setup(m => m.FindUserByUserName(It.IsAny<string>()))
              .ReturnsAsync(null as IdentityUser);

            // Act
            var result = await _sut.Login(loginMock);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task Login_ValidData_ToBeOk()
        {

            // Arrange
            var loginMock = _fixture.Build<LoginRequest>().Create();
            var userMock = _fixture
                 .Build<IdentityUser>()
                 .With(e => e.UserName, loginMock.UserName)
                 .Create();

            _authServiceMock
                .Setup(m => m.Login(It.IsAny<LoginRequest>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _authServiceMock
                .Setup(m => m.FindUserByUserName(It.IsAny<string>()))
                .ReturnsAsync(userMock);

            string tokenMock = _fixture.Create<string>();

            _tokenServiceMock
                .Setup(m => m.GenerateToken(It.IsAny<IdentityUser>()))
                .Returns(tokenMock);

            var expectedResult = new AuthResponse()
            {
                Id = userMock.Id,
                Username = userMock.UserName,
                AccessToken = tokenMock
            };

            // Act
            var result = await _sut.Login(loginMock);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedResult);
            _tokenServiceMock.Verify(m => m.GenerateToken(userMock), Times.Once);
        }

        #endregion
    }
}
