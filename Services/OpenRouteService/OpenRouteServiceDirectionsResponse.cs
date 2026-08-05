using System.Text.Json.Serialization;

namespace guidebot_api.Services.OpenRouteService;

/// <summary>
/// Minimal shape of an OpenRouteService <c>POST /v2/directions/{profile}</c>
/// response — only the fields <see cref="Services.DirectionsService"/> needs.
/// See https://openrouteservice.org/dev/#/api-docs/v2/directions/{profile}/post
/// </summary>
internal class OpenRouteServiceDirectionsResponse
{
    [JsonPropertyName("routes")]
    public List<OrsRoute> Routes { get; set; } = new();
}

internal class OrsRoute
{
    [JsonPropertyName("summary")]
    public OrsSummary Summary { get; set; } = new();

    [JsonPropertyName("segments")]
    public List<OrsSegment> Segments { get; set; } = new();
}

internal class OrsSummary
{
    [JsonPropertyName("distance")]
    public double DistanceMeters { get; set; }

    [JsonPropertyName("duration")]
    public double DurationSeconds { get; set; }
}

internal class OrsSegment
{
    [JsonPropertyName("steps")]
    public List<OrsStep> Steps { get; set; } = new();
}

internal class OrsStep
{
    [JsonPropertyName("distance")]
    public double DistanceMeters { get; set; }

    [JsonPropertyName("duration")]
    public double DurationSeconds { get; set; }

    [JsonPropertyName("instruction")]
    public string Instruction { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
