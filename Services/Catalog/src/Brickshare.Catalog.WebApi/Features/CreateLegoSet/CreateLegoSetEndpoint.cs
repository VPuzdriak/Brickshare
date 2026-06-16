namespace Brickshare.Catalog.WebApi.Features.CreateLegoSet;

internal static class CreateLegoSetEndpoint
{
    private sealed record CreateLegoSetRequest(
        string Name,
        decimal CatalogPrice,
        int NumberOfPieces,
        int AgeRestriction,
        int AssemblyTimeInDays);

    public static IEndpointRouteBuilder MapCreateLegoSet(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/", async (
                CreateLegoSetRequest createLegoSetRequest,
                CreateLegoSetHandler handler,
                CancellationToken ct) =>
            {
                var command = new CreateLegoSet(
                    createLegoSetRequest.Name,
                    createLegoSetRequest.CatalogPrice,
                    createLegoSetRequest.NumberOfPieces,
                    createLegoSetRequest.AgeRestriction,
                    createLegoSetRequest.AssemblyTimeInDays);

                var setId = await handler.HandleAsync(command, ct);

                return Results.CreatedAtRoute("GetLegoSet", new { id = setId }, setId);
            })
            .WithName("CreateLegoSet");

        return builder;
    }
}