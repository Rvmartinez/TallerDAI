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
        private string _garajePath;
        public string garajePath
        {
            get => _garajePath;
            set => SetProperty(ref _garajePath, value);
        }

        private string _plantillaPath;
        public string plantillaPath
        {
            get => _plantillaPath;
            set => SetProperty(ref _plantillaPath, value);
        }


        private string trabajosPath;
        public string TrabajosPath
        {
            get => trabajosPath;
            set => SetProperty(ref trabajosPath, value);
        }


        private string _clientesPath;
        public string clientesPath
        {
            get => _clientesPath;
            set => SetProperty(ref _clientesPath, value);
        }


        [RelayCommand]
        public async Task RequestGarajeFolder()
        {
            string folder = await IOUtil.RequestFolderPath();
            garajePath = Path.Combine(folder, "garaje.xml");
            Settings.Instance.AddFilePath("garaje", garajePath);
        }


        [RelayCommand]
        public async Task RequestPlantillaFolder()
        {
            string folder = await IOUtil.RequestFolderPath();
            plantillaPath = Path.Combine(folder, "plantilla.xml");
            Settings.Instance.AddFilePath("plantilla", plantillaPath);
        }

        [RelayCommand]
        public async Task RequestClientesFolder()
        {
            string folder = await IOUtil.RequestFolderPath();
            clientesPath = Path.Combine(folder, "clientes.xml");
            Settings.Instance.AddFilePath("clientes", clientesPath);
        }

        [RelayCommand]
        public async Task RequestTrabajosFolder()
        {
            string folder = await IOUtil.RequestFolderPath();
            TrabajosPath = Path.Combine(folder, "trabajo.xml");
            Settings.Instance.AddFilePath("trabajo", TrabajosPath);
        }
    }
}
