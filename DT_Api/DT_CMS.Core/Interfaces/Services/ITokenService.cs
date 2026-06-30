using DT_CMS.Core.Entities;

namespace DT_CMS.Core.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
}
