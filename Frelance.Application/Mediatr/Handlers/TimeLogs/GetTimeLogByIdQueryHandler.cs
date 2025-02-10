using Frelance.Application.Mediatr.Queries.TimeLogs;
using Frelance.Application.Repositories;
using Frelance.Contracts.Responses.TimeLogs;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.TimeLogs;

public class GetTimeLogByIdQueryHandler : IRequestHandler<GetTimeLogByIdQuery, GetTimeLogByIdResponse>
{
    private readonly ITimeLogRepository _timeLogRepository;

    public GetTimeLogByIdQueryHandler(ITimeLogRepository timeLogRepository)
    {
        ArgumentNullException.ThrowIfNull(timeLogRepository, nameof(timeLogRepository));
        _timeLogRepository = timeLogRepository;
    }
    public async Task<GetTimeLogByIdResponse> Handle(GetTimeLogByIdQuery request, CancellationToken cancellationToken)
    {
        return await _timeLogRepository.GetTimeLogByIdAsync(request, cancellationToken);
    }
}