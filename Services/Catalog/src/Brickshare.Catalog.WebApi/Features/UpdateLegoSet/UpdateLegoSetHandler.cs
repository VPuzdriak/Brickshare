using Brickshare.Catalog.WebApi.Abstractions;
using Brickshare.Catalog.WebApi.Entities;

namespace Brickshare.Catalog.WebApi.Features.UpdateLegoSet;

internal sealed record UpdateLegoSet(
    Guid Id,
    string Name,
    string Theme,
    decimal CatalogPrice,
    int NumberOfPieces,
    int AgeRestriction,
    int AssemblyTimeInDays);

internal sealed class UpdateLegoSetHandler
{
    public async Task<Result<Empty>> HandleAsync(UpdateLegoSet command, CancellationToken cancellationToken)
    {
        // Simulation of waiting to get the set from DB
        await Task.Delay(100, cancellationToken);

        if (command.Id == Guid.Empty)
        {
            return new Failure("SET_NOT_FOUND", "Lego set not found");
        }

        var legoSet = new LegoSet
        {
            Guid = command.Id,
            Theme = command.Theme,
            Name = command.Name,
            CatalogPrice = command.CatalogPrice,
            AgeRestriction = command.AgeRestriction,
            AssemblyTimeInDays = command.AssemblyTimeInDays,
            NumberOfPieces = command.NumberOfPieces
        };

        // Simulation of saving changes
        await Task.Delay(100, cancellationToken);

        return Result.Empty;
    }
}