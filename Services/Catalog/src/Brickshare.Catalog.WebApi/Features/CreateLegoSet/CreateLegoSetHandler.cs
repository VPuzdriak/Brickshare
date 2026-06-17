using Brickshare.Catalog.WebApi.Entities;

namespace Brickshare.Catalog.WebApi.Features.CreateLegoSet;

internal sealed record CreateLegoSet(
    string Name,
    string Theme,
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
            Theme = command.Theme,
            CatalogPrice = command.CatalogPrice,
            AgeRestriction = command.AgeRestriction,
            AssemblyTimeInDays = command.AssemblyTimeInDays,
            NumberOfPieces = command.NumberOfPieces
        };
     
        await Task.Delay(100, cancellationToken);
        
        return legoSet.Guid;
    }
}