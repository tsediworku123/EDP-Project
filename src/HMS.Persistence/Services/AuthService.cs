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

            if (!_unitOfWork.Users.Find(u => u.Role == UserRole.Nurse.ToString()).Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Username = "nurse",
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Nurse.ToString(),
                    Email = "nurse@hospital.com",
                    IsActive = true,
                    NurseId = 1
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Role == UserRole.Pharmacist.ToString()).Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Username = "pharmacist",
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Pharmacist.ToString(),
                    Email = "pharmacist@hospital.com",
                    IsActive = true,
                    PharmacistId = 1
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Role == UserRole.LabTechnician.ToString()).Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Username = "labtech",
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.LabTechnician.ToString(),
                    Email = "labtech@hospital.com",
                    IsActive = true,
                    LabTechnicianId = 1
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Role == UserRole.Billing.ToString()).Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Username = "billing",
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Billing.ToString(),
                    Email = "billing@hospital.com",
                    IsActive = true,
                    BillingStaffId = 1
                });
            }

            _unitOfWork.Complete();
        }
    }
}
