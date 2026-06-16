using System.ComponentModel.DataAnnotations;

namespace Brickshare.Catalog.WebApi.Features.CreateLegoSet;

public sealed record CreateLegoSetRequest(
    [Required] string Name,
    [Required, Range(10, 2_000)] decimal CatalogPrice,
    [Required, Range(12, 13_000)] int NumberOfPieces,
    [Required, Range(4, 99)] int AgeRestriction,
    [Required, Range(1, 60)] int AssemblyTimeInDays);

internal static class CreateLegoSetEndpoint
{
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