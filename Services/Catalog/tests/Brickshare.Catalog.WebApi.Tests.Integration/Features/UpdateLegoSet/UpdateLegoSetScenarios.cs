using System.Net;
using System.Net.Http.Json;
using Brickshare.Catalog.WebApi.Features.CreateLegoSet;
using Brickshare.Catalog.WebApi.Features.GetLegoSet;
using Brickshare.Catalog.WebApi.Features.UpdateLegoSet;
using Shouldly;

namespace Brickshare.Catalog.WebApi.Tests.Integration.Features.UpdateLegoSet;

public sealed class UpdateLegoSetScenarios(BrickShareFactory factory) : IClassFixture<BrickShareFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task UpdateLegoSet_ShouldUpdateAndReturnOk_WhenExists()
    {
        // Arrange
        var createLegoSetResult = await CreateLegoSetAsync();
        var legoSet = await GetLegoSetAsync(createLegoSetResult.Id, createLegoSetResult.ThemeSlug);
        var request = new UpdateLegoSetRequest(
            legoSet.Name,
            legoSet.Theme,
            legoSet.CatalogPrice,
            legoSet.NumberOfPieces,
            legoSet.AgeRestriction,
            legoSet.AssemblyTimeInDays + 1
        );

        // Act
        var response =
            await _httpClient.PutAsJsonAsync(
                $"/lego-sets/{createLegoSetResult.Id}/{createLegoSetResult.ThemeSlug}",
                request
            );
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<UpdateLegoSetResult>();
        ArgumentNullException.ThrowIfNull(result);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.Id.ShouldBe(createLegoSetResult.Id);
    }

    [Fact]
    public async Task UpdateLegoSet_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        var nonExistingLegoSetId = "ca211935-3bfa-4a73-8e2b-7f2cd3637f50";
        var themeSlug = "technic";
        var request = new UpdateLegoSetRequest(
            Name: "LEGO Star Wars Millennium Falcon",
            Theme: "Star Wars",
            CatalogPrice: 679.99m,
            NumberOfPieces: 10294,
            AgeRestriction: 18,
            AssemblyTimeInDays: 15);

        // Act
        var response = await _httpClient.PutAsJsonAsync($"/lego-sets/{nonExistingLegoSetId}/{themeSlug}", request);

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
        var request = new CreateLegoSetRequest(name, theme, catalogPrice, numberOfPieces, ageRestriction,
            assemblyTimeInDays);
        var response = await _httpClient.PostAsJsonAsync("/lego-sets", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CreateLegoSetResult>();
        ArgumentNullException.ThrowIfNull(result);

        return result;
    }

    private async Task<GetLegoSetResponse> GetLegoSetAsync(string setId, string themeSlug)
    {
        var response = await _httpClient.GetAsync($"/lego-sets/{setId}/{themeSlug}");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadFromJsonAsync<GetLegoSetResponse>();
        ArgumentNullException.ThrowIfNull(responseBody);

        return responseBody;
    }
}