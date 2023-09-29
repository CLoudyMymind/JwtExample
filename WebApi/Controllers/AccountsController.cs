using Domain.Services.Abstracts;
using JwtExample.ViewModels;
using Microsoft.AspNetCore.Mvc;
using WebApi.Validations;
using WebApi.ViewModels;

namespace WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly NewAccountViewModelValidate _validate;
    private readonly IEmailSenderService _emailSender;

    public AccountsController(IAccountService accountService, NewAccountViewModelValidate validate, IEmailSenderService emailSender)
    {
        _accountService = accountService;
        _validate = validate;
        _emailSender = emailSender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> AccountCreateAsync([FromBody] NewAccountViewModel model)
    {
        var validate = await _validate.ValidateAsync(model);
        if (validate.IsValid)
        {
            var result = await _accountService.Register(NewAccountViewModel.MapToDomain(model));
            if (result.IsSuccess) return Ok(result.Success);
            return BadRequest(new ErrorViewModel(result.Failure));
        }

        return BadRequest(validate.Errors.First().ErrorMessage);
    }
    [HttpPost("verify")]
    public async Task<IActionResult> StartVerifyAccountAsync([FromBody] EmailViewModel model)
    {
        var result = await _accountService.StartVerifyAccount(model.Email);
        if (result.IsSuccess)
        {
            await _emailSender.SendEmailAsync(model.Email, "Подтждения регистрации", 
                "Подтвердите регистрацию нажав кнопку " + 
                $"ващ код для подтверждения регистрации {result.Success} перейдите обратно на сайт и введите код");
            return Ok("инструкция для подтверждения была отправлено на почту");
        }
        return BadRequest(new ErrorViewModel(result.Failure));
    }

    [HttpPut("confirm/{model:required}")]
    public async Task<IActionResult> ConfirmVerifyAsync(string codes)
    {
        return Ok();
    }
}