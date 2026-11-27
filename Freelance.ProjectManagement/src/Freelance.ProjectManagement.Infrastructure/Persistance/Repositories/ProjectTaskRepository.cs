using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Freelance.ProjectManagement.Infrastructure.Persistance.Repositories;

public sealed class ProjectTaskRepository(DbSet<ProjectTask> dbSet) : TrackingRepository<ProjectTask>(dbSet), IProjectTaskRepository;
