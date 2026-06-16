namespace Brickshare.Catalog.WebApi.Features.GetLegoSet;

internal static class GetLegoSetEndpoint
{
    public static IEndpointRouteBuilder MapGetLegoSet(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/{id:guid}", async (
                Guid id,
                GetLegoSetHandler handler,
                CancellationToken ct) =>
            {
                var legoSet = await handler.QueryAsync(id, ct);

                if (legoSet is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(legoSet);
            })
            .WithName("GetLegoSet");
        
        return builder;
    }
}