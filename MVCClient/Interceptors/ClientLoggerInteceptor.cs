using Grpc.Core;
using Grpc.Core.Interceptors;

namespace MVCClient.Interceptors
{
    public class ClientLoggerInteceptor : Interceptor
    {
        public readonly ILogger<ClientLoggerInteceptor> _logger;

        public ClientLoggerInteceptor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ClientLoggerInteceptor>();
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            try
            {
                _logger.LogInformation($"starting the client call of type: {context.Method.FullName}, {context.Method.Type}");
                return continuation(request, context );
            }catch (Exception ex)
            {
                throw;
            }
        }
    }
}
