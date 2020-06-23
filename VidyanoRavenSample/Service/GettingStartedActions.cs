using System.IO;
using Microsoft.Extensions.Hosting;
using Raven.Client.Documents;
using Vidyano.Service.Repository;

namespace VidyanoRavenSample.Service
{
    /// <summary>
    /// Contains the customizations for the "GettingStarted" Persistent Object (which is in this case "virtual" as it doesn't belong to a domain entity)
    /// </summary>
    public class GettingStartedActions : PersistentObjectActionsReference<VidyanoRavenSampleContext, object>
    {
        private readonly DocumentStore store;
        private readonly IHostEnvironment hostEnvironment;

        // constructor uses dependency injection (DI), context is required
        public GettingStartedActions(VidyanoRavenSampleContext context, DocumentStore store, IHostEnvironment hostEnvironment)
            : base(context)
        {
            this.store = store;
            this.hostEnvironment = hostEnvironment;
        }


        public override void OnLoad(PersistentObject obj, PersistentObject parent)
        {
            // NOTE: By default this would load the entity from the database using the obj.ObjectId and obj.ContextProperty

            var studioUrl = $"{store.Urls[0]}/studio/index.html#databases/documents?&database={store.Database}";
            obj.AddNotification($"[url:Open in Raven.Studio|{studioUrl}]", NotificationType.OK);

            obj.Actions = null;

            obj["Contents"].SetValue(File.ReadAllText(Path.Combine(hostEnvironment.ContentRootPath, "README.md")));
        }
    }
}