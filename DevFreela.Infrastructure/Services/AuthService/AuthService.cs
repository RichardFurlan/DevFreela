using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DevFreela.Infrastructure.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateJwtToken(string email)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = _configuration["Jwt:Key"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        

        var token = new JwtSecurityToken(
            issuer: issuer, 
            audience: audience, 
            expires: DateTime.Now.AddHours(8), 
            signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();

        var stringToken = tokenHandler.WriteToken(token);

        return stringToken;

    }

    public string ComputeSha256Hash(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - retorna byte array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Converte byte array para string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("X2")); // X2 faz com que seja convertido em representação hexadecimal
            }

            return builder.ToString();
        }
    }


    public string? ValidarSenha(string password, string passwordConfirm)
    {
        if (password != passwordConfirm)
        {
            return "As senhas não são idênticas";
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return "A senha não pode estar em branco.";
        }

        if (password.Length < 6)
        {
            return "A senha deve ter pelo menos 6 caracteres.";
        }

        if (!password.Any(char.IsDigit))
        {
            return "A senha deve conter pelo menos um número";
        }
        
        if (!password.Any(char.IsLetter))
        {
            return "A senha deve conter pelo menos uma letra.";
        }

        return null;
    }
}