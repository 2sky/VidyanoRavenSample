using Raven.Client.Documents.Indexes;
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
                var store = options.Store = DatabaseSettings.CreateStore(Configuration, Environment);

                options.OnInitializedAsync = () => IndexCreation.CreateIndexesAsync(typeof(Startup).Assembly, store);

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
            app.UseVidyano(env, Configuration);
        }
    }
}
