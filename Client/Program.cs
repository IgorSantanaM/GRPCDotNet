using Basics;
using Grpc.Net.Client;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

var option = new GrpcChannelOptions
{
    
};
using var channel = GrpcChannel.ForAddress("https://localhost:7057", option);
var client = new FirstServiceDefinition.FirstServiceDefinitionClient(channel);
//Unary(client);
//ClientStreaming(client);
//ServerStreaming(client);
BiDirectionalStreaming(client);

void Unary(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    var request = new Request { Content = "Hello" };
    var response = client.Unary(request, deadline: DateTime.UtcNow.AddMilliseconds();
}

async void ClientStreaming(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    using var call = client.ClientStream();
    for(int i = 0; i < 1000; i++)
    {
        await call.RequestStream.WriteAsync(new Request { Content = i.ToString()});

    }
    await call.RequestStream.CloseAsync();

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


}

async void BiDirectionalStreaming(FirstServiceDefinition.FirstServiceDefinitionClient client)
{
    using (var call = client.BiDirectionalStream())
    {
        var request = new Request();
        for(var i  = 0; i < 10; ++i)
        {
            request.Content = i.ToString();
            Console.WriteLine(request.Content);
            await call.RequestStream.WriteAsync(request);
        }
        while(await call.ResponseStream.MoveNext())
        {
            var message = call.ResponseStream.Current;
            Console.WriteLine(message);
        } 

        await call.ResponseStream.CompleteAsync();
    }
}
