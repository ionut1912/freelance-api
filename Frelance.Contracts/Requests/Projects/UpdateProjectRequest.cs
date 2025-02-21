namespace Frelance.Contracts.Requests.Projects;

public class UpdateProjectRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
    public List<string>? Technologies { get; set; }
    public decimal? Budget { get; set; }
}