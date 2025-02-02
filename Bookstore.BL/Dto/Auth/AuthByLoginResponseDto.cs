﻿namespace Bookstore.BL.Dto.Auth;

public class AuthByLoginResponseDto : RefreshTokenResponseDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public string? Email { get; set; }



    public string RoleName { get; set; } = string.Empty;
}
