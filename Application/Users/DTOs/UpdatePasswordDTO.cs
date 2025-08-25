using System.ComponentModel.DataAnnotations;

namespace Application.Users.DTOs;

public record UpdatePasswordDTO([Required] string Email, [Required] string Password, [Required] string OldPassword);