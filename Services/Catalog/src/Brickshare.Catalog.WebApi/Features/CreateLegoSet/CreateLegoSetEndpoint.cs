using System.ComponentModel.DataAnnotations;

namespace Brickshare.Catalog.WebApi.Features.CreateLegoSet;

public sealed record CreateLegoSetRequest(
    [Required] string Name,
    [Required] string Theme,
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
                    createLegoSetRequest.Theme,
                    createLegoSetRequest.CatalogPrice,
                    createLegoSetRequest.NumberOfPieces,
                    createLegoSetRequest.AgeRestriction,
                    createLegoSetRequest.AssemblyTimeInDays);

                var result = await handler.HandleAsync(command, ct);

                if (result.Success)
                {
                    return Results.CreatedAtRoute(
                        "GetLegoSet",
                        new { id = result.Data.Id, themeSlug = result.Data.ThemeSlug },
                        result.Data
                    );
                }

                return Results.Problem(title: result.Error.Code, detail: result.Error.Message);
            })
            .WithName("CreateLegoSet");

        return builder;
    }
}