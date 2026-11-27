using Npgsql;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.Results;
using Freelance.Identity.Api.Mappers;
using Freelance.Identity.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Shared.Domain.Common;
using Shared.Domain.Exceptions;

namespace Freelance.Identity.UnitTests.Api.Mappers;

public sealed class IdentityExceptionMapperTests
{
    private readonly IdentityExceptionMapper _mapper = new();

    public static TheoryData<Exception, int, string> DomainExceptions => new()
    {
        { new AccountNotFoundException("x"), StatusCodes.Status404NotFound, "Account Not Found" },
        { new AccountAlreadyBlockedException("x"), StatusCodes.Status400BadRequest, "Account Already Blocked" },
        { new AccountNotBlockedException("x"), StatusCodes.Status400BadRequest, "Account Not Blocked" },
        { new PasswordNotMatchException("x"), StatusCodes.Status400BadRequest, "Password Not Match" },
        { new AccountBlockedException("x"), StatusCodes.Status400BadRequest, "Account Blocked" },
        { new UserAlreadyExistsException("x"), StatusCodes.Status400BadRequest, "User Already Exists" }
    };

    [Theory]
    [MemberData(nameof(DomainExceptions))]
    public void TryMap_DomainException_MapsStatusTitleAndDetail(Exception exception, int status, string title)
    {
        _mapper.TryMap(exception, out var problem).Should().BeTrue();

        problem.Should().BeEquivalentTo(new { Status = status, Title = title, Detail = "x" });
    }

    [Fact]
    public void TryMap_FluentValidationException_ReturnsValidationErrors()
    {
        var exception = new ValidationException([new ValidationFailure("Email", "bad email")]);

        _mapper.TryMap(exception, out var problem).Should().BeTrue();

        problem.Status.Should().Be(StatusCodes.Status400BadRequest);
        problem.Extensions["errors"].Should().BeEquivalentTo(new[] { new ValidationError("Email", "bad email") });
    }

    [Fact]
    public void TryMap_CustomValidationException_ReturnsValidationErrors()
    {
        var errors = new List<ValidationError> { new("Role", "bad role") };

        _mapper.TryMap(new CustomValidationException(errors), out var problem).Should().BeTrue();

        problem.Title.Should().Be("Validation Error");
        problem.Extensions["errors"].Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void TryMap_UnknownException_ReturnsGenericServerError()
    {
        _mapper.TryMap(new InvalidOperationException("secret details"), out var problem).Should().BeTrue();

        problem.Status.Should().Be(StatusCodes.Status500InternalServerError);
        problem.Detail.Should().NotContain("secret details");
    }

    [Fact]
    public void TryMap_NullException_Throws() =>
        _mapper.Invoking(m => m.TryMap(null!, out _)).Should().Throw<ArgumentNullException>();

    [Theory]
    [InlineData(PostgresErrorCodes.UniqueViolation, 400)]
    [InlineData(PostgresErrorCodes.ForeignKeyViolation, 500)]
    public void TryMap_DatabaseUpdateFailure_OnlyTreatsUniqueViolationAsDuplicateUser(string sqlState, int status)
    {
        var exception = new DbUpdateException("save failed", new PostgresException("detail", "ERROR", "ERROR", sqlState));

        new IdentityExceptionMapper().TryMap(exception, out var problem);

        problem.Status.Should().Be(status);
        problem.Detail.Should().NotContain("save failed");
    }

    [Theory]
    [InlineData(400)]
    [InlineData(413)]
    public void TryMap_UnreadableRequest_KeepsItsStatusAndHidesTheReason(int status)
    {
        new IdentityExceptionMapper().TryMap(new BadHttpRequestException("internal parser detail", status), out var problem);

        problem.Status.Should().Be(status);
        problem.Detail.Should().NotContain("internal");
    }
}
