using FluentValidation;
using FluentValidation.Results;
using Freelance.ProjectManagement.Api.Mappers;
using Freelance.ProjectManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Shared.Domain.Common;
using Shared.Domain.Exceptions;

namespace Freelance.ProjectManagement.UnitTests.Api.Mappers;

public sealed class ProjectManagementMapperTests
{
    private readonly ProjectManagementMapper _mapper = new();

    public static TheoryData<Exception, string> NotFound => new()
    {
        { new ProjectNotFoundException("x"), "Project Not Found" },
        { new ProjectTaskNotFoundException("x"), "Project Task Not Found" },
        { new TimeLogNotFoundException("x"), "Time Log Not Found" }
    };

    [Theory]
    [MemberData(nameof(NotFound))]
    public void TryMap_NotFoundExceptions_Return404(Exception exception, string title)
    {
        _mapper.TryMap(exception, out var problem).Should().BeTrue();

        problem.Should().BeEquivalentTo(new { Status = StatusCodes.Status404NotFound, Title = title, Detail = "x" });
    }

    [Fact]
    public void TryMap_ValidationExceptions_Return400WithErrors()
    {
        _mapper.TryMap(new ValidationException([new ValidationFailure("Title", "required")]), out var fluent);
        _mapper.TryMap(new CustomValidationException([new ValidationError("Amount", "bad")]), out var custom);

        fluent.Status.Should().Be(StatusCodes.Status400BadRequest);
        fluent.Extensions["errors"].Should().BeEquivalentTo(new[] { new ValidationError("Title", "required") });
        custom.Extensions["errors"].Should().BeEquivalentTo(new[] { new ValidationError("Amount", "bad") });
    }

    [Fact]
    public void TryMap_UnknownException_Returns500()
    {
        _mapper.TryMap(new InvalidOperationException(), out var problem).Should().BeTrue();

        problem.Status.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public void TryMap_NullException_Throws() =>
        _mapper.Invoking(m => m.TryMap(null!, out _)).Should().Throw<ArgumentNullException>();

    [Theory]
    [InlineData(400)]
    [InlineData(413)]
    public void TryMap_UnreadableRequest_KeepsItsStatusAndHidesTheReason(int status)
    {
        new ProjectManagementMapper().TryMap(new BadHttpRequestException("internal parser detail", status), out var problem);

        problem.Status.Should().Be(status);
        problem.Detail.Should().NotContain("internal");
    }
}
