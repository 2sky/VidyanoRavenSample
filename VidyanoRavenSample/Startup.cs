using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raven.Client.Documents.Indexes;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Vidyano.Service;
using Vidyano.Service.RavenDB;
using VidyanoRavenSample.Service;

namespace VidyanoRavenSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddVidyanoRavenDB(Configuration, options =>
            {
                var settings = new DatabaseSettings();
                Configuration.Bind("Database", settings);
                if (settings.CertPath != null)
                    settings.CertPath = Path.Combine(Environment.ContentRootPath, settings.CertPath);

                var store = settings.CreateStore();
                options.Store = store;

                options.OnInitialized = () => IndexCreation.CreateIndexes(typeof(Startup).Assembly, store);

                // NOTE: For demo purposes
                // - we'll create the database if it doesn't exist and generate the sample data
                options.EnsureDatabaseExists = true;
                options.OnDatabaseCreated = () => store.Operations.Send(new CreateSampleDataOperation());
            });
            services.AddTransient<VidyanoRavenSampleContext>();
            services.AddTransient<RequestScopeProvider<VidyanoRavenSampleContext>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseVidyano(env, Configuration);
        }
    }
}
