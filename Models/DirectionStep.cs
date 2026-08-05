namespace guidebot_api.Models;

/// <summary>
/// One turn-by-turn leg of a <see cref="DirectionsRoute"/>, as returned by the
/// routing provider (e.g. "Turn right onto Quad Walk").
/// </summary>
public class DirectionStep
{
    public string Instruction { get; set; } = string.Empty;

    /// <summary>Street/path name this step follows, if the provider gave one.</summary>
    public string? Name { get; set; }

    public double DistanceMeters { get; set; }

    public double DurationSeconds { get; set; }
}
