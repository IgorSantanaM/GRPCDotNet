﻿using Basics;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;

var retryPolicy = new MethodConfig
{
    Names = { MethodName.Default },
    RetryPolicy = new RetryPolicy
    {
        MaxAttempts = 5,
        BackoffMultiplier = 1,
        InitialBackoff = TimeSpan.FromSeconds(0.5),
        MaxBackoff = TimeSpan.FromSeconds(0.5),
        RetryableStatusCodes = { StatusCode.Internal}, // requests that are Retryable
    }
};

var factory = new StaticResolverFactory(addr => new[]
{
    new BalancerAddress("localhost", 5057),
    new BalancerAddress("localhost", 5058)
});

var services = new ServiceCollection();
services.AddSingleton<ResolverFactory>(factory);

var options = new GrpcChannelOptions
{
    ServiceConfig = new ServiceConfig
    {
        MethodConfigs = {retryPolicy}
    }
};

//using var channel = GrpcChannel.ForAddress("https://localhost:7057", options);
var channel = GrpcChannel.ForAddress("static://localhost", new GrpcChannelOptions()
{
    Credentials = ChannelCredentials.Insecure,
    ServiceConfig = new ServiceConfig
    {
        LoadBalancingConfigs = { new RoundRobinConfig() }
    },
    ServiceProvider = services.BuildServiceProvider()
});
//using var channel = GrpcChannel.ForAddress("https://localhost:7057", options);
var client = new FirstServiceDefinition.FirstServiceDefinitionClient(channel);
Unary(client);
//ClientStreaming(client);
//ServerStreaming(client);
//BiDirectionalStreaming(client);


void Unary(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    var metadata = new Metadata { { "grpc-accept-enconding", "gzip" } };
    var request = new Request { Content = "Hello" };
    var response = client.Unary(request, deadline: DateTime.UtcNow.AddMilliseconds(3), headers: metadata);
}

async void ClientStreaming(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    using var call = client.ClientStream();
    for (int i = 0; i < 1000; i++)
    {
        await call.RequestStream.WriteAsync(new Request { Content = i.ToString() });

    }
    await call.RequestStream.CompleteAsync();

    Response response = await call;
}
void ServerStreaming(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    try
    {
        var cancellationToken = new CancellationTokenSource();
        using var streamingCall = client.ServerStream(new Request { Content = "Hello" });

        foreach (var response in streamingCall.ResponseStream.ReadAllAsync(cancellationToken))
        {
            Console.WriteLine(response.Message);
            if (response.Message.Contains(2))
            {
                cancellationToken.Cancel();
            }
        }
    }
    catch (Exception ex) when (ex.InnerException != null)
    {
        Console.WriteLine(ex);
    }
}
}

async void BiDirectionalStreaming(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    using (var call = client.BiDirectionalStream())
    {
        var request = new Request();
        for (var i = 0; i < 10; ++i)
        {
            request.Content = i.ToString();
            Console.WriteLine(request.Content);
            await call.RequestStream.WriteAsync(request);
        }
        while (await call.ResponseStream.MoveNext())
        {
            var message = call.ResponseStream.Current;
            Console.WriteLine(message);
        }

        await call.ResponseStream.Compre;
    }
}
