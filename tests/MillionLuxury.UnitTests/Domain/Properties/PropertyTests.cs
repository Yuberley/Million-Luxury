namespace MillionLuxury.UnitTests.Domain.Properties;

#region Usings
using FluentAssertions;
using MillionLuxury.Domain.Properties;
using MillionLuxury.Domain.Properties.ValueObjects;
using MillionLuxury.Domain.SharedValueObjects;
#endregion

public class PropertyTests
{
    [Fact]
    public void Create_Property_Should_Return_Valid_Property()
    {
        // Arrange
        var name = "Beautiful Villa";
        var price = new Money(500000m, Currency.FromCode("USD"));
        var internalCode = "PROP001";
        var year = 2020;
        var ownerId = Guid.NewGuid();
        var propertyType = PropertyType.House;
        var bedrooms = 4;
        var bathrooms = 3;
        var area = 250.5m;
        var description = "A beautiful villa with ocean view";
        var createdAt = DateTime.UtcNow;

        var address = new Address(
            "USA",
            "CA",
            "San Francisco",
            "94105",
            "456 Market Street"
        );

        var details = new PropertyDetails(
            propertyType,
            bedrooms,
            bathrooms,
            area,
            description
        );

        // Act
        var property = Property.Create(
            name,
            address,
            price,
            internalCode,
            year,
            ownerId,
            details,
            createdAt
        );

        // Assert
        property.Should().NotBeNull();
        property.Id.Should().NotBeEmpty();
        property.Name.Should().Be(name);
        property.Address.Should().Be(address);
        property.Price.Should().Be(price);
        property.InternalCode.Should().Be(internalCode);
        property.Year.Should().Be(year);
        property.OwnerId.Should().Be(ownerId);
        property.Status.Should().Be(PropertyStatus.Available);
        property.Details.PropertyType.Should().Be(propertyType);
        property.Details.Bedrooms.Should().Be(bedrooms);
        property.Details.Bathrooms.Should().Be(bathrooms);
        property.Details.AreaInSquareMeters.Should().Be(area);
        property.Details.Description.Should().Be(description);
        property.CreatedAt.Should().Be(createdAt);
        property.UpdatedAt.Should().Be(createdAt);
        property.Images.Should().BeEmpty();
    }

    [Fact]
    public void ChangePrice_Should_Update_Price_And_UpdatedAt()
    {
        // Arrange
        var property = CreateTestProperty();
        var newPrice = new Money(600000m, Currency.FromCode("USD"));
        var updatedAt = DateTime.UtcNow.AddDays(1);

        // Act
        property.ChangePrice(newPrice, updatedAt);

        // Assert
        property.Price.Should().Be(newPrice);
        property.UpdatedAt.Should().Be(updatedAt);
    }

    [Fact]
    public void ChangePrice_WithZeroOrNegativePrice_Should_Throw_ArgumentException()
    {
        // Arrange
        var property = CreateTestProperty();
        var invalidPrice = new Money(0m, Currency.FromCode("USD"));
        var updatedAt = DateTime.UtcNow;

        // Act & Assert
        var act = () => property.ChangePrice(invalidPrice, updatedAt);
        act.Should().Throw<ArgumentException>()
            .WithMessage("Price must be greater than zero*");
    }

    [Fact]
    public void UpdateProperty_Should_Update_All_Fields_And_UpdatedAt()
    {
        // Arrange
        var property = CreateTestProperty();
        var newName = "Updated Villa";
        var newYear = 2021;
        var newPropertyType = PropertyType.Apartment;
        var newStatus = PropertyStatus.Sold;
        var newBedrooms = 3;
        var newBathrooms = 2;
        var newArea = 200m;
        var newDescription = "Updated description";
        var updatedAt = DateTime.UtcNow.AddDays(1);

        var newAddress = new Address(
            "USA",
            "CA",
            "Los Angeles",
            "90001",
            "789 Sunset Boulevard"
        );

        var newDetails = new PropertyDetails(
            newPropertyType,
            newBedrooms,
            newBathrooms,
            newArea,
            newDescription
        );

        // Act
        property.UpdateProperty(
            newName,
            newAddress,
            newYear,
            newStatus,
            newDetails,
            updatedAt
        );

        // Assert
        property.Name.Should().Be(newName);
        property.Address.Should().Be(newAddress);
        property.Year.Should().Be(newYear);
        property.Status.Should().Be(newStatus);
        property.Details.PropertyType.Should().Be(newPropertyType);
        property.Details.Bedrooms.Should().Be(newBedrooms);
        property.Details.Bathrooms.Should().Be(newBathrooms);
        property.Details.AreaInSquareMeters.Should().Be(newArea);
        property.Details.Description.Should().Be(newDescription);
        property.UpdatedAt.Should().Be(updatedAt);
    }

    [Fact]
    public void AddImage_Should_Add_Image_To_Property()
    {
        // Arrange
        var property = CreateTestProperty();
        var image = PropertyImage.Create(
            property.Id,
            "test.jpg",
            "/images/test.jpg",
            DateTime.UtcNow
        );

        // Act
        property.AddImage(image);

        // Assert
        property.Images.Should().HaveCount(1);
        property.Images.First().Should().Be(image);
    }

    [Fact]
    public void RemoveImage_Should_Remove_Image_From_Property()
    {
        // Arrange
        var property = CreateTestProperty();
        var image = PropertyImage.Create(
            property.Id,
            "test.jpg",
            "/images/test.jpg",
            DateTime.UtcNow
        );
        property.AddImage(image);

        // Act
        property.RemoveImage(image.Id);

        // Assert
        property.Images.Should().BeEmpty();
    }

    private static Property CreateTestProperty()
    {
        var address = new Address(
            "USA",
            "CA",
            "San Francisco",
            "94105",
            "456 Market Street"
        );

        var details = new PropertyDetails(
            PropertyType.House,
            3,
            2,
            150m,
            "Test description"
        );

        return Property.Create(
            "Test Property",
            address,
            new Money(100000m, Currency.FromCode("USD")),
            "TEST001",
            2020,
            Guid.NewGuid(),
            details,
            DateTime.UtcNow
        );
    }
}
