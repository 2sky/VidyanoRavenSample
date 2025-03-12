using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations;
using Raven.Client.Http;
using Sparrow.Json;

namespace VidyanoRavenSample;

/// <summary>
/// NOTE: For demo purposes, invokes the Create Sample Data action
/// </summary>
internal sealed class CreateSampleDataOperation : IOperation
{
    public RavenCommand GetCommand(IDocumentStore store, DocumentConventions conventions, JsonOperationContext context, HttpCache cache)
    {
        return new CreateSampleDataCommand();
    }

    private sealed class CreateSampleDataCommand : RavenCommand
    {
        public override HttpRequestMessage CreateRequest(JsonOperationContext ctx, ServerNode node, out string url)
        {
            url = node.Url + "/databases/" + node.Database + "/studio/sample-data";
            return new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };
        }
    }
}