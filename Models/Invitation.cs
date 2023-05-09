using Newtonsoft.Json;

public class Invitation
{
    [JsonProperty(PropertyName = "id")]
    public string? Id { get; set; }
    public bool? CanAttend { get; set; }
    public string? FoodAllergies { get; set; }
    public string? Message { get; set; }
    public string? Name { get; internal set; }
}
