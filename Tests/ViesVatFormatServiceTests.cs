using ViesApi.Interfaces;
using ViesApi.Services;

namespace Tests;

public class ViesVatFormatServiceTests
{
    private readonly IViesVatFormatService _service = new ViesVatFormatService();

    [Fact]
    public void FormatVatNumber_ShouldAddCountryCode_WhenNotPresent()
    {
        // Arrange
        var vatNumber = "12345678";
        var countryCode = "HU";

        // Act
        var result = _service.FormatVatNumber(vatNumber, countryCode);

        // Assert
        Assert.Equal("HU12345678", result);
    }

    [Fact]
    public void FormatVatNumber_ShouldNotDuplicate_WhenCountryCodeAlreadyPresent()
    {
        // Arrange
        var vatNumber = "HU12345678";
        var countryCode = "HU";

        // Act
        var result = _service.FormatVatNumber(vatNumber, countryCode);

        // Assert
        Assert.Equal("HU12345678", result);
    }

    [Fact]
    public void FormatVatNumber_ShouldHandleAustrianFormat_WithUPrefix()
    {
        // Arrange
        var vatNumber = "12345678";
        var countryCode = "AT";

        // Act
        var result = _service.FormatVatNumber(vatNumber, countryCode);

        // Assert
        Assert.Equal("ATU12345678", result);
    }

    [Fact]
    public void FormatVatNumber_ShouldHandleDutchFormat_WithBSuffix()
    {
        // Arrange
        var vatNumber = "123456789";
        var countryCode = "NL";

        // Act
        var result = _service.FormatVatNumber(vatNumber, countryCode);

        // Assert
        Assert.Equal("NL123456789B01", result);
    }

    [Theory]
    [InlineData("", "HU", "")]
    [InlineData(null, "HU", null)]
    [InlineData("12345678", "", "12345678")]
    [InlineData("12345678", null, "12345678")]
    public void FormatVatNumber_ShouldHandleInvalidInputs(string? vatNumber, string? countryCode, string? expected)
    {
        // Act
        var result = _service.FormatVatNumber(vatNumber, countryCode);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCountryName_ShouldReturnEnglishByDefault()
    {
        // Act
        var result = _service.GetCountryName("HU");

        // Assert
        Assert.Equal("Hungary", result);
    }

    [Fact]
    public void GetCountryName_ShouldReturnHungarianWhenRequested()
    {
        // Act
        var result = _service.GetCountryName("HU", "hu");

        // Assert
        Assert.Equal("Magyarország", result);
    }

    [Fact]
    public void GetCountryName_ShouldReturnGermanWhenRequested()
    {
        // Act
        var result = _service.GetCountryName("DE", "de");

        // Assert
        Assert.Equal("Deutschland", result);
    }

    [Fact]
    public void GetCountryName_ShouldFallbackToEnglish_WhenLanguageNotSupported()
    {
        // Act
        var result = _service.GetCountryName("HU", "xyz");

        // Assert
        Assert.Equal("Hungary", result);
    }

    [Fact]
    public void GetCountryName_ShouldReturnCountryCode_WhenCountryNotFound()
    {
        // Act
        var result = _service.GetCountryName("XX");

        // Assert
        Assert.Equal("XX", result);
    }

    [Fact]
    public void GetAllCountries_ShouldReturnAllCountries()
    {
        // Act
        var result = _service.GetAllCountries();

        // Assert
        Assert.True(result.Count >= 27); // At least 27 EU countries
        Assert.Contains(result, c => c.Code == "HU");
        Assert.Contains(result, c => c.Code == "DE");
        Assert.Contains(result, c => c.Code == "FR");
    }

    [Fact]
    public void GetAllCountries_ShouldReturnLocalizedNames()
    {
        // Act
        // ReSharper disable once RedundantArgumentDefaultValue
        var englishResult = _service.GetAllCountries("en");
        var hungarianResult = _service.GetAllCountries("hu");

        // Assert
        var huCountryEn = englishResult.First(c => c.Code == "HU");
        var huCountryHu = hungarianResult.First(c => c.Code == "HU");

        Assert.Equal("Hungary", huCountryEn.Name);
        Assert.Equal("Magyarország", huCountryHu.Name);
    }

    [Fact]
    public void GetSupportedLanguages_ShouldReturnLanguageList()
    {
        // Act
        var result = _service.GetSupportedLanguages();

        // Assert
        Assert.Contains("en", result);
        Assert.Contains("hu", result);
        Assert.Contains("de", result);
        Assert.Contains("fr", result);
        Assert.True(result.Count >= 10);
    }

    [Fact]
    public void GetAllCountryNames_ShouldReturnLocalizedDictionary()
    {
        // Act
        // ReSharper disable once RedundantArgumentDefaultValue
        var englishNames = _service.GetAllCountryNames("en");
        var hungarianNames = _service.GetAllCountryNames("hu");

        // Assert
        Assert.Equal("Hungary", englishNames["HU"]);
        Assert.Equal("Magyarország", hungarianNames["HU"]);
        Assert.Equal("Germany", englishNames["DE"]);
        Assert.Equal("Németország", hungarianNames["DE"]);
    }

    [Fact]
    public void GetVatConfig_ShouldReturnCorrectConfig()
    {
        // Act
        var result = _service.GetVatConfig("HU");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("########", result.Format);
        Assert.Equal("HU12345678", result.Example);
    }

    [Fact]
    public void GetVatConfig_ShouldReturnNull_WhenCountryNotFound()
    {
        // Act
        var result = _service.GetVatConfig("XX");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetExampleVatNumber_ShouldReturnCorrectExample()
    {
        // Act
        var result = _service.GetExampleVatNumber("HU");

        // Assert
        Assert.Equal("HU12345678", result);
    }

    [Fact]
    public void GetExampleVatNumber_ShouldReturnDefault_WhenCountryNotFound()
    {
        // Act
        var result = _service.GetExampleVatNumber("XX");

        // Assert
        Assert.Equal("XX12345678", result);
    }
}