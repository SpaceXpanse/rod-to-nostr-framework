using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Relay.Data;

namespace Relay
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextFactory<RelayDbContext>(builder =>
            {
                var connString = _configuration.GetConnectionString(RelayDbContext.DatabaseConnectionStringName);
                if (string.IsNullOrEmpty(connString))
                {
                    throw new Exception("Database: Connection string not set");
                }
                builder.UseNpgsql(connString, optionsBuilder => { optionsBuilder.EnableRetryOnFailure(10); });
            }, ServiceLifetime.Singleton);
            services.AddHostedService<MigrationHostedService>();
            services.AddLogging();
            services.AddSingleton<NostrEventService>();
            services.AddSingleton<StateManager>();
            services.AddSingleton<ConnectionManager>();
            services.AddSingleton<WebSocketHandler>();
            services.AddSingleton<WebsocketMiddleware>();
            services.AddSingleton<INostrMessageHandler, CloseNostrMessageHandler>();
            services.AddSingleton<INostrMessageHandler, EventNostrMessageHandler>();
            services.AddSingleton<INostrMessageHandler, RequestNostrMessageHandler>();
            services.AddSingleton<IHostedService>(provider => provider.GetService<ConnectionManager>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebSocketHandler webSocketHandler)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseWebSockets();
            app.UseMiddleware<WebsocketMiddleware>();
        }
    }
}