using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager
{
    internal class JSON
    {
        public static string FilePath = "Finance.json";
        public static void Serialize<T>(T data, string fileName)
        {
            checkFile();
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(fileName, json);
        }

        public static T Deserialize<T>(string fileName)
        {
            checkFile();
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string json = File.ReadAllText(fileName);
            T data = JsonConvert.DeserializeObject<T>(json);
            return data;

        }
        private static void checkFile()
        {

            if (!File.Exists(FilePath))
            {
                using (StreamWriter sw = File.CreateText(FilePath))
                {
                    sw.WriteLine("[]");
                }
            }
        }
    }
}
