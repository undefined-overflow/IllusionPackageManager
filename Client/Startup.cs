using Client.Managers;
using Client.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octokit;

namespace Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) => services
            .AddSingleton<Api>()
            .AddSingleton<PluginManager>()
            .AddSingleton<GamesRepository>()
            .AddSingleton<PluginsRepository>()
            .AddSingleton(new GitHubClient(new ProductHeaderValue("IllusionPackageManager")))
            .AddControllers();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, GamesRepository games, PluginsRepository plugins)
        {
            lifetime.ApplicationStopping.Register(() =>
            {
                plugins.Dispose();
                games.Dispose();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseRouting()
                .UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader())
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}