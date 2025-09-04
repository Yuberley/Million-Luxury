namespace MillionLuxury.UnitTests.Application.Properties;

#region Usings
using FluentAssertions;
using MillionLuxury.Application.Common.Abstractions.Authentication;
using MillionLuxury.Application.Common.Abstractions.Clock;
using MillionLuxury.Application.Properties.CreateProperty;
using MillionLuxury.Application.Properties.Dtos;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.Properties.ValueObjects;
using Moq;
#endregion

public class CreatePropertyHandlerTests
{
    private readonly Mock<IPropertyRepository> mockPropertyRepository;
    private readonly Mock<IUserContext> mockUserContext;
    private readonly Mock<IDateTimeProvider> mockDateTimeProvider;
    private readonly Mock<IUnitOfWork> mockUnitOfWork;
    private readonly CreatePropertyHandler handler;

    public CreatePropertyHandlerTests()
    {
        mockPropertyRepository = new Mock<IPropertyRepository>();
        mockUserContext = new Mock<IUserContext>();
        mockDateTimeProvider = new Mock<IDateTimeProvider>();
        mockUnitOfWork = new Mock<IUnitOfWork>();

        handler = new CreatePropertyHandler(
            mockPropertyRepository.Object,
            mockDateTimeProvider.Object,
            mockUnitOfWork.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_Should_Create_Property_Successfully()
    {
        var address = new MillionLuxury.Application.Properties.Dtos.Address(
            "USA",
            "CA",
            "San Francisco",
            "94105",
            "456 Market Street"
        );

        var details = new MillionLuxury.Application.Properties.Dtos.PropertyDetails(
            MillionLuxury.Application.Properties.Dtos.PropertyType.House,
            Bedrooms: 3,
            Bathrooms: 2,
            AreaInSquareMeters: 150m,
            Description: "Test description"
        );

        // Arrange
        var userId = Guid.NewGuid();
        var currentTime = DateTime.UtcNow;
        var request = new CreatePropertyRequest(
            OwnerId: userId,
            Name: "Test Property",
            Address: address,
            Price: 100000m,
            Currency: "USD",
            InternalCode: "TEST001",
            Year: 2020,
            Details: details
        );

        var command = new CreatePropertyCommand(request);

        mockUserContext.Setup(x => x.UserId).Returns(userId);
        mockDateTimeProvider.Setup(x => x.UtcNow).Returns(currentTime);
        mockPropertyRepository.Setup(x => x.ExistsByInternalCodeAsync(request.InternalCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        mockPropertyRepository.Verify(x => x.Add(It.IsAny<Property>()), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateInternalCode_Should_Return_Failure()
    {
        var address = new MillionLuxury.Application.Properties.Dtos.Address(
            "USA",
            "CA",
            "San Francisco",
            "94105",
            "456 Market Street"
        );

        var details = new MillionLuxury.Application.Properties.Dtos.PropertyDetails(
            MillionLuxury.Application.Properties.Dtos.PropertyType.House,
            Bedrooms: 3,
            Bathrooms: 2,
            AreaInSquareMeters: 150m,
            Description: "Test description"
        );

        // Arrange
        var request = new CreatePropertyRequest(
            OwnerId: Guid.NewGuid(),
            Name: "Test Property",
            Address: address,
            Price: 100000m,
            Currency: "USD",
            InternalCode: "DUPLICATE001",
            Year: 2020,
            Details: details
        );

        var command = new CreatePropertyCommand(request);

        mockPropertyRepository.Setup(x => x.ExistsByInternalCodeAsync(request.InternalCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PropertyErrors.InternalCodeAlreadyExists(request.InternalCode));

        mockPropertyRepository.Verify(x => x.Add(It.IsAny<Property>()), Times.Never);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
