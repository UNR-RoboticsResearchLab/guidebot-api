namespace guidebot_api.Services.OpenRouteService;

/// <summary>
/// Configuration for calling the OpenRouteService directions API. Bound from the
/// <c>OpenRouteService</c> section of configuration (appsettings, user-secrets,
/// or environment variables) — see <see cref="Services.DirectionsService"/>.
/// </summary>
public class OpenRouteServiceOptions
{
    public const string SectionName = "OpenRouteService";

    /// <summary>
    /// API key from https://openrouteservice.org/dev/#/signup. Keep this out of
    /// source control (use user-secrets locally, an env var / secret store in
    /// deployment) — appsettings.json only holds an empty placeholder.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = "https://api.openrouteservice.org";

    /// <summary>Routing profile — "foot-walking" matches how the robot moves.</summary>
    public string Profile { get; set; } = "foot-walking";
}
