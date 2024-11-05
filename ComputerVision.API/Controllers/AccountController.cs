using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ComputerVision.Data.Models;
using ComputerVision.Data.Models.DTO;
using ComputerVision.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace ComputerVision.API.Controllers;

[Route("api/account")] //Definierar base route för http request controller metoder
[ApiController]
public class AccountController : ControllerBase
{
    //DIs för user token och inlogning
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;

    // Constructor initierar ovan dependencies
    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    // hanterar Post anrop vid api/account/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        // Kontrollerar att inlognings data är korrekta
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // hittar user med hjälp av Email
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email!.ToLower());
        if (user == null)
        {
            return Unauthorized("Invalid username or password"); // 401 unauthorized
        }

        //Kontrollerar lösenord
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password!, false);
        if(!result.Succeeded)
        {
            return Unauthorized("Invalid username or password"); //401 Unauthorized
        }

        return Ok( // status 200 ok
                new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
    }

    // Hanterar registrering av ny användare api/account/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid) //Kontrollerar att registreringsuppgifter är korrekta
            {
                return BadRequest(ModelState);
            }
            // AppUser instans med data från formen
            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };
            
            //Skapar en ny användare i databasen
            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password!);

            //kontrollerar att användar skapandet lyckats
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
