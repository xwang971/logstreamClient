using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LogstreamingDemo
{
    public class LogstreamingDemo
    {
        static async Task Main(string[] args)
        {
            var subscription = "";
            var resourceGroupName = "";
            var containerAppName = "";

            //Get revisionName by "GET https://management.azure.com/subscriptions/{subId}/resourceGroups/{rg}/providers/Microsoft.App/containerApps/{appName}/revisions?api-version=2022-01-01-preview"
            var revisionName = "";

            //Get replicaName and containerName by "GET https://management.azure.com/subscriptions/{subId}/resourceGroups/{rg}/providers/Microsoft.App/containerApps/{appName}/revisions/{revisionName}/replicas?api-version=2022-01-01-preview"
            var replicaName = "";
            var containerName = "";

            //Get token by "POST https://management.azure.com/subscriptions/{subId}/resourcegroups/{rg}/providers/Microsoft.App/containerApps/{appName}/authtoken?api-version=2022-01-01-preview"
            var token = "";

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                // output can be json/text
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://northcentralusstage.azurecontainerapps.dev/subscriptions/{subscription}/resourceGroups/{resourceGroupName}/containerApps/{containerAppName}/revisions/{revisionName}/replicas/{replicaName}/containers/{containerName}/logstream?token={token}&tailLines=20&output=text&follow=true");
                using (var response = await client.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    using (var body = await response.Content.ReadAsStreamAsync()) 
                    {
                        using (var reader = new StreamReader(body, leaveOpen: true)) 
                        {
                            while (!reader.EndOfStream)
                                Console.WriteLine(reader.ReadLine());
                        }
                    }
                }
            }
        }
    }
}
