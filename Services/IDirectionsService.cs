using guidebot_api.Models;

namespace guidebot_api.Services;

/// <summary>
/// Provides wayfinding data for the guide robot: the campus locations it knows
/// about, and walking routes between them.
/// </summary>
/// <remarks>
/// Locations are maintained locally (id, name, coordinates), but the actual
/// path between two locations is computed by an external routing provider
/// rather than a hand-maintained graph — see <see cref="DirectionsService"/>.
/// </remarks>
public interface IDirectionsService
{
    /// <summary>All locations the robot can be sent to or asked to start from.</summary>
    Task<IReadOnlyCollection<Location>> GetLocationsAsync(CancellationToken cancellationToken = default);

    /// <summary>A single location by id, or <c>null</c> if it isn't known.</summary>
    Task<Location?> GetLocationAsync(string locationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// A walking route from <paramref name="originId"/> to <paramref name="destinationId"/>,
    /// or <c>null</c> if either location is unknown or the routing provider
    /// couldn't find a path between them.
    /// </summary>
    Task<DirectionsRoute?> GetRouteAsync(string originId, string destinationId, CancellationToken cancellationToken = default);
}
