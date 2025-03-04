using Grpc.Core;
using Grpc.Core.Interceptors;

namespace MVCClient.Interceptors
{
    public class ServerLoggingInteceptor : Interceptor
    {
        public readonly ILogger<ServerLoggingInteceptor> _logger;

        public ServerLoggingInteceptor(ILogger<ServerLoggingInteceptor> logger)
        {
            _logger = logger;
        }

        public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                _logger.LogInformation("Server inteceptig");
                return await continuation(request, context);
            }catch (Exception ex)
            {
                _logger.LogError(ex, $"Error thrown by: {context.Method}");
                throw;
            }
        }
    }
}
