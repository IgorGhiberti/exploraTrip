using System.ComponentModel.DataAnnotations;

namespace Application.Users.DTOs;
public record LoginUserDTO([Required] string Email, [Required] string Password);