using CsvHelper.Configuration.Attributes;

namespace ProductsConsoleApp.Entities
{
    public class Catalog
    {
        [Index(0)]
        public string SKU { get; set; }
        [Index(1)]
        public string Description { get; set; }
    }
}
