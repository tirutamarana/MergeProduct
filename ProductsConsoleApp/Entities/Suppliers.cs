using CsvHelper.Configuration.Attributes;

namespace ProductsConsoleApp.Entities
{
    public class Suppliers
    {
        [Index(0)]
        public int ID { get; set; }
        [Index(1)]
        public string Name { get; set; }
    }
}
