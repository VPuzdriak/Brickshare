using Brickshare.Catalog.WebApi.Abstractions;
using Brickshare.Catalog.WebApi.Entities;
using Brickshare.Catalog.WebApi.Features.Shared;
using Brickshare.Catalog.WebApi.Infrastructure;

namespace Brickshare.Catalog.WebApi.Features.UpdateLegoSet;

internal sealed record UpdateLegoSet(
    string Id,
    string ThemeSlug,
    string Name,
    string Theme,
    decimal CatalogPrice,
    int NumberOfPieces,
    int AgeRestriction,
    int AssemblyTimeInDays);

internal sealed class UpdateLegoSetHandler(ILegoSetDataStore legoSetDataStore)
{
    public async Task<Result<UpdateLegoSetResult>> HandleAsync(
        UpdateLegoSet command,
        CancellationToken cancellationToken)
    {
        var existingSet = await legoSetDataStore.GetAsync(command.Id, command.ThemeSlug, cancellationToken);
        if (existingSet is null)
        {
            return new LegoSetNotFound();
        }

        var legoSet = new LegoSet
        {
            Id = command.Id,
            Theme = command.Theme,
            Name = command.Name,
            CatalogPrice = command.CatalogPrice,
            AgeRestriction = command.AgeRestriction,
            AssemblyTimeInDays = command.AssemblyTimeInDays,
            NumberOfPieces = command.NumberOfPieces
        };

        var legoSetCosmos = await legoSetDataStore.ReplaceAsync(legoSet, command.ThemeSlug, cancellationToken);

        return new UpdateLegoSetResult(legoSetCosmos.Id, legoSetCosmos.ThemeSlug);
    }
}