using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Brickshare.Catalog.WebApi.Tests.Integration;

public class BrickShareFactory : WebApplicationFactory<BrickshareWebApiMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config => config.AddEnvironmentVariables());
    }
}