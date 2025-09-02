namespace MillionLuxury.WebApi.Controllers.Authentication;

#region Usings
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MillionLuxury.Application.Users.LogIn;
using MillionLuxury.Application.Users.Register;
using MillionLuxury.Application.Users.WhoAmI;
using MillionLuxury.Domain.Abstractions;
using MillionLuxury.Domain.Users;
#endregion

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost("login")]
    public async Task<IActionResult> LogIn(LogInRequest request, CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(request.Email, request.Password);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : HandleError(result.Error);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(request.Email, request.Password, request.Roles);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : HandleError(result.Error);
    }

    [Authorize]
    [HttpGet("whoami")]
    public async Task<IActionResult> WhoAmI(CancellationToken cancellationToken)
    {
        var query = new WhoAmIUserQuery();

        var result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);

    }

    #region Private methods
    private IActionResult HandleError(Error error)
    {
        return error switch
        {
            _ when error.Code == UserErrors.NotFound.Code => Unauthorized(error),
            _ when error.Code == UserErrors.InvalidCredentials.Code => Unauthorized(error),
            _ when error.Code == UserErrors.EmailAlreadyExists("").Code => Conflict(error),
            _ => BadRequest(error)
        };
    }
    #endregion
}