namespace Frelance.Infrastructure.Entities;

public class Addresses
{
    public int Id { get; init; }
    public required string Country { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string StreetNumber { get; set; }
    public required string ZipCode { get; set; }
}