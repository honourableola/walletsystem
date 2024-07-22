using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Models.Request;
using Application.Models.Response;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRoleRepository _roleRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IWalletRepository walletRepository, IUnitOfWork unitOfWork, IAuditLogRepository auditLogRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
            _auditLogRepository = auditLogRepository;
            _authenticationService = authenticationService;
        }

        public async Task<LoginResponseModel> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmail(request.Email);
            if (user == null)
            {
                return new LoginResponseModel
                {
                    Message = "Invalid email or password"
                };
            }
            var saltedPassword = $"{request.Password}{user.HashSalt}";

            if (!BCrypt.Net.BCrypt.Verify(saltedPassword, user.PasswordHash))
            {
                return new LoginResponseModel
                {
                    Message = "Invalid email or password"
                };
            }

            var token = _authenticationService.GenerateToken(user);

            return new LoginResponseModel
            {
                Email = user.Email,
                Role = user.Role.Name,
                Message = "Login successful",
                Token = token
            };
        }

        public async Task<UserResponse> RegisterAsync(CreateUserRequest request)
        {
            var userExists = await _userRepository.Exists(request.Email);
            if (userExists)
            {
                return new UserResponse
                {
                    Message = $"User with email {request.Email} already exist"
                };
            }

            var role = await _roleRepository.GetByName("standard");
            if (role == null)
            {
                return new UserResponse
                {
                    Message = $"Role Not found"
                };
            }

            var salt = Guid.NewGuid().ToString();
            var saltedPassword = $"{request.Password}{salt}";
            var user = new User
            {
                Email = request.Email,
                HashSalt = salt,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(saltedPassword),
                RoleId = role.Id
            };

            var wallet = new Wallet
            {
                UserId = user.Id,
                Currency = request.CurrencyType,
                WalletAddress = GenerateWalletAddress(user)
            };

            var auditLog = new AuditLog
            {
                Action = AuditAction.Registered,
                Details = $"User with email {request.Email} registered",
                UserId = user.Id,
            };

            await _userRepository.Create(user);
            await _walletRepository.Create(wallet);

            if(await _unitOfWork.SaveChangesAsync() > 0)
            {
                await _auditLogRepository.Create(auditLog);
                await _unitOfWork.SaveChangesAsync();
            }
            return new UserResponse
            {
                Data = new UserModel
                {
                    Email = user.Email,
                    Role = user.Role.Name
                },
                Message = "user registered successfully",
                Status = true
            };
        }

        private string GenerateWalletAddress(User user) => $"{user.Email}{Guid.NewGuid()}{DateTime.Now.Year}";
     
    }
}
