
using API.Authorization;
using API.Helpers;
using CORE.Models;
using CORE.ViewModels.Authenticate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(string UserName, string UserPassword, string ipAddress);
        AuthenticateResponse AuthenticateSms(int UserId, string ipAddress);
        IEnumerable<ikys_user> GetAll();
        ikys_user GetById(int id);
    }

    public class UserService : IUserService
    {
        private AppDbContext _context;
        private IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;

        public UserService(
            AppDbContext context,
            IJwtUtils jwtUtils,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(string UserName, string UserPassword, string ipAddress)
        {
            var user = _context.ikys_users.SingleOrDefault(x => x.UserName == UserName && x.UserPassword == UserPassword && x.UserStatus == true);

            // validate
            if (user == null)
            {
                // Loglama
                Console.WriteLine($"Authentication failed for username: {UserName}");
                throw new AppException("Username or password is incorrect");
            }

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = _jwtUtils.GenerateJwtToken(user);
            return new AuthenticateResponse(user, jwtToken);
        }

        public AuthenticateResponse AuthenticateSms(int UserId, string ipAddress)
        {
            var user = _context.ikys_users.SingleOrDefault(x => x.Id == UserId && x.UserStatus == true);

            // validate
            if (user == null)
            {
                throw new AppException("Username or password is incorrect");
            }

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            // save changes to db
            _context.Update(user);
            _context.SaveChanges();

            return new AuthenticateResponse(user, jwtToken);
        }

        public IEnumerable<ikys_user> GetAll()
        {
            return _context.ikys_users.Take(1);
        }

        public ikys_user GetById(int id)
        {
            var user = _context.ikys_users.Where(w => w.Id == id).FirstOrDefault();
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }

    }
}