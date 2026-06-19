using System.Net;
using System.Net.Http.Json;
using Brickshare.Catalog.WebApi.Features.CreateLegoSet;
using Shouldly;

namespace Brickshare.Catalog.WebApi.Tests.Integration.Features.CreateLegoSet;

public sealed class CreateLegoSetScenarios(BrickShareFactory factory) : IClassFixture<BrickShareFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task CreateLegoSet_ShouldReturnCreated_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateLegoSetRequest(
            Name: "LEGO Star Wars Millennium Falcon",
            Theme: "Star Wars",
            CatalogPrice: 679.99m,
            NumberOfPieces: 10294,
            AgeRestriction: 18,
            AssemblyTimeInDays: 15
        );

        // Act
        var response = await _httpClient.PostAsJsonAsync("/lego-sets", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CreateLegoSetResult>();

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        result.ShouldNotBeNull();
        ArgumentNullException.ThrowIfNull(response.Headers.Location);
        response.Headers.Location.ShouldBe(
            new Uri(_httpClient.BaseAddress!, $"lego-sets/{result.Id}/{result.ThemeSlug}"));
    }

    [Fact]
    public async Task CreateLegoSet_ShouldReturnBadRequest_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new CreateLegoSetRequest(
            Name: "", // Invalid: Name is required
            Theme: "", // Invalid: Theme is required
            CatalogPrice: -10.00m, // Invalid: Price cannot be negative
            NumberOfPieces: 0, // Invalid: Number of pieces must be greater than 0
            AgeRestriction: -1, // Invalid: Age restriction cannot be negative
            AssemblyTimeInDays: -5 // Invalid: Assembly time cannot be negative
        );

        // Act
        var response = await _httpClient.PostAsJsonAsync("/lego-sets", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}