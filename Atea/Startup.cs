using Atea.Core.Configuration;
using Atea.Core.Services;
using Atea.Core.Validators;
using Atea.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Atea.Startup))]

namespace Atea
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IBlobService, BlobService>();

            builder.Services.AddTransient<ITableService, TableService>();

            builder.Services.AddTransient<IDateValidator, DateValidator>();

            builder.Services.AddSingleton<IHTTPService, HTTPService>();

            builder.Services.AddSingleton<IStorageConfiguration, StorageConfiguration>();
        }
    }
}
