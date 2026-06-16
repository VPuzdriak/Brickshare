using Brickshare.Catalog.WebApi.Entities;

namespace Brickshare.Catalog.WebApi.Features.CreateLegoSet;

internal sealed record CreateLegoSet(
    string Name,
    decimal CatalogPrice,
    int NumberOfPieces,
    int AgeRestriction,
    int AssemblyTimeInDays
);

internal sealed class CreateLegoSetHandler
{
    public async Task<Guid> HandleAsync(CreateLegoSet command, CancellationToken cancellationToken)
    {
        var legoSet = new LegoSet
        {
            Guid = Guid.NewGuid(),
            Name = command.Name,
            CatalogPrice = command.CatalogPrice,
            AgeRestriction = command.AgeRestriction,
            AssemblyTimeInDays = command.AssemblyTimeInDays,
            NumberOfPieces = command.NumberOfPieces
        };
        
        return legoSet.Guid;
    }
}