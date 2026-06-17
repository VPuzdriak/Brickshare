namespace Brickshare.Catalog.WebApi.Entities;

internal sealed class LegoSet
{
    public required string Id { get; init; }
    public required string Name { get; set; }
    public required string Theme { get; set; }
    public required decimal CatalogPrice { get; set; }
    public required int NumberOfPieces { get; set; }
    public required int AgeRestriction { get; set; }
    public required int AssemblyTimeInDays { get; set; }
}