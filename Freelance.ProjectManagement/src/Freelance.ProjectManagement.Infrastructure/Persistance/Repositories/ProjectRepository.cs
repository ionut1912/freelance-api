using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Freelance.ProjectManagement.Infrastructure.Persistance.Repositories;

public sealed class ProjectRepository(DbSet<Project> dbSet) : TrackingRepository<Project>(dbSet), IProjectRepository;
