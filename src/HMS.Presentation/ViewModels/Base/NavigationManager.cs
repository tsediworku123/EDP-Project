
using HMS.Core.AppLogic.Services;

using System;
 
namespace HMS.Core.ViewModels.Base {
    /// <summary>     /// Manages page navigation within a shell window.     /// Shells bind their ContentControl.Content to CurrentPage.     /// </summary>     

    public class  NavigationManager : BaseViewModel     {
        
    private static NavigationManager _instance;
        
    public static NavigationManager Instance => _instance ?? (_instance = new NavigationManager());
         
    private NavigationManager() {
}          
    private BaseViewModel _currentPage;
        
    public BaseViewModel CurrentPage         {
            get => _currentPage;
            
    private set => SetProperty(ref _currentPage, value);
       
}          
    private string _currentPageTitle;
        
    public string CurrentPageTitle         {
            get => _currentPageTitle;
            
    private set => SetProperty(ref _currentPageTitle, value);
       
}          
    public event EventHandler<BaseViewModel> PageChanged;
         
    public void NavigateTo(BaseViewModel page, string title = "")         {
            CurrentPage      = page;
            CurrentPageTitle = title;
            PageChanged?.Invoke(this, page);
       
}    
}
}

