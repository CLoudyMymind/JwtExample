using Domain.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using WebApi.ViewModels;

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
        var result = await _tokenService.GetTokenAsync(model.Login, model.Password);
        return result.IsSuccess ? Ok(result.Success) : BadRequest(new ErrorViewModel(result.Failure));
    }
}