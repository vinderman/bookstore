﻿using Bookstore.BL.Dto.Auth;
using Bookstore.BL.Interfaces;
using Bookstore.Shared.Exceptions;
using Bookstore.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IAuthService _authService;

    public AuthController(IConfiguration config, IAuthService authService)
    {
        _config = config;
        _authService = authService;
    }

    [HttpPost("login")]
    [ActionName("authByLogin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<AuthByLoginResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<AuthByLoginResponseDto>> Login([FromBody] AuthByLoginDto authByLoginDto)
    {
        try
        {
            var result = await _authService.Login(authByLoginDto);

            if (result == null)
            {
                return BadRequest("Произошла ошибка. Проверьте учетные данные");
            }

            return Ok(new SuccessResponse<AuthByLoginResponseDto>(result));
        }
        catch
        {
            throw;
        }
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<bool>> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _authService.Register(registerDto);
        return Ok(new SuccessResponse<bool>(result));

    }

    [HttpPost("refreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<RefreshTokenResponseDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<RefreshTokenResponseDto>> RefreshToken(string refreshToken)
    {
        var result = await _authService.RefreshToken(refreshToken);

        return Ok(new SuccessResponse<RefreshTokenResponseDto>(result));
    }
}
