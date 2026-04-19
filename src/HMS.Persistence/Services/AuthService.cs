using System;
using System.Linq;
using HMS.Core.Domain.Entities;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Interfaces;
using HMS.Core.Domain.Enums;

namespace HMS.Core.Persistence.Services
{
    public class AuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            EnsureDefaultAdminExists();
        }

        public User Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = _unitOfWork.Users.Find(u =>
                u.Username.ToLower() == username.Trim().ToLower()
                && u.IsActive).FirstOrDefault();

            if (user == null) return null;

            bool valid = PasswordHasher.VerifyPassword(password, user.Password);
            return valid ? user : null;
        }

        public bool ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = _unitOfWork.Users.GetById(userId);
            if (user == null) return false;

            if (!PasswordHasher.VerifyPassword(oldPassword, user.Password)) return false;

            user.Password = PasswordHasher.HashPassword(newPassword);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Complete();
            return true;
        }

        private void EnsureDefaultAdminExists()
        {
            if (!_unitOfWork.Users.Find(u => u.Role == UserRole.Admin.ToString()).Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Username = "admin",
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Admin.ToString(),
                    Email = "admin@hospital.com",
                    IsActive = true
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Role == UserRole.Doctor.ToString()).Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Username = "doctor",
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Doctor.ToString(),
                    Email = "doctor@hospital.com",
                    IsActive = true,
                    DoctorId = 1 // Linking to the first seeded doctor
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Role == UserRole.Receptionist.ToString()).Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Username = "recep",
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Receptionist.ToString(),
                    Email = "recep@hospital.com",
                    IsActive = true
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Role == UserRole.Patient.ToString()).Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Username = "patient",
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Patient.ToString(),
                    Email = "patient@hospital.com",
                    IsActive = true,
                    PatientId = 1
                });
            }

            _unitOfWork.Complete();
        }
    }
}
