using MasterProductsConsoleApp.Entities;
using System.Collections.Generic;

namespace MasterProductsConsoleApp.Services
{
    public interface IService
    {
        /// <summary>
        /// Read data from CSV and convert it to List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        List<T> ReadData<T>(string filePath);

        /// <summary>
        /// Write output data to CSV file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="entities"></param>
        void WriteData<T>(string filePath, IEnumerable<T> entities);

        /// <summary>
        /// Process catalogs and return mega catalog list
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        IEnumerable<SuperCatalog> MergeProducts(DataConfig config);
    }
}
