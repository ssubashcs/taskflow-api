using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace TaskFlow.Api.Tests
{
    public class TasksApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TasksApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
                                            {
                                                AllowAutoRedirect = false                                               
                                            });
        }

        [Fact]
        public async Task GetTasksWithoutTokenReturnsUnauthorized()
        {
            var response = await _client.GetAsync("api/tasks", TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
