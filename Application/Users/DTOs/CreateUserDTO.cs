using System.ComponentModel.DataAnnotations;

namespace Application.Users;
public record CreateUserDTO(Guid Id, [Required] string Name, [Required] string EmailVal, [Required] string Password);