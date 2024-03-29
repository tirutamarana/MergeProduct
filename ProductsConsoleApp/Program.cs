﻿using ProductsConsoleApp.Entities;
using ProductsConsoleApp.Helpers;
using ProductsConsoleApp.Services;
using System;
using System.IO;

namespace ProductsConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started...!");
            DataConfig config = CommonHelper.DataConfig;

            IService service = new Service();

            Console.WriteLine("Processing products info stated...!");
            var mergedProducts = new Service().MergeProducts(config);
            Console.WriteLine("Processing products info ended...!");

            service.WriteData<SuperCatalog>(Path.Combine(config.OutputFolderPath, config.OutputFileName), mergedProducts);
            Console.WriteLine("File created with mega catalog...!");
            Console.WriteLine("Ended...!");

            Console.ReadLine();
        }
    }
}
