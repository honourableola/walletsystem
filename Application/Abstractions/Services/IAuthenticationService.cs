using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface IAuthenticationService
    {
        string GenerateToken(User user);
    }
}
