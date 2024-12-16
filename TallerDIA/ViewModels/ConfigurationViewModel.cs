using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TallerDIA.Utils;

namespace TallerDIA.ViewModels
{
    partial class ConfigurationViewModel : ViewModelBase
    {
        public ConfigurationViewModel()
        {
            Dictionary<string, string> paths = Settings.Instance.filePaths;
            garajePath = paths.GetValueOrDefault("garaje");
            plantillaPath = paths.GetValueOrDefault("plantilla");
            clientesPath = paths.GetValueOrDefault("clientes");
            TrabajosPath = paths.GetValueOrDefault("trabajos");


        }
        private string? _garajePath;
        public string? garajePath
        {
            get => _garajePath;
            set => SetProperty(ref _garajePath, value);
        }

        private string? _plantillaPath;
        public string? plantillaPath
        {
            get => _plantillaPath;
            set => SetProperty(ref _plantillaPath, value);
        }


        private string? trabajosPath;
        public string? TrabajosPath
        {
            get => trabajosPath;
            set => SetProperty(ref trabajosPath, value);
        }


        private string? _clientesPath;
        public string? clientesPath
        {
            get => _clientesPath;
            set => SetProperty(ref _clientesPath, value);
        }
        private string? _reparacionesPath;
        public string? reparacionesPath
        {
            get => _reparacionesPath;
            set => SetProperty(ref _reparacionesPath, value);
        }


        [RelayCommand]
        public async Task RequestFolder()
        {
            string folder = await IOUtil.RequestFolderPath()?? "";
            if (folder == "") return;
            garajePath = Path.Combine(folder, "garaje.xml");
            plantillaPath = Path.Combine(folder, "plantilla.xml");
            clientesPath = Path.Combine(folder, "clientes.xml");
            reparacionesPath = Path.Combine(folder, "trabajo.xml");


            Settings.Instance.AddFilePath("plantilla", plantillaPath);
            Settings.Instance.AddFilePath("trabajo", reparacionesPath);
            Settings.Instance.AddFilePath("clientes", clientesPath);
            Settings.Instance.AddFilePath("plantilla", clientesPath);
        }

    }
}
