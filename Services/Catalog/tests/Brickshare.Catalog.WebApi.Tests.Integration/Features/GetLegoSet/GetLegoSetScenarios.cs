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
        var legoSetId = await CreateLegoSet(legoSetName);

        // Act
        var response = await _httpClient.GetAsync($"/lego-sets/{legoSetId}");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadFromJsonAsync<GetLegoSetResponse>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        responseBody.ShouldNotBeNull();
        responseBody.Id.ShouldBe(legoSetId);
        responseBody.Name.ShouldBe(legoSetName);
    }

    [Fact]
    public async Task GetLegoSet_ShouldReturnNotFound_WhenSetDoesNotExist()
    {
        // Arrange
        var nonExistingLegoSetId = Guid.Empty;
        
        // Act
        var response = await _httpClient.GetAsync($"/lego-sets/{nonExistingLegoSetId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private async Task<Guid> CreateLegoSet(
        string name = "LEGO Star Wars Millennium Falcon",
        decimal catalogPrice = 679.99m,
        int numberOfPieces = 10294,
        int ageRestriction = 18,
        int assemblyTimeInDays = 15)
    {
        var request = new CreateLegoSetRequest(name, catalogPrice, numberOfPieces, ageRestriction, assemblyTimeInDays);
        var response = await _httpClient.PostAsJsonAsync("/lego-sets", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>();
    }
}