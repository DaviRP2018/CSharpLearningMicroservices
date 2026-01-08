using Basket.API.Dtos;

namespace Basket.API.Tests.Dtos;

public class BasketCheckoutDtoTests
{
    [Fact]
    public void BasketCheckoutDto_Initialization_ShouldSetProperties()
    {
        // Arrange
        var userName = "testuser";
        var customerId = Guid.NewGuid();
        var totalPrice = 100.50m;
        var firstName = "John";
        var lastName = "Doe";
        var emailAddress = "john.doe@example.com";
        var addressLine = "123 Main St";
        var country = "USA";
        var state = "NY";
        var zipCode = "10001";
        var cardName = "John Doe";
        var cardNumber = "1111222233334444";
        var expiration = "12/25";
        var cvv = "123";
        var paymentMethod = 1;

        // Act
        var dto = new BasketCheckoutDto
        {
            UserName = userName,
            CustomerId = customerId,
            TotalPrice = totalPrice,
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = emailAddress,
            AddressLine = addressLine,
            Country = country,
            State = state,
            ZipCode = zipCode,
            CardName = cardName,
            CardNumber = cardNumber,
            Expiration = expiration,
            Cvv = cvv,
            PaymentMethod = paymentMethod
        };

        // Assert
        Assert.Equal(userName, dto.UserName);
        Assert.Equal(customerId, dto.CustomerId);
        Assert.Equal(totalPrice, dto.TotalPrice);
        Assert.Equal(firstName, dto.FirstName);
        Assert.Equal(lastName, dto.LastName);
        Assert.Equal(emailAddress, dto.EmailAddress);
        Assert.Equal(addressLine, dto.AddressLine);
        Assert.Equal(country, dto.Country);
        Assert.Equal(state, dto.State);
        Assert.Equal(zipCode, dto.ZipCode);
        Assert.Equal(cardName, dto.CardName);
        Assert.Equal(cardNumber, dto.CardNumber);
        Assert.Equal(expiration, dto.Expiration);
        Assert.Equal(cvv, dto.Cvv);
        Assert.Equal(paymentMethod, dto.PaymentMethod);
    }

    [Fact]
    public void BasketCheckoutDto_DefaultValues_ShouldBeCorrect()
    {
        // Act
        var dto = new BasketCheckoutDto();

        // Assert
        Assert.Null(dto.UserName);
        Assert.Equal(Guid.Empty, dto.CustomerId);
        Assert.Equal(0, dto.TotalPrice);
        Assert.Null(dto.FirstName);
        Assert.Null(dto.LastName);
        Assert.Null(dto.EmailAddress);
        Assert.Null(dto.AddressLine);
        Assert.Null(dto.Country);
        Assert.Null(dto.State);
        Assert.Null(dto.ZipCode);
        Assert.Null(dto.CardName);
        Assert.Null(dto.CardNumber);
        Assert.Null(dto.Expiration);
        Assert.Null(dto.Cvv);
        Assert.Equal(0, dto.PaymentMethod);
    }
}