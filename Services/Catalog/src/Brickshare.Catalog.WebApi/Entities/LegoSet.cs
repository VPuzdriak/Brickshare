namespace Brickshare.Catalog.WebApi.Entities;

internal sealed class LegoSet
{
    public Guid Guid { get; init; }
    public string Name { get; set; }
    public decimal CatalogPrice { get; set; }
    public int NumberOfPieces { get; set; }
    public int AgeRestriction { get; set; }
    public int AssemblyTimeInDays { get; set; }
}