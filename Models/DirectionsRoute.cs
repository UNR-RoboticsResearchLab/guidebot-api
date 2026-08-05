namespace guidebot_api.Models;

/// <summary>
/// A walking route between two locations, as computed by an external routing
/// provider (see <see cref="Services.DirectionsService"/>).
/// </summary>
/// <remarks>
/// Named <c>DirectionsRoute</c> rather than <c>Route</c> to avoid colliding with
/// <see cref="Microsoft.AspNetCore.Routing.Route"/>, which ASP.NET Core Web
/// projects bring in via implicit usings.
/// </remarks>
public class DirectionsRoute
{
    public Location Origin { get; set; } = null!;

    public Location Destination { get; set; } = null!;

    public List<DirectionStep> Steps { get; set; } = new();

    public double TotalDistanceMeters { get; set; }

    public double TotalDurationSeconds { get; set; }
}
