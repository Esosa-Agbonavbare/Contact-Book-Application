using ContactBookApp.Model;

namespace ContactBookApp.Core.Services.Interfaces
{
    public interface ITokenGeneratorService
    {
        Task<string> GenerateToken(User user, string roles);
    }
}
