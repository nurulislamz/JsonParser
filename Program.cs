using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace JsonParser
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            var app = serviceProvider.GetRequiredService<App>();
            return app.Run(args);
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<App>();
            services.AddTransient<IJsonLexerFactory, JsonLexerFactory>();
            services.AddTransient<IJsonLexer, JsonLexer>();
            services.AddTransient<IJsonParser, JsonParser>();

            return services.BuildServiceProvider();
        
        }
    }
}