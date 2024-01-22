using CsvHelper.Configuration.Attributes;

namespace ProductsConsoleApp.Entities
{
    public class SupplierProductBarcode
    {
        [Index(0)]
        public int SupplierID { get; set; }
        [Index(1)]
        public string SKU { get; set; }
        [Index(2)]
        public string Barcode { get; set; }
    }
}
