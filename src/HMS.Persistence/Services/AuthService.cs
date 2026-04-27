using System;
using System.Linq;
using HMS.Core.Domain.Entities;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Interfaces;
using HMS.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;

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

        public User Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            // Use a completely fresh DbContext for every login check.
            // This guarantees EF's change-tracking cache cannot serve a stale (old) password.
            using (var freshContext = DatabaseFactory.CreateContext())
            {
                var user = freshContext.Users.AsNoTracking()
                    .FirstOrDefault(u => u.Email != null
                        && u.Email.ToLower() == email.Trim().ToLower()
                        && u.IsActive);

                if (user == null) return null;

                return PasswordHasher.VerifyPassword(password.Trim(), user.Password) ? user : null;
            }
        }

        public bool ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = _unitOfWork.Users.GetById(userId);
            if (user == null) return false;

            if (!PasswordHasher.VerifyPassword(oldPassword, user.Password)) return false;

            string hashedNew = PasswordHasher.HashPassword(newPassword);
            
            using (var context = DatabaseFactory.CreateContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Use direct SQL to ensure no EF caching/tracking issues
                        context.Database.ExecuteSqlRaw(
                            "UPDATE Users SET Password = {0} WHERE Id = {1}", 
                            hashedNew, userId);
                        
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        private void EnsureDefaultAdminExists()
        {
            if (!_unitOfWork.Users.Find(u => u.Email.ToLower() == "admin@hospital.com").Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Admin.ToString(),
                    Email = "admin@hospital.com",
                    IsActive = true
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Email.ToLower() == "doctor@hospital.com").Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Doctor.ToString(),
                    Email = "doctor@hospital.com",
                    IsActive = true,
                    DoctorId = 1 // Linking to the first seeded doctor
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Email.ToLower() == "recep@hospital.com").Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Receptionist.ToString(),
                    Email = "recep@hospital.com",
                    IsActive = true
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Email.ToLower() == "patient@hospital.com").Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Patient.ToString(),
                    Email = "patient@hospital.com",
                    IsActive = true,
                    PatientId = 1
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Email.ToLower() == "nurse@hospital.com").Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Nurse.ToString(),
                    Email = "nurse@hospital.com",
                    IsActive = true,
                    NurseId = 1
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Email.ToLower() == "pharmacist@hospital.com").Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.Pharmacist.ToString(),
                    Email = "pharmacist@hospital.com",
                    IsActive = true,
                    PharmacistId = 1
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Email.ToLower() == "labtech@hospital.com").Any())
            {
                _unitOfWork.Users.Add(new User
                {
                    Password = PasswordHasher.HashPassword("1234"),
                    Role = UserRole.LabTechnician.ToString(),
                    Email = "labtech@hospital.com",
                    IsActive = true,
                    LabTechnicianId = 1
                });
            }

            if (!_unitOfWork.Users.Find(u => u.Email.ToLower() == "billing@hospital.com").Any())
            {
                _unitOfWork.Users.Add(new User
                {
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
