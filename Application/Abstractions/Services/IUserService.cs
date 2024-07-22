using Application.Models.Request;
using Application.Models.Response;

namespace Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<UserResponse> RegisterAsync(CreateUserRequest request);
        Task<LoginResponseModel> LoginAsync(LoginRequest request);
    }
}
