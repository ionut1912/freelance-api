namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;

public sealed record AssignFreelancerCommand(Guid ProjectId, Guid FreelancerId, Guid ClientId);
