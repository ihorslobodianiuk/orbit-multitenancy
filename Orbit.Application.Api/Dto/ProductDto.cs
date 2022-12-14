using System.Text.Json.Serialization;

namespace Orbit.Application.Api.Dto
{
    public class ProductDto
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
    }
}