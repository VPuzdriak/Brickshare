using System.Net;
using System.Net.Http.Json;
using Brickshare.Catalog.WebApi.Features.CreateLegoSet;
using Brickshare.Catalog.WebApi.Features.GetLegoSet;
using Shouldly;

namespace Brickshare.Catalog.WebApi.Tests.Integration.Features.GetLegoSet;

public sealed class GetLegoSetScenarios(BrickShareFactory factory) : IClassFixture<BrickShareFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task GetLegoSet_ShouldReturnOk_WhenSetExists()
    {
        // Arrange
        const string legoSetName = "Titanic";
        var createLegoSetResult = await CreateLegoSetAsync(legoSetName);

        // Act
        var response =
            await _httpClient.GetAsync($"/lego-sets/{createLegoSetResult.Id}/{createLegoSetResult.ThemeSlug}");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadFromJsonAsync<GetLegoSetResponse>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        responseBody.ShouldNotBeNull();
        responseBody.Id.ShouldBe(createLegoSetResult.Id);
        responseBody.Name.ShouldBe(legoSetName);
    }

    [Fact]
    public async Task GetLegoSet_ShouldReturnNotFound_WhenSetDoesNotExist()
    {
        // Arrange
        var nonExistingLegoSetId = "abc123";
        var legoSetThemeSlug = "titanic";

        // Act
        var response = await _httpClient.GetAsync($"/lego-sets/{nonExistingLegoSetId}/{legoSetThemeSlug}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private async Task<CreateLegoSetResult> CreateLegoSetAsync(
        string name = "LEGO Star Wars Millennium Falcon",
        string theme = "Star Wars",
        decimal catalogPrice = 679.99m,
        int numberOfPieces = 10294,
        int ageRestriction = 18,
        int assemblyTimeInDays = 15)
    {
        var request = new CreateLegoSetRequest(
            name,
            theme,
            catalogPrice,
            numberOfPieces,
            ageRestriction,
            assemblyTimeInDays);

        var response = await _httpClient.PostAsJsonAsync("/lego-sets", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CreateLegoSetResult>();
        ArgumentNullException.ThrowIfNull(result);

        return result;
    }
}