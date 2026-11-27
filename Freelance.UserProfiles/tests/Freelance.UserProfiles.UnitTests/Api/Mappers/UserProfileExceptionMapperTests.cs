using FluentValidation;
using FluentValidation.Results;
using Freelance.UserProfiles.Api.Mappers;
using Freelance.UserProfiles.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Shared.Domain.Common;
using Shared.Domain.Exceptions;

namespace Freelance.UserProfiles.UnitTests.Api.Mappers;

public sealed class UserProfileExceptionMapperTests
{
    private readonly UserProfileExceptionMapper _mapper = new();

    public static TheoryData<Exception, int, string> DomainExceptions => new()
    {
        { new ProfileNotFoundException("x"), StatusCodes.Status404NotFound, "Profile Not Found" },
        { new BioAlreadyExistsException("x"), StatusCodes.Status400BadRequest, "Bio Already Exists" },
        { new ImageAlreadyExistsException("x"), StatusCodes.Status400BadRequest, "Image Already Exists" },
        { new ProfileAllreadyExistsException("x"), StatusCodes.Status400BadRequest, "Profile Already Exists" }
    };

    [Theory]
    [MemberData(nameof(DomainExceptions))]
    public void TryMap_DomainException_MapsStatusAndTitle(Exception exception, int status, string title)
    {
        _mapper.TryMap(exception, out var problem).Should().BeTrue();

        problem.Should().BeEquivalentTo(new { Status = status, Title = title, Detail = "x" });
    }

    [Fact]
    public void TryMap_ValidationExceptions_Return400WithErrors()
    {
        _mapper.TryMap(new ValidationException([new ValidationFailure("Bio", "required")]), out var fluent);
        _mapper.TryMap(new CustomValidationException([new ValidationError("Image", "bad")]), out var custom);

        fluent.Extensions["errors"].Should().BeEquivalentTo(new[] { new ValidationError("Bio", "required") });
        custom.Status.Should().Be(StatusCodes.Status400BadRequest);
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
        new UserProfileExceptionMapper().TryMap(new BadHttpRequestException("internal parser detail", status), out var problem);

        problem.Status.Should().Be(status);
        problem.Detail.Should().NotContain("internal");
    }
}
