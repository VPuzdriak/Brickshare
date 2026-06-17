using Brickshare.Catalog.WebApi.Entities;
using Brickshare.Catalog.WebApi.Infrastructure;

namespace Brickshare.Catalog.WebApi.Features.CreateLegoSet;

internal sealed record CreateLegoSet(
    string Name,
    string Theme,
    decimal CatalogPrice,
    int NumberOfPieces,
    int AgeRestriction,
    int AssemblyTimeInDays
);

internal sealed class CreateLegoSetHandler(ILegoSetDataStore legoSetDataStore)
{
    public async Task<CreateLegoSetResult> HandleAsync(CreateLegoSet command, CancellationToken cancellationToken)
    {
        var legoSet = new LegoSet
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = command.Name,
            Theme = command.Theme,
            CatalogPrice = command.CatalogPrice,
            AgeRestriction = command.AgeRestriction,
            AssemblyTimeInDays = command.AssemblyTimeInDays,
            NumberOfPieces = command.NumberOfPieces
        };

        var legoSetCosmos = await legoSetDataStore.AddAsync(legoSet, cancellationToken);

        return new CreateLegoSetResult(legoSetCosmos.Id, legoSetCosmos.ThemeSlug);
    }
}