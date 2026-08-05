using System.Text.Json;
using guidebot_api.Models;
using guidebot_api.Services.OpenRouteService;
using Microsoft.Extensions.Options;

namespace guidebot_api.Services;

/// <summary>
/// Implementation of <see cref="IDirectionsService"/> that keeps a small local
/// directory of named campus locations (id, name, coordinates) but delegates
/// actual pathfinding to the OpenRouteService directions API, rather than
/// hand-maintaining a graph of every walkway on campus.
///
/// <see cref="BuildSampleLocations"/> currently seeds placeholder locations.
/// Replace it with the real set of campus destinations, or move it to
/// configuration/a database once requirements are firmed up.
/// </summary>
public class DirectionsService(HttpClient httpClient, IOptions<OpenRouteServiceOptions> options, ILogger<DirectionsService> logger)
    : IDirectionsService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly OpenRouteServiceOptions _options = options.Value;
    private readonly ILogger<DirectionsService> _logger = logger;
    private readonly Dictionary<string, Location> _locations = BuildSampleLocations();

    public Task<IReadOnlyCollection<Location>> GetLocationsAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<Location> locations = _locations.Values.ToList();
        return Task.FromResult(locations);
    }

    public Task<Location?> GetLocationAsync(string locationId, CancellationToken cancellationToken = default)
    {
        _locations.TryGetValue(locationId, out var location);
        return Task.FromResult(location);
    }

    public async Task<DirectionsRoute?> GetRouteAsync(string originId, string destinationId, CancellationToken cancellationToken = default)
    {
        if (!_locations.TryGetValue(originId, out var origin) || !_locations.TryGetValue(destinationId, out var destination))
        {
            return null;
        }

        if (originId == destinationId)
        {
            return new DirectionsRoute { Origin = origin, Destination = destination };
        }

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            _logger.LogError("OpenRouteService:ApiKey is not configured; cannot compute a route.");
            return null;
        }

        var requestBody = new
        {
            coordinates = new[]
            {
                new[] { origin.Longitude, origin.Latitude },
                new[] { destination.Longitude, destination.Latitude }
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/v2/directions/{_options.Profile}")
        {
            Content = JsonContent.Create(requestBody)
        };
        request.Headers.Add("Authorization", _options.ApiKey);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to reach OpenRouteService while routing {Origin} -> {Destination}.", originId, destinationId);
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning(
                "OpenRouteService returned {StatusCode} while routing {Origin} -> {Destination}.",
                response.StatusCode, originId, destinationId);
            return null;
        }

        OpenRouteServiceDirectionsResponse? result;
        try
        {
            result = await response.Content.ReadFromJsonAsync<OpenRouteServiceDirectionsResponse>(cancellationToken: cancellationToken);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse OpenRouteService response while routing {Origin} -> {Destination}.", originId, destinationId);
            return null;
        }

        var route = result?.Routes.FirstOrDefault();
        if (route is null)
        {
            return null; // no path found between the two coordinates
        }

        return new DirectionsRoute
        {
            Origin = origin,
            Destination = destination,
            TotalDistanceMeters = route.Summary.DistanceMeters,
            TotalDurationSeconds = route.Summary.DurationSeconds,
            Steps = route.Segments
                .SelectMany(segment => segment.Steps)
                .Select(step => new DirectionStep
                {
                    Instruction = step.Instruction,
                    Name = step.Name,
                    DistanceMeters = step.DistanceMeters,
                    DurationSeconds = step.DurationSeconds
                })
                .ToList()
        };
    }

    private static Dictionary<string, Location> BuildSampleLocations()
    {
        // Placeholder coordinates - replace with the real campus destinations.
        return new[]
        {
            new Location { Id = "student-union", Name = "Student Union", Latitude = 39.5439, Longitude = -119.8138 },
            new Location { Id = "library", Name = "Main Library", Latitude = 39.5442, Longitude = -119.8159 },
            new Location { Id = "engineering-building", Name = "Engineering Building", Latitude = 39.5450, Longitude = -119.8149 },
        }.ToDictionary(l => l.Id);
    }
}
