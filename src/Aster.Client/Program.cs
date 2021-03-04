using System;
using Aster.Client.Base;
using Aster.Client.World;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Aster.Client
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = CreateServiceProvider();

            var game = serviceProvider.GetService<ClientGame>();
            game!.Run();
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.File("client.log")
                .CreateLogger();

            var chunkCreatorOptions = new ChunkCreatorOptions();
            configuration.GetSection(nameof(ChunkCreator)).Bind(chunkCreatorOptions);
            var chunkProviderOptions = new ChunkProviderOptions();
            configuration.GetSection(nameof(ChunkProvider)).Bind(chunkProviderOptions);

            var services = new ServiceCollection();
            services.AddSingleton(configuration);
            services.AddSingleton(Log.Logger);
            services.AddSingleton(new GameSettings(1280, 720, false));
            services.AddSingleton(chunkCreatorOptions);
            services.AddSingleton(chunkProviderOptions);
            services.AddSingleton<INoiseGenerator, NoiseGenerator>();
            services.AddSingleton<IInputLayoutFactory, InputLayoutFactory>();
            services.AddSingleton<IChunkCreator, ChunkCreator>();
            services.AddSingleton<IChunkLoader, ChunkLoader>();
            services.AddSingleton<IChunkSaver, ChunkSaver>();
            services.AddSingleton<IChunkProvider, ChunkProvider>();
            services.AddSingleton<ClientGame>();
            services.AddSingleton<IShaderFactory, ShaderFactory>();
            return services.BuildServiceProvider();
        }
    }
}
