using Avalonia.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TallerDIA.Utils
{
    class Settings
    {
        
        private static Settings _instance;
        public static Settings Instance => _instance ??= new Settings();
        private static readonly string Path = "./settings.json";
        public Dictionary<string, string> filePaths { get; } = new Dictionary<string, string>();

        private  Settings()
        {
            LoadSettings();
        }
        public async Task saveSettigs()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(filePaths, options);

            await File.WriteAllTextAsync(Path, json);
        }

        public void AddFilePath(string file, string value)
        {
            if (filePaths.ContainsKey(file))
                filePaths[file] = value;
            else
                filePaths.Add(file, value);
        }

        public string GetFilepath(string file)
        {
            if (filePaths.ContainsKey(file))
                return filePaths[file];
            else return null;
        }

        public void LoadSettings()
        {
            if (!File.Exists(Path))
                return;

            try
            {
                var json = File.ReadAllText(Path);
                var loadedPaths = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                if (loadedPaths != null)
                {
                    foreach (var path in loadedPaths)
                    {
                        filePaths[path.Key] = path.Value;
                    }
                }
            }
            catch (JsonException ex)
            {
                //error todo
            }
        }
    }
}
