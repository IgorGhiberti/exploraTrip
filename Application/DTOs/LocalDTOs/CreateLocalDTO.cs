namespace Application.DTOs.LocalDTOs;

public record CreateLocalDTO(string LocalName, DateTime? DateStart, DateTime? DateEnd, string[]? Notes, Guid TripId, decimal? LocalBudget);