using Evenda.App.Contracts;
using Evenda.App.Contracts.IInfrastructure.IHasher;
using Evenda.App.Contracts.IInfrastructure.ITokenProvider;
using Evenda.App.Contracts.IPersistence.IRepositories;
using Evenda.App.Contracts.IPersistence.IUnitOfWork;
using Evenda.App.Contracts.IServices.IAuth;
using Evenda.App.Contracts.IValidators;
using Evenda.App.Dtos.Auth;
using Evenda.App.Models;
using Evenda.App.Utils.Constants;
using Evenda.Domain.Entities.UserEntities;
using Evenda.Services.Services.Base;

namespace Evenda.App.Services.Auth
{
    public class AuthService : BaseService, IAuthService
    {
        #region Fields

        private readonly IValidatorDispatcher _validator;
        private readonly IHasher _hasher;
        private readonly ITokenProvider _tokenProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly IBaseRepository<UserSession> _userSessionsRepo;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public AuthService(IValidatorDispatcher validator, IUnitOfWork unitOfWork, IHasher hasher, ITokenProvider tokenProvider, IWorkContext workContext)
        {
            _validator = validator;
            _unitOfWork = unitOfWork;
            _userRepo = _unitOfWork.GetCustomRepository<IUserRepository, User>();
            _userSessionsRepo = _unitOfWork.GetRepository<UserSession>();
            _hasher = hasher;
            _tokenProvider = tokenProvider;
            _workContext = workContext;
        }

        #endregion

        #region Utils

        protected async Task<AuthDto> RefreshUserSession(User user)
        {
            var (accessToken, tokenExpiration) = await _tokenProvider.GenerateAccessToken(user);
            var refreshToken = _tokenProvider.GenerateRefreshToken();
            var expireAt = DateTime.UtcNow.AddDays(Constants.REFRESH_TOKEN_EXPIRATION_TIME_IN_DAYES);

            user.UserSessions.Add(new UserSession
            {
                Token = refreshToken,
                ExpireAt = expireAt,
            });

            await _unitOfWork.SaveChangesAsync();

            var authDto = new AuthDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                AccessToken = accessToken,
                AccessTokenExpirationDate = tokenExpiration,
                RefreshToken = refreshToken,
                RefreshTokenExpirationDate = expireAt
            };

            return authDto;
        }

        #endregion

        #region Features

        public async Task<DataResponse<AuthDto>> Login(LoginDto loginDto)
        {
            var validationResult = _validator.Validate(loginDto);
            if (!validationResult.IsValid)
                return ValidationError<AuthDto>(validationResult.Errors);

            var user = await _userRepo.GetUserWithSessionsAndRolesByEmail(loginDto.Email);

            if (user == null || !_hasher.Verify(user.PasswordHash, loginDto.Password))
                return Unauthorized<AuthDto>("Invalid email or password");

            var authDto = await RefreshUserSession(user);

            return Success(authDto);
        }

        public async Task<DataResponse<AuthDto>> RefreshUserTokens(RefreshTokenDto refreshTokenDto)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenDto.RefreshToken))
                return Unauthorized<AuthDto>();

            var expiredAccessToken = _workContext.GetAccessToken();
            var userId = _workContext.GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(expiredAccessToken) || string.IsNullOrEmpty(userId))
                return Unauthorized<AuthDto>();

            var user = await _userRepo.GetUserWithSessionsAndRolesById(userId);
            var activeSession = user.UserSessions
                .FirstOrDefault(s => s.Token.Equals(refreshTokenDto.RefreshToken));

            if (activeSession == null)
                return Unauthorized<AuthDto>();

            _userSessionsRepo.Delete(activeSession);

            var authDto = await RefreshUserSession(user);

            return Success(authDto);
        }

        public async Task<DataResponse<Guid>> Register(RegisterDto registerDto)
        {
            var validationResult = _validator.Validate(registerDto);
            if (!validationResult.IsValid)
                return ValidationError<Guid>(validationResult.Errors);

            if (await _userRepo.Exists(u => u.Email.Equals(registerDto.Email)))
            {
                validationResult.AddError("Email", "Email already exists");
                return ValidationError<Guid>(validationResult.Errors);
            }

            var customerRole = await _unitOfWork.GetRepository<Role>()
                .FirstOrDefaultAsync(r => r.SystemName.Equals(Constants.CUSTOMER_ROLE_NAME))
                ?? throw new ArgumentNullException();

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = _hasher.Hash(registerDto.Password),
                Roles = new List<Role> { customerRole }
            };

            await _userRepo.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Created(user.Id);
        }

        #endregion
    }
}
