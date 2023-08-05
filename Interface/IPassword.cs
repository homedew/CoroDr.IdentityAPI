using System.Security.Cryptography;
using System.Text;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string password);
}

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        return hashedPassword == HashPassword(password);
    }
}
