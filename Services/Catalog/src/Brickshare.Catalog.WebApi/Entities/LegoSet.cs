namespace Brickshare.Catalog.WebApi.Entities;

internal sealed class LegoSet
{
    public Guid Guid { get; init; }
    public required string Name { get; set; }
    public required string Theme { get; init; }
    public required decimal CatalogPrice { get; set; }
    public required int NumberOfPieces { get; set; }
    public required int AgeRestriction { get; set; }
    public required int AssemblyTimeInDays { get; set; }
}