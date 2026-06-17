namespace Brickshare.Catalog.WebApi.Features.GetLegoSet;

internal sealed record LegoSetDto(
    Guid Id,
    string Name,
    string Theme,
    decimal CatalogPrice,
    int NumberOfPieces,
    int AgeRestriction,
    int AssemblyTimeInDays);

internal sealed class GetLegoSetHandler
{
    public async Task<LegoSetDto?> QueryAsync(Guid id, CancellationToken cancellationTokens)
    {
        await Task.Delay(100, cancellationTokens);

        if (id == Guid.Empty)
        {
            return null;
        }

        return new LegoSetDto(id, "Titanic", "Icons", 679.99m, 10294, 18, 15);
    }
}