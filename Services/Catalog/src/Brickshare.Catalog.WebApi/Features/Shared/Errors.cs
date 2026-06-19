using Brickshare.Catalog.WebApi.Abstractions;

namespace Brickshare.Catalog.WebApi.Features.Shared;

public sealed record LegoSetNotFound() : Failure("SET_NOT_FOUND", "Lego set not found");