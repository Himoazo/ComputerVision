using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerVision.Data.Models.DTO;

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

public class NewUserDto
{
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Token { get; set; }
}

public class LoginDto
{
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}