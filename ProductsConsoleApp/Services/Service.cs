using ProductsConsoleApp.Entities;
using ProductsConsoleApp.Helpers;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsConsoleApp.Services
{
    public class Service : IService
    {
        public Service()
        {
        }

        public List<T> ReadData<T>(string filePath)
        {
            //TODO: Handle exceptions and log

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found at {filePath}");


            List<T> records = null;
            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    records = csv.GetRecords<T>().ToList();
                }
            }

            return records;
        }

        public IEnumerable<SuperCatalog> MergeProducts(DataConfig config)
        {
            //TODO: Handle exceptions and log
            if (config is null)
                throw new ArgumentNullException($"param {config} is null");

            var companyAConfigData = config.CompanyFiles.FirstOrDefault(m => m.Source.Equals(SourceEnum.A.ToString(), StringComparison.InvariantCultureIgnoreCase));
            var companyBConfigData = config.CompanyFiles.FirstOrDefault(m => m.Source.Equals(SourceEnum.B.ToString(), StringComparison.InvariantCultureIgnoreCase));
            List<Catalog> companyBCatalogs = null;
            List<SupplierProductBarcode> companyBProductBarcodes = null;
            List<Catalog> companyACatalogs = null;
            List<SupplierProductBarcode> companyAProductBarcodes = null;

            //Paraller call
            Parallel.Invoke(
                () => companyACatalogs = ReadData<Catalog>(Path.Combine(config.InputFolderPath, companyAConfigData.Catalog)),
                () => companyAProductBarcodes = ReadData<SupplierProductBarcode>(Path.Combine(config.InputFolderPath, companyAConfigData.SupplierBarcodes)),
                () => companyBCatalogs = ReadData<Catalog>(Path.Combine(config.InputFolderPath, companyBConfigData.Catalog)),
                () => companyBProductBarcodes = ReadData<SupplierProductBarcode>(Path.Combine(config.InputFolderPath, companyBConfigData.SupplierBarcodes))
                );
            //ends here

            var mergedProductBarcodes = companyAProductBarcodes.Select(m => new { m.SKU, m.Barcode, Source = SourceEnum.A.ToString() }).ToList();
            mergedProductBarcodes.AddRange(companyBProductBarcodes.Select(m => new { m.SKU, m.Barcode, Source = SourceEnum.B.ToString() }));

            //Group by bardcodes and get the respective source and sku
            var filteredBarcodes = (from a in mergedProductBarcodes
                                    group a by a.Barcode into grp
                                    select new
                                    {
                                        Bardcode = grp.Key,
                                        Source = grp.FirstOrDefault().Source,
                                        SKU = grp.FirstOrDefault().SKU
                                    }).ToList();

            //now, narrow down to sku and source from barcodes list
            var filteredCatalogs = filteredBarcodes.GroupBy(m => new { m.SKU, m.Source }).Select(m => m.Key).ToList();

            //Merge catalogs from A and B by separating them with source
            var mergedCatalogs = companyACatalogs.Select(m => new { m.SKU, m.Description, Source = SourceEnum.A.ToString() }).ToList();
            mergedCatalogs.AddRange(companyBCatalogs.Select(m => new { m.SKU, m.Description, Source = SourceEnum.B.ToString() }));

            //join the filtered bard codes with merged catalogs to get the mega catalog list
            var megaCatalog = (from filter in filteredCatalogs
                               join merge in mergedCatalogs on
                               new { filter.SKU, filter.Source } equals new { merge.SKU, merge.Source }
                               select new SuperCatalog { SKU = merge.SKU, Description = merge.Description, Source = merge.Source }).ToList();

            return megaCatalog;
        }

        public void WriteData<T>(string filePath, IEnumerable<T> entities)
        {
            //TODO: Handle exceptions and log

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords<T>(entities);
                }
            }
        }
    }
}
