using HMS.Core.Domain.Entities;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Enums;
using System.Linq;

namespace HMS.Core.Infrastructure.Repositories.Json
{
    public class AuthService
    {
        private readonly JsonDataService _data;

        public AuthService(JsonDataService dataService)
        {
            _data = dataService;
            EnsureDefaultAdminExists();
        }

        /// <summary>
        /// Validates credentials. Returns the User on success, null on failure.
        /// </summary>
        public User Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            var users = _data.LoadUsers();
            var user  = users.FirstOrDefault(u =>
                u.Username.Equals(username.Trim(), System.StringComparison.OrdinalIgnoreCase)
                && u.IsActive);

            if (user == null) return null;

            bool valid = PasswordHasher.VerifyPassword(password, user.Password);
            return valid ? user : null;
        }

        /// <summary>
        /// Changes the password for a given user. Returns true on success.
        /// </summary>
        public bool ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var users = _data.LoadUsers();
            var user  = users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return false;

            if (!PasswordHasher.VerifyPassword(oldPassword, user.Password)) return false;

            user.Password = PasswordHasher.HashPassword(newPassword);
            _data.SaveUsers(users);
            return true;
        }

        /// <summary>
        /// Seeds a default admin account if no admin exists yet.
        /// Default credentials: admin / Admin@123
        /// </summary>
        private void EnsureDefaultAdminExists()
        {
            var users = _data.LoadUsers();
            if (users.Any(u => u.Role == UserRole.Admin.ToString())) return;

            users.Add(new User
            {
                Id       = 1,
                Username = "admin",
                Password = PasswordHasher.HashPassword("Admin@123"),
                Role     = UserRole.Admin.ToString(),
                Email    = "admin@hospital.com",
                IsActive = true
            });
            _data.SaveUsers(users);
        }
    }
}
