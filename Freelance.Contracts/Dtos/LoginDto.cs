using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Freelance.Contracts.Dtos;

[UsedImplicitly]
public record LoginDto([Required] string Username, [Required] string Password);