using PersonalPhotos.Controllers;
using Moq;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using PersonalPhotos.Models;
using System.Threading.Tasks;

namespace PersonalPhotos.Test
{
    public class LoginsTests
    {
        private readonly LoginsController _controller;
        private readonly Mock<ILogins> _logins; // Precisa ser a interface do business/services
        private readonly Mock<IHttpContextAccessor> _acessor;
        public LoginsTests()
        {
            // O moq vai criar uma inst�ncia dos attributos
            _logins = new Mock<ILogins>();
            _acessor = new Mock<IHttpContextAccessor>();
            // Com meus objetos criados, eu posso passar os par�metros necess�rio para o meu controller
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

            // Mockando um view model para acessar o m�todo de login
            var result = await _controller.Login(Mock.Of<LoginViewModel>()) as ViewResult;
            Assert.Equal("Login", result.ViewName, ignoreCase: true);
        }
    }
}
