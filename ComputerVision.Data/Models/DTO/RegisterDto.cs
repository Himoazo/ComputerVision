using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerVision.Data.Models.DTO;

// strukturerar nya användarens registrerings data
public class RegisterDto
{
    [Required]
    public string? Username  { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}

//Skickas med som svar status 200ok i båda login och regiser metoder i account controller. Hanterar JWT
public class NewUserDto
{
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Token { get; set; }
}

// Strukturerar användarens inloggnings uppgifter
public class LoginDto
{
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}