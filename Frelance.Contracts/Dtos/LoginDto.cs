using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Frelance.Contracts.Dtos;

[UsedImplicitly]
public record LoginDto([Required] string Username, [Required] [EmailAddress] string Email, [Required] string Password);