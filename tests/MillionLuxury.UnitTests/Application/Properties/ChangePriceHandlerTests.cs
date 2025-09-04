namespace MillionLuxury.UnitTests.Application.Properties;

#region Usings
using FluentAssertions;
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Properties.ChangePrice;
using MillionLuxury.Application.Properties.Dtos;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.SharedValueObjects;
using Moq;
#endregion

public class ChangePriceHandlerTests
{
    private readonly Mock<IPropertyRepository> mockPropertyRepository;
    private readonly Mock<IDateTimeProvider> mockDateTimeProvider;
    private readonly Mock<IUnitOfWork> mockUnitOfWork;
    private readonly ChangePriceHandler handler;

    public ChangePriceHandlerTests()
    {
        mockPropertyRepository = new Mock<IPropertyRepository>();
        mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockUnitOfWork = new Mock<IUnitOfWork>();

        handler = new ChangePriceHandler(
            mockPropertyRepository.Object,
            mockDateTimeProvider.Object,
            mockUnitOfWork.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_Should_Change_Price_Successfully()
    {
        // Arrange
        var propertyId = Guid.NewGuid();
        var currentTime = DateTime.UtcNow;
        var newPrice = 200000m;
        var currency = "USD";
        var property = CreateTestProperty(propertyId);

        var request = new ChangePriceRequest(newPrice, currency);
        var command = new ChangePriceCommand(propertyId, request);

        mockPropertyRepository.Setup(x => x.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(property);
        mockDateTimeProvider.Setup(x => x.UtcNow).Returns(currentTime);
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        property.Price.Should().Be(newPrice);
        property.UpdatedAt.Should().Be(currentTime);

        mockPropertyRepository.Verify(x => x.Update(property), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_PropertyNotFound_Should_Return_Failure()
    {
        // Arrange
        var propertyId = Guid.NewGuid();
        var request = new ChangePriceRequest(200000m, "USD");
        var command = new ChangePriceCommand(propertyId, request);

        mockPropertyRepository.Setup(x => x.GetByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property?)null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PropertyErrors.NotFound);

        mockPropertyRepository.Verify(x => x.Update(It.IsAny<Property>()), Times.Never);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static Property CreateTestProperty(Guid propertyId)
    {
        var address = new MillionLuxury.Domain.SharedValueObjects.Address(
            "USA",
            "CA",
            "San Francisco",
            "94105",
            "456 Market Street"
        );

        var details = new MillionLuxury.Domain.Properties.ValueObjects.PropertyDetails(
            MillionLuxury.Domain.Properties.ValueObjects.PropertyType.House,
            3,
            2,
            150m,
            "Test description"
        );

        var property = Property.Create(
            "Test Property",
            address,
            new Money(100000m, Currency.FromCode("USD")),
            "TEST001",
            2020,
            Guid.NewGuid(),
            details,
            DateTime.UtcNow
        );

        // Use reflection to set the Id for testing purposes
        var propertyType = typeof(Property);
        var idProperty = propertyType.BaseType!.GetProperty("Id")!;
        idProperty.SetValue(property, propertyId);

        return property;
    }
}
