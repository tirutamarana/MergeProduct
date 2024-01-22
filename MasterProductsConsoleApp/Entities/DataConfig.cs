
using System.Collections.Generic;

namespace MasterProductsConsoleApp.Entities
{
    public class DataConfig
    {
        public string InputFolderPath { get; set; }
        public List<CompanyFile> CompanyFiles { get; set; }
        public string OutputFolderPath { get; set; }
        public string OutputFileName { get; set; }
    }
    public class CompanyFile
    {
        public string Source { get; set; }
        public string Catalog { get; set; }
        public string SupplierBarcodes { get; set; }
        public string Suppliers { get; set; }
    }
}
