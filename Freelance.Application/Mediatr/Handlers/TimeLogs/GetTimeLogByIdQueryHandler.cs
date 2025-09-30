using Freelance.Application.Mediatr.Queries.TimeLogs;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.TimeLogs;

public class GetTimeLogByIdQueryHandler : IRequestHandler<GetTimeLogByIdQuery, TimeLogDto>
{
    private readonly ITimeLogRepository _timeLogRepository;

    public GetTimeLogByIdQueryHandler(ITimeLogRepository timeLogRepository)
    {
        ArgumentNullException.ThrowIfNull(timeLogRepository, nameof(timeLogRepository));
        _timeLogRepository = timeLogRepository;
    }

    public async Task<TimeLogDto> Handle(GetTimeLogByIdQuery request, CancellationToken cancellationToken)
    {
        return await _timeLogRepository.GetTimeLogByIdAsync(request, cancellationToken);
    }
}