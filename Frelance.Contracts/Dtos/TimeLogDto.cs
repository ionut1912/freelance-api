namespace Frelance.Contracts.Dtos;

public record TimeLogDto(int Id, DateTime StartTime, DateTime EndTime,DateOnly Date,int TotalHours);