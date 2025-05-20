using System.IO;
using Newtonsoft.Json;

namespace Tetris.Helpers
{
    public static class JSONHelper
    {
        public static void WriteJSONToFile(string path, object data, bool prettyPrint = false)
        {
            EnsureDirectoryExists(path);
            var formatting = prettyPrint ? Formatting.Indented : Formatting.None;
            string json = JsonConvert.SerializeObject(data, formatting);
            File.WriteAllText(path, json);
        }
        
        public static T ReadJSONFromFile<T>(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }
        
        private static void EnsureDirectoryExists(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Directory.Exists)
            {
                Directory.CreateDirectory(file.DirectoryName);
            }
        }
    }
}