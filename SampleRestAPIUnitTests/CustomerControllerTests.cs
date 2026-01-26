using SampleRestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SampleRestAPI.Interfaces;
using Xunit;
using SampleRestAPI.Controllers;

public class CustomerControllerTests
{
    [Fact]
    public async Task GetAll_returns_ok_with_items()
    {
        // Arrange
        var mockRepo = new Mock<ICustomerRepository>();

        var expectedItems = new List<Customer>
        {
            new Customer { CustomerId = 1, Name = "JOHN DOE" },
            new Customer { CustomerId = 1, Name = "JANE DOE" }
        };

        mockRepo
            .Setup(r => r.GetCustomersAsync())
            .ReturnsAsync(expectedItems);

        var controller = new CustomersController(mockRepo.Object);

        // Act
        var result = await controller.GetCustomers();

        // Assert
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result.Result);
        var model = Xunit.Assert.IsAssignableFrom<IEnumerable<Customer>>(okResult.Value);
        Xunit.Assert.Collection(model,
            item => Xunit.Assert.Equal("JOHN DOE", item.Name),
            item => Xunit.Assert.Equal("JANE DOE", item.Name));

        mockRepo.Verify(r => r.GetCustomersAsync(), Times.Once);
    }

    [Fact]
    public async Task GetById_returns_not_found_when_missing()
    {
        // Arrange
        var mockRepo = new Mock<ICustomerRepository>();
        mockRepo
            .Setup(r => r.GetCustomerByIdAsync(42))
            .ReturnsAsync((Customer?)null);

        var controller = new CustomersController(mockRepo.Object);

        // Act
        var result = await controller.GetById(42);

        // Assert
        Xunit.Assert.IsType<NotFoundResult>(result.Result);
        mockRepo.Verify(r => r.GetCustomerByIdAsync(42), Times.Once);
    }
}
