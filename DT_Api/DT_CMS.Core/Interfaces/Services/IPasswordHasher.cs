namespace DT_CMS.Core.Interfaces.Services;

public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string hashedPassword, string providedPassword);
}
