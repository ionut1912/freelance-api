using JetBrains.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Frelance.Contracts.Dtos;

[UsedImplicitly]
public record LoginDto([Required] string Username, [Required] string Password);