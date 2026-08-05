namespace guidebot_api.Models;

/// <summary>
/// A named point the robot can be sent to or asked to start from (e.g. a
/// building, room, or landmark), anchored to a real-world coordinate so a
/// routing provider can compute paths between locations.
/// </summary>
public class Location
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}
