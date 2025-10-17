using Domain.Entities;

namespace Application.DTOs.LocalDTOs;

public record UpdateLocalDTO(Guid LocalId, string LocalName, DateTime? DateStart, DateTime? DateEnd, Guid TripId, string[] Notes, decimal? LocalBudget);