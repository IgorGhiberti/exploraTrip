using System.ComponentModel.DataAnnotations;

namespace Application.Users;
public record CreateUserDTO([Required] string Name, [Required] string EmailVal, [Required] string Password);