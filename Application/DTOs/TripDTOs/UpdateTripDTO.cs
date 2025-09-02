namespace Application.DTOs.TripDTOs;

public record UpdateTripDTO(string? TripName, DateTime? startDate, DateTime? endDate, decimal? TripBudget, string[]? Notes);