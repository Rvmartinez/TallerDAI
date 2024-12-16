using Avalonia.Controls;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using TallerDIA.ViewModels;
using TallerDIA.Utils;

namespace TallerDIA.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _vm = new MainWindowViewModel();
            this.Closing += OnMainWindowClosing;
        }
        private async void OnMainWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // Previene el cierre de la ventana hasta que el usuario confirme
            e.Cancel = true;

            // Muestra un mensaje de confirmaci�n
            var box = MessageBoxManager
                .GetMessageBoxStandard("Cerrar App",
                                       "¿Estas seguro que quieres salir?",
                                       ButtonEnum.YesNo,
                                       MsBox.Avalonia.Enums.Icon.Question);

            var result = await box.ShowAsync();

            if (result == ButtonResult.Yes)
            {
                this.Closing -= OnMainWindowClosing;
                SharedDB.Instance.SaveAllXml();
                Settings.Instance.saveSettigs();
                this.Close();
            }
        }
    }
}