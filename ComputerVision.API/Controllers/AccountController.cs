using ComputerVision.Data.Repositories;
using ImageManipulation.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ComputerVision.Data.Models;
using ComputerVision.Data.Models.DTO;
using ComputerVision.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace ComputerVision.API.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;
    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email!.ToLower());
        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password!, false);
        if(!result.Succeeded)
        {
            return Unauthorized("Invalid username or password");
        }

        return Ok(
                new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password!);
            if (createdUser.Succeeded)
            {
                return Ok(
                    new NewUserDto
                    {
                        Username = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    }
                    );
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, createdUser.Errors);
            }

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
