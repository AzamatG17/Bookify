using AutoFixture;
using Bookify.Api.Controllers;
using Bookify.Application.Interfaces.Services;
using Bookify.Application.Requests.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Tests.Unit.Controllers;

public class AvailableTimeControllerTests
{
    [Fact]
    public async Task GetAsync_ShouldCallService_Once()
    {
        // Arrange
        var autoFixture = new Fixture();
        var mockFreeTimeService = new Mock<IFreeTimeService>();
        var queryParametrs = autoFixture.Create<FreeTimeRequest>();

        mockFreeTimeService.Setup(x => x.GetFreeDayListAsync(It.IsAny<FreeTimeRequest>()));

        var sut = new AvailableTimeController(mockFreeTimeService.Object);

        // Act
        _ = await sut.GetFreeDaysAsync(queryParametrs);

        // Assert
        mockFreeTimeService.Verify(service => service.GetFreeDayListAsync(It.IsAny<FreeTimeRequest>()), Times.Once);
    }
}
