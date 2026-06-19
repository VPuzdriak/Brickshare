using System.ComponentModel.DataAnnotations;
using Brickshare.Catalog.WebApi.Features.Shared;

namespace Brickshare.Catalog.WebApi.Features.UpdateLegoSet;

public sealed record UpdateLegoSetRequest(
    [Required] string Name,
    [Required] string Theme,
    [Required, Range(10, 2_000)] decimal CatalogPrice,
    [Required, Range(12, 13_000)] int NumberOfPieces,
    [Required, Range(4, 99)] int AgeRestriction,
    [Required, Range(1, 60)] int AssemblyTimeInDays);

internal static class UpdateLegoSetEndpoint
{
    public static IEndpointRouteBuilder MapUpdateLegoSet(this IEndpointRouteBuilder builder)
    {
        builder.MapPut("/{id}/{themeSlug}", async (
                string id,
                string themeSlug,
                UpdateLegoSetRequest updateLegoSetRequest,
                UpdateLegoSetHandler handler,
                CancellationToken ct) =>
            {
                var command = new UpdateLegoSet(
                    id,
                    themeSlug,
                    updateLegoSetRequest.Name,
                    updateLegoSetRequest.Theme,
                    updateLegoSetRequest.CatalogPrice,
                    updateLegoSetRequest.NumberOfPieces,
                    updateLegoSetRequest.AgeRestriction,
                    updateLegoSetRequest.AssemblyTimeInDays);

                var result = await handler.HandleAsync(command, ct);

                if (result.Success)
                {
                    return Results.Ok(result.Data);
                }

                return result.Error switch
                {
                    LegoSetNotFound notFound => Results.NotFound(notFound),
                    _ => Results.Problem(title: result.Error.Code, detail: result.Error.Message)
                };
            })
            .WithName("UpdateLegoSet");

        return builder;
    }
}