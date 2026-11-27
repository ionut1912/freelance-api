using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Infra.Services;

namespace Freelance.ProjectManagement.Infrastructure.Persistance.Repositories;

public sealed class TimeLogRepository(DbSet<TimeLog> dbSet) : GenericRepository<TimeLog>(dbSet), ITimeLogRepository;
