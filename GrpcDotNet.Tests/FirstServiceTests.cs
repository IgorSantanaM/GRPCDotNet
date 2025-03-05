using Basics;
using FluentAssertions;
using GrpcDotNet.Services;
using GrpcDotNet.Tests.Helpers;
using Xunit.Sdk;

namespace GrpcDotNet.Tests
{
    public class FirstServiceTests
    {
        private readonly IFirstService sut; 
        public FirstServiceTests()
        {
            sut = new FirstService();
        }
        [Fact]
        public async void Unary_ShouldReturn_an_Object()
        {
            //Arrange
            var request = new Request() { Content = "Hello" };
            var callContext = TestServerCallContext.Create();
            var expectedResponse = new Response()
            {
                Message = "messagefrom server",
            };
            //Act
            var actual = await sut.Unary(request, callContext);
            //Assert
            actual.Should().BeEquivalentTo(expectedResponse);
        }
    }
}