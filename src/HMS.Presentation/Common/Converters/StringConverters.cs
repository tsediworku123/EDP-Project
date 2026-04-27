using System;
using System.Globalization;
using System.Windows.Data;
using System.Linq;

namespace HMS.Core.Common.Converters
{
    public class FirstLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrWhiteSpace(str))
            {
                return str.Substring(0, 1).ToUpper();
            }
            return "?";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class UserToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HMS.Core.Domain.Entities.User user)
            {
                if (user.DoctorId.HasValue) return HMS.Core.AppLogic.Services.DataManager.Doctors.FirstOrDefault(d => d.Id == user.DoctorId)?.FullName ?? "Unknown Doctor";
                if (user.NurseId.HasValue) return HMS.Core.AppLogic.Services.DataManager.Nurses.FirstOrDefault(n => n.Id == user.NurseId)?.FullName ?? "Unknown Nurse";
                if (user.PatientId.HasValue) return HMS.Core.AppLogic.Services.DataManager.Patients.FirstOrDefault(p => p.Id == user.PatientId)?.FullName ?? "Unknown Patient";
                if (user.PharmacistId.HasValue) return HMS.Core.AppLogic.Services.DataManager.Pharmacists.FirstOrDefault(p => p.Id == user.PharmacistId)?.FullName ?? "Unknown Pharmacist";
                if (user.LabTechnicianId.HasValue) return HMS.Core.AppLogic.Services.DataManager.LabTechnicians.FirstOrDefault(l => l.Id == user.LabTechnicianId)?.FullName ?? "Unknown Lab Tech";
                if (user.BillingStaffId.HasValue) return HMS.Core.AppLogic.Services.DataManager.BillingStaff.FirstOrDefault(b => b.Id == user.BillingStaffId)?.FullName ?? "Unknown Billing Staff";
                
                return "System Administrator";
            }
            return "System User";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
