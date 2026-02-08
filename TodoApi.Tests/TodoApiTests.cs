using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;

//IClassFeature -> create one instance for all tests
//webapplicationfactory<program> -> spin up api in-memory
public class TodoApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TodoApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllTodos_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync("todoitems");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateTodo_ReturnsCreatedStatus()
    {
        var newTodo = new {Name = "Test Todo", IsComplete = false};
        var response = await _client.PostAsJsonAsync("/todoitems", newTodo);

        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

}