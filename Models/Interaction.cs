namespace guidebot_api.Models;

public class Interaction
{
    public Guid Id { get; set; }

    public string? Speech { get; set; }

    public bool RobotTurn { get; set; }

}
