using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class BillingInsuranceViewModel : ObservableObject
    {
        private ObservableCollection<InsuranceClaim> _claims;
        public ObservableCollection<InsuranceClaim> Claims
        {
            get => _claims;
            set => SetProperty(ref _claims, value);
        }

        public BillingInsuranceViewModel()
        {
            LoadClaims();
        }

        private void LoadClaims()
        {
            Claims = new ObservableCollection<InsuranceClaim>(DataManager.InsuranceClaims.OrderByDescending(c => c.SubmittedDate));
        }
    }
}
