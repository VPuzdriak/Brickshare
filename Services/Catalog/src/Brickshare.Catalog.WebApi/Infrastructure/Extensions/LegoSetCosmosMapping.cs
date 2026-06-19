using Brickshare.Catalog.WebApi.Entities;

namespace Brickshare.Catalog.WebApi.Infrastructure.Extensions;

internal static class LegoSetCosmosMapping
{
    public static LegoSetCosmos ToCosmos(this LegoSet legoSet) =>
        new()
        {
            Id = legoSet.Id,
            ThemeSlug = MakeSlug(legoSet.Theme),
            Name = legoSet.Name,
            Theme = legoSet.Theme,
            CatalogPrice = legoSet.CatalogPrice,
            NumberOfPieces = legoSet.NumberOfPieces,
            AgeRestriction = legoSet.AgeRestriction,
            AssemblyTimeInDays = legoSet.AssemblyTimeInDays
        };

    public static LegoSet ToDomain(this LegoSetCosmos legoSetCosmos) =>
        new()
        {
            Id = legoSetCosmos.Id,
            Name = legoSetCosmos.Name,
            Theme = legoSetCosmos.Theme,
            CatalogPrice = legoSetCosmos.CatalogPrice,
            NumberOfPieces = legoSetCosmos.NumberOfPieces,
            AgeRestriction = legoSetCosmos.AgeRestriction,
            AssemblyTimeInDays = legoSetCosmos.AssemblyTimeInDays
        };

    private static string MakeSlug(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
        }

        var slug = value.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("_", "-");

        return slug;
    }
}