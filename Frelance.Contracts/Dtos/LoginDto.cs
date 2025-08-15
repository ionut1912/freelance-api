using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Frelance.Contracts.Dtos;

[UsedImplicitly]
public record LoginDto([Required] string Username, [Required] string Password);