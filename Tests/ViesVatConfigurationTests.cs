using ViesApi.Configuration;

namespace ViesApi.Tests;

public class ViesVatConfigurationTests
{
    [Fact]
    public void Formats_ShouldContainAllEUCountries()
    {
        // Assert
        Assert.True(ViesVatConfiguration.Formats.Count >= 27);
        
        // Test some key EU countries
        Assert.True(ViesVatConfiguration.Formats.ContainsKey("HU"));
        Assert.True(ViesVatConfiguration.Formats.ContainsKey("DE"));
        Assert.True(ViesVatConfiguration.Formats.ContainsKey("FR"));
        Assert.True(ViesVatConfiguration.Formats.ContainsKey("AT"));
        Assert.True(ViesVatConfiguration.Formats.ContainsKey("NL"));
    }

    [Fact]
    public void GetCountryNames_ShouldReturnEnglishByDefault()
    {
        // Act
        var result = ViesVatConfiguration.GetCountryNames();

        // Assert
        Assert.Equal("Hungary", result["HU"]);
        Assert.Equal("Germany", result["DE"]);
        Assert.Equal("France", result["FR"]);
    }

    [Fact]
    public void GetCountryNames_ShouldReturnHungarianWhenRequested()
    {
        // Act
        var result = ViesVatConfiguration.GetCountryNames("hu");

        // Assert
        Assert.Equal("Magyarország", result["HU"]);
        Assert.Equal("Németország", result["DE"]);
        Assert.Equal("Franciaország", result["FR"]);
    }

    [Fact]
    public void GetCountryNames_ShouldReturnAllCountries()
    {
        // Act
        var result = ViesVatConfiguration.GetCountryNames();

        // Assert
        Assert.True(result.Count >= 27);
        Assert.All(ViesVatConfiguration.Formats.Keys, countryCode => 
            Assert.True(result.ContainsKey(countryCode)));
    }

    [Fact]
    public void GetSupportedLanguages_ShouldReturnLanguageList()
    {
        // Act
        var result = ViesVatConfiguration.GetSupportedLanguages();

        // Assert
        Assert.Contains("en", result);
        Assert.Contains("hu", result);
        Assert.Contains("de", result);
        Assert.Contains("fr", result);
        Assert.True(result.Count >= 10);
    }

    [Fact]
    public void GetSupportedLanguages_ShouldReturnSortedList()
    {
        // Act
        var result = ViesVatConfiguration.GetSupportedLanguages();

        // Assert
        var sortedResult = result.OrderBy(x => x).ToList();
        Assert.Equal(sortedResult, result);
    }
}