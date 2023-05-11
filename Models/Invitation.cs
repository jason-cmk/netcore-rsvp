
using Newtonsoft.Json;

public class Invitation
{
    [JsonProperty(PropertyName = "id")]
    public string? Id { get; set; }

    [JsonProperty(PropertyName = "canAttend")]
    public bool? CanAttend { get; set; }

    [JsonProperty(PropertyName = "foodAllergies")]
    public string? FoodAllergies { get; set; }

    [JsonProperty(PropertyName = "message")]
    public string? Message { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string? Name { get; set; }
}
