namespace Brickshare.Catalog.WebApi.Features.GetLegoSet;

internal sealed class GetLegoSetHandler
{
    public async Task<LegoSetDto?> QueryAsync(Guid id, CancellationToken cancellationTokens)
    {
        await Task.Delay(100, cancellationTokens);
        return new LegoSetDto(id, "Titanic", 679.99m, 10294, 18, 15);
    }
}

internal sealed record LegoSetDto(
    Guid Id,
    string Name,
    decimal CatalogPrice,
    int NumberOfPieces,
    int AgeRestriction,
    int AssemblyTimeInDays);