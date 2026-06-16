using Brickshare.Catalog.WebApi.Features.CreateLegoSet;
using Brickshare.Catalog.WebApi.Features.GetLegoSet;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<CreateLegoSetHandler>();
builder.Services.AddScoped<GetLegoSetHandler>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options => options.WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch));

app.MapGroup("/lego-sets")
    .MapCreateLegoSet()
    .MapGetLegoSet();

app.Run();