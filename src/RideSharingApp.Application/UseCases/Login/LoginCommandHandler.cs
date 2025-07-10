using Microsoft.IdentityModel.Tokens;
using RideSharingApp.Application.Abstractions.Messaging;
using RideSharingApp.Application.Common.Interfaces;
using RideSharingApp.SharedKernel.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RideSharingApp.Application.UseCases.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepo;
    private readonly string _jwtKey;

    public LoginCommandHandler(IUserRepository userRepo, string jwtKey)
    {
        _userRepo = userRepo;
        _jwtKey = jwtKey;
    }

    public async Task<Result<LoginResponse>> HandleAsync(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByEmailAsync(command.Email);
        if (user == null || user.PasswordHash != command.Password) // Hash check simplificado
        {
            return Result.Failure<LoginResponse>(Error.NotFound("Login.InvalidCredentials", "Credenciais inválidas."));
        }

        // Gerar JWT
        var claims = new[] { new Claim("sub", user.Id.ToString()) };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );
        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
        return Result.Success(new LoginResponse(tokenStr));
    }
}
