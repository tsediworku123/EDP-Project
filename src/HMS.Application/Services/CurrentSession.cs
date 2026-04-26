using HMS.Core.Domain.Entities;
using HMS.Core.Domain.Enums;
using System;

namespace HMS.Core.AppLogic.Services
{
    public class CurrentSession
    {
        private static CurrentSession _instance;
        public static CurrentSession Instance => _instance ?? (_instance = new CurrentSession());

        private CurrentSession() { }

        public User   LoggedInUser   { get; private set; }
        public Doctor LoggedInDoctor { get; private set; }
        public Patient LoggedInPatient { get; private set; }
        public Nurse LoggedInNurse { get; private set; }
        public Pharmacist LoggedInPharmacist { get; private set; }

        public bool IsLoggedIn => LoggedInUser != null;
        public bool IsAdmin    => LoggedInUser?.Role == UserRole.Admin.ToString();
        public bool IsDoctor   => LoggedInUser?.Role == UserRole.Doctor.ToString();
        public bool IsPatient  => LoggedInUser?.Role == UserRole.Patient.ToString();
        public bool IsNurse    => LoggedInUser?.Role == UserRole.Nurse.ToString();
        public bool IsPharmacist => LoggedInUser?.Role == UserRole.Pharmacist.ToString();

        public event EventHandler SessionStarted;
        public event EventHandler SessionEnded;

        public void StartSession(User user, Doctor doctor = null, Patient patient = null, Nurse nurse = null, Pharmacist pharmacist = null)
        {
            LoggedInUser    = user;
            LoggedInDoctor  = doctor;
            LoggedInPatient = patient;
            LoggedInNurse   = nurse;
            LoggedInPharmacist = pharmacist;
            SessionStarted?.Invoke(this, EventArgs.Empty);
        }

        public void EndSession()
        {
            LoggedInUser    = null;
            LoggedInDoctor  = null;
            LoggedInPatient = null;
            LoggedInNurse   = null;
            LoggedInPharmacist = null;
            SessionEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}
