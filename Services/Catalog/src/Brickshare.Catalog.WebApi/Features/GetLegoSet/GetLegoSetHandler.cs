using Brickshare.Catalog.WebApi.Infrastructure;
using Brickshare.Catalog.WebApi.Infrastructure.Extensions;

namespace Brickshare.Catalog.WebApi.Features.GetLegoSet;

internal sealed record LegoSetDto(
    string Id,
    string Name,
    string Theme,
    decimal CatalogPrice,
    int NumberOfPieces,
    int AgeRestriction,
    int AssemblyTimeInDays);

internal sealed record GetLegoSet(string Id, string ThemeSlug);

internal sealed class GetLegoSetHandler(ILegoSetDataStore legoSetDataStore)
{
    public async Task<LegoSetDto?> QueryAsync(GetLegoSet query, CancellationToken cancellationTokens)
    {
        await Task.Delay(100, cancellationTokens);

        if (string.IsNullOrEmpty(query.Id) || string.IsNullOrEmpty(query.ThemeSlug))
        {
            return null;
        }

        var legoSetCosmos = await legoSetDataStore.GetAsync(query.Id, query.ThemeSlug, cancellationTokens);

        if (legoSetCosmos is null)
        {
            return null;
        }

        return new LegoSetDto(
            legoSetCosmos.Id,
            legoSetCosmos.Name,
            legoSetCosmos.Theme,
            legoSetCosmos.CatalogPrice,
            legoSetCosmos.NumberOfPieces,
            legoSetCosmos.AgeRestriction,
            legoSetCosmos.AssemblyTimeInDays
        );
    }
}