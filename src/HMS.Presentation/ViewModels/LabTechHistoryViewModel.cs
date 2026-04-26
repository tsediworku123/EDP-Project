using HMS.Core.AppLogic.Services;
using HMS.Core.Common.Utils;
using HMS.Core.Domain.Entities;
using System.Collections.ObjectModel;
using System.Linq;

namespace HMS.Core.ViewModels
{
    public class LabTechHistoryViewModel : ObservableObject
    {
        private ObservableCollection<LabTest> _testHistory;
        public ObservableCollection<LabTest> TestHistory 
        { 
            get => _testHistory; 
            set => SetProperty(ref _testHistory, value); 
        }

        private string _searchText;
        public string SearchText 
        { 
            get => _searchText; 
            set {
                if (SetProperty(ref _searchText, value))
                    FilterHistory();
            }
        }

        public LabTechHistoryViewModel()
        {
            LoadHistory();
        }

        private void LoadHistory()
        {
            var history = DataManager.LabTests
                .Where(t => t.Status == "Completed")
                .OrderByDescending(t => t.CompletedDate)
                .ToList();
            
            TestHistory = new ObservableCollection<LabTest>(history);
        }

        private void FilterHistory()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadHistory();
                return;
            }

            var query = SearchText.ToLower();
            var filtered = DataManager.LabTests
                .Where(t => t.Status == "Completed" && 
                       (t.TestName.ToLower().Contains(query) || 
                        t.LabTechnicianName?.ToLower().Contains(query) == true))
                .OrderByDescending(t => t.CompletedDate)
                .ToList();
            
            TestHistory = new ObservableCollection<LabTest>(filtered);
        }
    }
}
