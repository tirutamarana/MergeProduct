using CsvHelper.Configuration.Attributes;

namespace ProductsConsoleApp.Entities
{
    public class SuperCatalog
    {
        [Index(0)]
        public string SKU { get; set; }
        [Index(1)]
        public string Description { get; set; }
        [Index(2)]
        public string Source { get; set; }
    }
}
