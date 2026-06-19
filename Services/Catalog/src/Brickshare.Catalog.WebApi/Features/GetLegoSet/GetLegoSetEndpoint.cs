namespace Brickshare.Catalog.WebApi.Features.GetLegoSet;

public sealed record GetLegoSetResponse(
    string Id,
    string Name,
    string Theme,
    decimal CatalogPrice,
    int NumberOfPieces,
    int AgeRestriction,
    int AssemblyTimeInDays);

internal static class GetLegoSetEndpoint
{
    public static IEndpointRouteBuilder MapGetLegoSet(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/{id}/{themeSlug}", async (
                string id,
                string themeSlug,
                GetLegoSetHandler handler,
                CancellationToken ct) =>
            {
                LegoSetDto? dto = await handler.QueryAsync(new GetLegoSet(id, themeSlug), ct);

                if (dto is null)
                {
                    return Results.NotFound();
                }

                var response = new GetLegoSetResponse(
                    dto.Id,
                    dto.Name,
                    dto.Theme,
                    dto.CatalogPrice,
                    dto.NumberOfPieces,
                    dto.AgeRestriction,
                    dto.AssemblyTimeInDays
                );

                return Results.Ok(response);
            })
            .WithName("GetLegoSet");

        return builder;
    }
}