using Basics;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace GrpcDotNet.Tests.Integration
{
    public class FirstServicesTests : IClassFixture<MyFactory<Program>>
    {
        public MyFactory<Program> _factory;

        public FirstServicesTests(MyFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public void GetUnaryMessage()
        {
            //Arrange
            var client = _factory.CreateGrpcClient();
            var expectedResponse = new Response { Message = "messagefrom server" };

            //Act
            var actualResponse = client.Unary(new Request { Content = "message" });

            //Assert
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}