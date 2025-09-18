using ViesApi;
using ViesApi.Configuration;

namespace ViesApi.Tests;

public class CountryVatFormatTests
{
    [Fact]
    public void GetCountryName_ShouldReturnEnglishByDefault()
    {
        // Arrange
        var format = new CountryVatFormat
        {
            CountryNames = new Dictionary<string, string>
            {
                {"en", "Hungary"},
                {"hu", "Magyarország"}
            }
        };

        // Act
        var result = format.GetCountryName();

        // Assert
        Assert.Equal("Hungary", result);
    }

    [Fact]
    public void GetCountryName_ShouldReturnSpecificLanguage()
    {
        // Arrange
        var format = new CountryVatFormat
        {
            CountryNames = new Dictionary<string, string>
            {
                {"en", "Hungary"},
                {"hu", "Magyarország"},
                {"de", "Ungarn"}
            }
        };

        // Act
        var resultHu = format.GetCountryName("hu");
        var resultDe = format.GetCountryName("de");

        // Assert
        Assert.Equal("Magyarország", resultHu);
        Assert.Equal("Ungarn", resultDe);
    }

    [Fact]
    public void GetCountryName_ShouldFallbackToEnglish_WhenLanguageNotFound()
    {
        // Arrange
        var format = new CountryVatFormat
        {
            CountryNames = new Dictionary<string, string>
            {
                {"en", "Hungary"},
                {"hu", "Magyarország"}
            }
        };

        // Act
        var result = format.GetCountryName("fr");

        // Assert
        Assert.Equal("Hungary", result);
    }

    [Fact]
    public void GetCountryName_ShouldReturnFirstAvailable_WhenEnglishNotFound()
    {
        // Arrange
        var format = new CountryVatFormat
        {
            CountryNames = new Dictionary<string, string>
            {
                {"hu", "Magyarország"},
                {"de", "Ungarn"}
            }
        };

        // Act
        var result = format.GetCountryName("en");

        // Assert
        Assert.True(result == "Magyarország" || result == "Ungarn");
    }

    [Fact]
    public void GetCountryName_ShouldReturnEmpty_WhenNoNamesAvailable()
    {
        // Arrange
        var format = new CountryVatFormat
        {
            CountryNames = new Dictionary<string, string>()
        };

        // Act
        var result = format.GetCountryName();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetCountryName_ShouldBeCaseInsensitive()
    {
        // Arrange
        var format = new CountryVatFormat
        {
            CountryNames = new Dictionary<string, string>
            {
                {"en", "Hungary"},
                {"hu", "Magyarország"}
            }
        };

        // Act
        var result1 = format.GetCountryName("HU");
        var result2 = format.GetCountryName("hu");
        var result3 = format.GetCountryName("Hu");

        // Assert
        Assert.Equal("Magyarország", result1);
        Assert.Equal("Magyarország", result2);
        Assert.Equal("Magyarország", result3);
    }

    [Fact]
    public void CountryName_LegacyProperty_ShouldReturnEnglishName()
    {
        // Arrange
        var format = new CountryVatFormat
        {
            CountryNames = new Dictionary<string, string>
            {
                {"en", "Hungary"},
                {"hu", "Magyarország"}
            }
        };

        // Act
#pragma warning disable CS0618 // Type or member is obsolete
        var result = format.CountryName;
#pragma warning restore CS0618 // Type or member is obsolete

        // Assert
        Assert.Equal("Hungary", result);
    }
}