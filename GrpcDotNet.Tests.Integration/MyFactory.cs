using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using static Basics.FirstServiceDefinition;

namespace GrpcDotNet.Tests.Integration
{
    public class MyFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
            builder.ConfigureTestServices(services =>
            {
            });
            builder.UseTestServer();
        }

        public FirstServiceDefinitionClient CreateGrpcClient()
        {
            var httpClient = CreateClient();
            var channel = GrpcChannel.ForAddress(httpClient.BaseAddress!, new GrpcChannelOptions
            {
                HttpClient = httpClient
            });

            return new FirstServiceDefinitionClient(channel);
        } 
    }
}
