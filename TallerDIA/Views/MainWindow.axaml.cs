using Avalonia.Controls;
using TallerDIA.ViewModels;

namespace TallerDIA.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _vm = new MainWindowViewModel();
        }
    }
}