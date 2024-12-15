using Avalonia.Controls;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
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
        private async void OnMainWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            var box = MessageBoxManager
                .GetMessageBoxStandard("Cerrar App",
                                       "¿Estas seguro que quieres salir?",
                                       ButtonEnum.YesNo,
                                       MsBox.Avalonia.Enums.Icon.Question);

            var result = await box.ShowAsync();

            if (result == ButtonResult.Yes)
            {

                e.Cancel = false;
            }
        }
    }
}