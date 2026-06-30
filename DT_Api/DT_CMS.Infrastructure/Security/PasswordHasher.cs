using DT_CMS.Core.Entities;
using DT_CMS.Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace DT_CMS.Infrastructure.Security;

/// <summary>
/// Wraps ASP.NET Core Identity's battle-tested PBKDF2 hasher without
/// pulling in the full Identity membership system.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _innerHasher = new();

    public string Hash(string password) => _innerHasher.HashPassword(null!, password);

    public bool Verify(string hashedPassword, string providedPassword)
    {
        var result = _innerHasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
