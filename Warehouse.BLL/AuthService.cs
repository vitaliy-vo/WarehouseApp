using Mapster;
using System.Security.Cryptography;
using System.Text;
using Warehouse.Core.Dtos;
using Warehouse.Core.OutPutModels;
using Warehouse.DAL;

namespace Warehouse.BLL
{
    public class AuthService
    {
        private IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool Authenticate(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);

            if (user == null)
            {
                return false;
            }

            return VerifyPassword(password, user.Password, user.Salt);
        }

        private bool VerifyPassword(string password, string storedHash, string salt)
        {
            var hash = ComputeHash(password, salt);

            return hash.Equals(storedHash);
        }

        private string ComputeHash(string password, string salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            Encoding.UTF8.GetBytes(salt),
            10000,
            HashAlgorithmName.SHA256);

            byte[] hash = pbkdf2.GetBytes(20);

            return Convert.ToBase64String(hash);
        }

        public UserOutputModel GetUser(string username)
        {
            var tmp = _userRepository.GetByUsername(username);
            var user = tmp.Adapt<UserOutputModel>();

            return user;
        }

        public void RegisterUser(string username, string password, string role)
        {
            var salt = GenerateSalt();
            var passwordHash = ComputeHash(password, salt);
            var user = new UserDTO
            {
                Login = username,
                Password = passwordHash,
                Role = role,
                Salt = salt
            };
            _userRepository.Add(user);
        }

        private string GenerateSalt()
        {
            byte[] salt = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }
    }
}
