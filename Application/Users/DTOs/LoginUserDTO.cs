using System.ComponentModel.DataAnnotations;

namespace Application.Users.DTOs;
public record LoginUserDTO([Required] Guid Id, [Required] string Email, [Required] string Password);