namespace Application.DTOs.LocalDTOs;

public record ViewLocalDTO(Guid LocalId, string LocalName, DateTime? DateStart, DateTime? DateEnd, string?[] Notes, decimal? LocalBudget);