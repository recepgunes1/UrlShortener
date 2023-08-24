using Buffer.WebAPI.Controllers;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Buffer.UnitTests
{
    public class ControllerTests
    {
        [Fact]
        public async void GetUrl_ShouldReturnOk()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<BufferController>>();
            var mockPublishEndpoint = new Mock<IPublishEndpoint>();

            var controller = new BufferController(mockMediator.Object, mockLogger.Object, mockPublishEndpoint.Object);

            var result = await controller.GetUrl("test.com");


            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAllUrls_ShouldReturnOk()
        {
            var mockMediator = new Mock<IMediator>();
            var mockLogger = new Mock<ILogger<BufferController>>();
            var mockPublishEndpoint = new Mock<IPublishEndpoint>();

            var controller = new BufferController(mockMediator.Object, mockLogger.Object, mockPublishEndpoint.Object);

            var result = await controller.GetAllUrls();

            //
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
