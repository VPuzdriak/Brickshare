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
    public async Task UpdateLegoSet_ShouldUpdateAndReturnNoContent_WhenExists()
    {
        // Arrange
        var legoSetId = await CreateLegoSetAsync();
        var legoSet = await GetLegoSetAsync(legoSetId);
        var request = new UpdateLegoSetRequest(
            legoSet.Name,
            legoSet.Theme,
            legoSet.CatalogPrice,
            legoSet.NumberOfPieces,
            legoSet.AgeRestriction,
            legoSet.AssemblyTimeInDays + 1
        );

        // Act
        var response = await _httpClient.PutAsJsonAsync($"/lego-sets/{legoSetId}", request);
        response.EnsureSuccessStatusCode();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateLegoSet_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        var nonExistingLegoSetId = Guid.Empty;
        var request = new UpdateLegoSetRequest(
            Name: "LEGO Star Wars Millennium Falcon",
            Theme: "Star Wars",
            CatalogPrice: 679.99m,
            NumberOfPieces: 10294,
            AgeRestriction: 18,
            AssemblyTimeInDays: 15);

        // Act
        var response = await _httpClient.PutAsJsonAsync($"/lego-sets/{nonExistingLegoSetId}", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private async Task<Guid> CreateLegoSetAsync(
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
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    private async Task<GetLegoSetResponse> GetLegoSetAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"/lego-sets/{id}");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadFromJsonAsync<GetLegoSetResponse>();
        ArgumentNullException.ThrowIfNull(responseBody);

        return responseBody;
    }
}