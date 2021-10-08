using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalPhotos.Controllers;
using PersonalPhotos.Models;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PersonalPhotos.Test
{
    public class PhotosTests
    {
        private readonly IFileStorage fileStore;
        private readonly IKeyGenerator keyGen;
        private readonly IPhotoMetaData photoMetaData;
        private readonly IFormFile formFile;
        private readonly PhotoUploadViewModel model;

        public PhotosTests()
        {
            fileStore = Mock.Of<IFileStorage>();
            keyGen = Mock.Of<IKeyGenerator>();
            photoMetaData = Mock.Of<IPhotoMetaData>();
            formFile = Mock.Of<IFormFile>();
            model = Mock.Of<PhotoUploadViewModel>(x => x.File == formFile);
        }

        [Fact]
        public async Task Upload_GivenFileName_ReturnsDisplayAction()
        {
            // Fase "Arrange" -> ver tudo o que precisa ser mockado.
            var session = Mock.Of<ISession>();
            session.Set("User", Encoding.UTF8.GetBytes("ab@com")); // Mockando um valor em session para não dar erro no getuser
            var context = Mock.Of<HttpContext>(x => x.Session == session);
            var acessor = Mock.Of<IHttpContextAccessor>(x => x.HttpContext == context); // Além de comparar, também atribui
            var controller = new PhotosController(keyGen, acessor, photoMetaData, fileStore);

            // Fase Act -> execução das ações de teste
            var result = await controller.Upload(model) as RedirectToActionResult;

            // Assert -> comparando os resultados obtidos com os resultados esperados no teste
            Assert.Equal("Display", result.ActionName, ignoreCase: true);
        }

        [Fact]
        public void Display_GivenEmptySession_ReturnsNull()
        {
            var session = Mock.Of<ISession>();
            var context = Mock.Of<HttpContext>(x => x.Session == session);
            var acessor = Mock.Of<IHttpContextAccessor>(x => x.HttpContext == context);
            var controller = new PhotosController(keyGen, acessor, photoMetaData, fileStore);

            var result = controller.Display() as RedirectToActionResult;

            Assert.Null(result);
        }
    }
}
