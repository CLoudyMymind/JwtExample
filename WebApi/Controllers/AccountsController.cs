using Domain.Services.Abstracts;
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
        if (!validate.IsValid) return BadRequest(validate.Errors.First().ErrorMessage);
        var result = await _accountService.Register(NewAccountViewModel.MapToDomain(model));
        if (result.IsSuccess) return Ok(result.Success);
        return BadRequest(new ErrorViewModel(result.Failure));

    }
    
    [HttpPost("verify")]
    public async Task<IActionResult> StartVerifyAccountAsync([FromBody] EmailViewModel model)
    {
        var result = await _accountService.StartVerifyAccount(model.Email);
        if (!result.IsSuccess) return BadRequest(new ErrorViewModel(result.Failure));
        await _emailSender.SendEmailAsync(model.Email, "Подтждения регистрации", 
            "Подтвердите регистрацию нажав кнопку " + 
            $"ващ код для подтверждения регистрации {result.Success} перейдите обратно на сайт и введите код");
        return Ok("инструкция для подтверждения была отправлено на почту");
    }

    [HttpPut("confirm/{model:required}")]
    public async Task<IActionResult> ConfirmVerifyAsync(string codes)
    {
        var result = await _accountService.ConfirmVerifyAccount(codes);
        return result.IsSuccess ? Ok(result.Success) : StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel(result.Failure));
    }
}