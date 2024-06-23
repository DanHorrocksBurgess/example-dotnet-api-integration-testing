using System.Net;
using System.Net.Http.Json;
using Api.Data.Models;
using Integration.Tests.Shared;

namespace Integration.Tests;

[Collection(nameof(SharedApiCollectionTests))]
public class AttractionTests(SharedApiTestFixture fixture)
{
    private readonly HttpClient _client = fixture.CreateClient();

    [Fact]
    public async void GetAll_NoFilter_ReturnsResponse200()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/attraction");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async void Create_ValidData_ReturnsResponse201()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/attraction")
        {
            Content = JsonContent.Create(new Attraction() { Name = "Big Thunder Mountain", ParkName = "Disney's Magic Kingdom" })
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async void GetSingleAttraction_ByValidAttractionId_ReturnsResponse200AndAttraction()
    {
        // Arrange
        // Create Attraction
        var attraction = new Attraction() { Name = "Big Thunder Mountain", ParkName = "Disney's Magic Kingdom" };
        var attractionCreateRequest = new HttpRequestMessage(HttpMethod.Post, "/attraction")
        {
            Content = JsonContent.Create(attraction)
        };
        var attractionCreateResponse = await _client.SendAsync(attractionCreateRequest);
        Assert.Equal(HttpStatusCode.Created, attractionCreateResponse.StatusCode);

        var attractionCreated = await attractionCreateResponse.Content.ReadFromJsonAsync<Attraction>();
        Assert.NotNull(attractionCreated?.AttractionId);
        Assert.True(attractionCreated.AttractionId > 0);

        // Act
        var attractionGetRequest = new HttpRequestMessage(HttpMethod.Get, $"/attraction/{attractionCreated.AttractionId}");
        var attractionGetResponse = await _client.SendAsync(attractionGetRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, attractionGetResponse.StatusCode);
        var attractionGetResponseData = await attractionGetResponse.Content.ReadFromJsonAsync<Attraction>();

        Assert.NotNull(attractionGetResponseData);
        Assert.Equal(attractionGetResponseData.Name, attraction.Name);
        Assert.Equal(attractionGetResponseData.ParkName, attraction.ParkName);
    }
}