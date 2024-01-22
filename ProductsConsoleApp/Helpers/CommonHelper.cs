using ProductsConsoleApp.Entities;
using Newtonsoft.Json;
using System.IO;

namespace ProductsConsoleApp.Helpers
{
    public static class CommonHelper
    {
        private readonly static DataConfig _dataConfig;
        static CommonHelper()
        {
            using (StreamReader reader = new StreamReader("DataConfig.json"))
            {
                var json = reader.ReadToEnd();
                _dataConfig = JsonConvert.DeserializeObject<DataConfig>(json);
            }
        }

        public static DataConfig DataConfig
        {
            get
            {
                return _dataConfig;
            }
        }
    }
}
