using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
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
            Window? mainWindow = null;

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                mainWindow = desktop.MainWindow;
            }
            else
            {
                mainWindow = new Window();
            }
            if (mainWindow?.StorageProvider != null)
            {
                var openFolderOptions = new FolderPickerOpenOptions
                {
                    Title = "Seleccionar una Carpeta"
                };
                IReadOnlyList<IStorageFolder>? folder = await mainWindow.StorageProvider.OpenFolderPickerAsync(openFolderOptions);

                if (folder != null)
                {
                    return folder.FirstOrDefault()?.Path.AbsolutePath;
                }
            }

            return null;
        }
    }
}
