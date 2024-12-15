using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TallerDIA.Utils
{
    public static class IOUtil
    {
        public static async Task<string?> RequestFolderPath()
        {

            Window mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
            if(mainWindow == null)
            {
                mainWindow = new Window();
            }
            var dialog = new OpenFolderDialog
            {
                Title = "Seleccionar Carpeta"
            };

            string? result = await dialog.ShowAsync(mainWindow);

            if (result != null)
            {
                return result;
            }

            return null;
        }
    }
}
