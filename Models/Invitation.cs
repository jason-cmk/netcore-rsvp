using System.Text.Json.Serialization;

public class Invitation
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("canAttend")]
    public bool? CanAttend { get; set; }

    [JsonPropertyName("foodAllergies")]
    public string? FoodAllergies { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
