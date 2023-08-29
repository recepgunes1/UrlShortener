using Buffer.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Buffer.UnitTests
{
    public class ControllerTests
    {
        [Fact]
        public async void GetUrl_ShouldReturnOk()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<BufferController>>();

            var controller = new BufferController(mockMediator.Object, mockLogger.Object);

            var result = await controller.GetUrl("test.com");


            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAllUrls_ShouldReturnOk()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<BufferController>>();

            var controller = new BufferController(mockMediator.Object, mockLogger.Object);

            var result = await controller.GetAllUrls();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}
