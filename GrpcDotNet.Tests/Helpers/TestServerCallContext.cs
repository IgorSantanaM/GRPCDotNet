using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcDotNet.Tests.Helpers
{
    public class TestServerCallContext : ServerCallContext
    {
        private readonly Metadata _requestHeaders;
        private readonly CancellationToken _token;
        

        private TestServerCallContext(Metadata requestHeaders, CancellationToken token)
        {
            _requestHeaders = requestHeaders;
            _token = token;
        }
            
        protected override string MethodCore => "MethodName";

        protected override string HostCore => "HostName";

        protected override string PeerCore => "PeerName";

        protected override DateTime DeadlineCore { get; }

        protected override Metadata RequestHeadersCore => throw new NotImplementedException();

        protected override CancellationToken CancellationTokenCore => throw new NotImplementedException();

        protected override Metadata ResponseTrailersCore => throw new NotImplementedException();

        protected override Status StatusCore { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override WriteOptions? WriteOptionsCore { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override AuthContext AuthContextCore => throw new NotImplementedException();

        protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options)
        {
            throw new NotImplementedException();
        }

        protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
        {
            throw new NotImplementedException();
        }
    }
}
