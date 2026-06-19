using Brickshare.Catalog.WebApi.Features.CreateLegoSet;
using Brickshare.Catalog.WebApi.Features.GetLegoSet;
using Brickshare.Catalog.WebApi.Features.UpdateLegoSet;
using Brickshare.Catalog.WebApi.Infrastructure;
using Microsoft.Azure.Cosmos;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<CreateLegoSetHandler>();
builder.Services.AddScoped<GetLegoSetHandler>();
builder.Services.AddScoped<UpdateLegoSetHandler>();

builder.Services.AddSingleton<CosmosClient>(_ => new CosmosClient(builder.Configuration["CosmosDb:ConnectionString"],
    new CosmosClientOptions
    {
        SerializerOptions = new CosmosSerializationOptions
        {
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
            IgnoreNullValues = true
        }
    }));

builder.Services.AddSingleton<ILegoSetDataStore>(sp =>
{
    var cosmosClient = sp.GetRequiredService<CosmosClient>();
    var container = cosmosClient.GetContainer(
        builder.Configuration["CosmosDb:DatabaseName"],
        builder.Configuration["CosmosDb:ContainerName"]
    );

    return new CosmosDbLegoSetDataStore(container);
});

builder.Services.AddValidation();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options => options.WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch));

app.MapGroup("/lego-sets")
    .MapCreateLegoSet()
    .MapGetLegoSet()
    .MapUpdateLegoSet();

app.Run();