using Brickshare.Catalog.WebApi.Entities;
using Brickshare.Catalog.WebApi.Infrastructure.Extensions;
using Microsoft.Azure.Cosmos;

namespace Brickshare.Catalog.WebApi.Infrastructure;

internal interface ILegoSetDataStore
{
    Task<LegoSetCosmos?> GetAsync(string id, string themeSlug, CancellationToken cancellationToken);
    Task<LegoSetCosmos> AddAsync(LegoSet legoSet, CancellationToken cancellationToken);
}

internal sealed class CosmosDbLegoSetDataStore(Container container) : ILegoSetDataStore
{
    public async Task<LegoSetCosmos?> GetAsync(string id, string themeSlug, CancellationToken cancellationToken)
    {
        try
        {
            var response = await container.ReadItemAsync<LegoSetCosmos>(
                id,
                new PartitionKey(themeSlug),
                cancellationToken: cancellationToken);

            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<LegoSetCosmos> AddAsync(LegoSet legoSet, CancellationToken cancellationToken)
    {
        var document = legoSet.ToCosmos();
        await container.CreateItemAsync(
            document,
            new PartitionKey(document.ThemeSlug),
            cancellationToken: cancellationToken);

        return document;
    }
}

internal sealed class LegoSetCosmos
{
    public required string Id { get; init; }
    public required string ThemeSlug { get; init; }
    public required string Theme { get; init; }
    public required string Name { get; set; }
    public required decimal CatalogPrice { get; set; }
    public required int NumberOfPieces { get; set; }
    public required int AgeRestriction { get; set; }
    public required int AssemblyTimeInDays { get; set; }
}