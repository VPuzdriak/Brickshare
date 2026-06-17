using System.ComponentModel.DataAnnotations;

namespace Brickshare.Catalog.WebApi.Features.UpdateLegoSet;

public sealed record UpdateLegoSetRequest(
    [Required] string Name,
    [Required, Range(10, 2_000)] decimal CatalogPrice,
    [Required, Range(12, 13_000)] int NumberOfPieces,
    [Required, Range(4, 99)] int AgeRestriction,
    [Required, Range(1, 60)] int AssemblyTimeInDays);

internal static class UpdateLegoSetEndpoint
{
    public static IEndpointRouteBuilder MapUpdateLegoSet(this IEndpointRouteBuilder builder)
    {
        builder.MapPut("/{id:guid}", async (
                Guid id,
                UpdateLegoSetRequest updateLegoSetRequest,
                UpdateLegoSetHandler handler,
                CancellationToken ct) =>
            {
                var command = new UpdateLegoSet(
                    id,
                    updateLegoSetRequest.Name,
                    updateLegoSetRequest.CatalogPrice,
                    updateLegoSetRequest.NumberOfPieces,
                    updateLegoSetRequest.AgeRestriction,
                    updateLegoSetRequest.AssemblyTimeInDays);

                var result = await handler.HandleAsync(command, ct);

                if (result.Success)
                {
                    return Results.NoContent();
                }

                return result.Error.Code switch
                {
                    "SET_NOT_FOUND" => Results.NotFound(result.Error),
                    _ => Results.Problem(result.Error.Message)
                };
            })
            .WithName("UpdateLegoSet");

        return builder;
    }
}