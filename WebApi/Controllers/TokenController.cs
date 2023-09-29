using Domain.Services.Abstracts;
using JwtExample.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] AccountViewModel model)
    {
        return Ok();
    }
}