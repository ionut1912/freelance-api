using System.ComponentModel.DataAnnotations;

namespace Frelance.Contracts.Dtos;

public record LoginDto([Required] string Username,[Required][EmailAddress] string Enail, [Required] string Password);