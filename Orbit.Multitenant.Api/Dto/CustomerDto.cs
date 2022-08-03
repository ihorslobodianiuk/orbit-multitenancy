using System.Text.Json.Serialization;

namespace Orbit.Multitenant.Api.Dto;

/// <summary>
/// A Customer.
/// </summary>
public class CustomerDto
{
    /// <summary>
    /// Id.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// First Name.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Last Name.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }
}