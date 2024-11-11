using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using TallerDIA.ViewModels;

namespace TallerDIA.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        private ViewModelBase _contentViewModel;
        public MainWindowViewModel()
        {
            _contentViewModel= new ClientesViewModel();
        }
        
        public ViewModelBase ContentViewModel
        {
            get => _contentViewModel;
            private set => OnPropertyChanged("_contentViewModel");
        }
        [RelayCommand]
        public void GoToGestorClientes()
        {
            ContentViewModel = new ClientesViewModel();
        }
    }
}
