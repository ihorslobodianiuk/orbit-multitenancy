using System.Text.Json.Serialization;

namespace Orbit.Multitenant.Api.Dto;

public class ProductDto
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
}