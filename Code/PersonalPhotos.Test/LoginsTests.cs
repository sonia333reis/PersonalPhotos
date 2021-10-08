using PersonalPhotos.Controllers;
using Moq;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using PersonalPhotos.Models;
using System.Threading.Tasks;
using Core.Models;

namespace PersonalPhotos.Test
{
    public class LoginsTests
    {
        private readonly LoginsController _controller;
        private readonly Mock<ILogins> _logins; // Precisa ser a interface do business/services
        private readonly Mock<IHttpContextAccessor> _acessor;
        public LoginsTests()
        {
            // O moq vai criar uma instância dos attributos
            _logins = new Mock<ILogins>();
            var session = Mock.Of<ISession>();
            var httpContenxt = Mock.Of<HttpContext>(x => x.Session == session);
            _acessor = new Mock<IHttpContextAccessor>();
            _acessor.Setup(x => x.HttpContext).Returns(httpContenxt);
            // Com meus objetos criados, eu posso passar os parâmetros necessário para o meu controller
            _controller = new LoginsController(_logins.Object, _acessor.Object);
        }

        [Fact]
        public void Index_GivenNoReturnUrl_ReturnLoginView()
        {
            var result = (_controller.Index() as ViewResult);
            Assert.NotNull(result);
            Assert.Equal("Login", result.ViewName, ignoreCase: true);
        }

        [Fact]
        public async Task Login_GivenModelStateIsInvalid_ReturnLoginView()
        {
            // Invalidando meu model state
            _controller.ModelState.AddModelError("Test", "Test");

            // Mockando um view model para acessar o método de login
            var result = await _controller.Login(Mock.Of<LoginViewModel>()) as ViewResult;
            Assert.Equal("Login", result.ViewName, ignoreCase: true);
        }

        [Fact]
        public async Task Login_GivenCorrectPassword_RedirectToDisplayAction()
        {
            const string pass = "123";
            var modelView = Mock.Of<LoginViewModel>(x => x.Email == "ab@com.br" && x.Password == pass);
            var model = Mock.Of<User>(x => x.Password == pass);
            _logins.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(model);
            var result = await _controller.Login(modelView);
            Assert.IsType<RedirectToActionResult>(result);
        } 
    }
}
