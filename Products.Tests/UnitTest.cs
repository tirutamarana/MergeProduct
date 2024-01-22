using ProductsConsoleApp.Entities;
using ProductsConsoleApp.Helpers;
using ProductsConsoleApp.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Products.Tests
{
    public class Tests
    {
        private IService _service;
        [SetUp]
        public void Setup()
        {
            _service = new Service();
        }

        [TestCase("result_output_test.csv")]
        [TestCase("DataConfig.json")]
        [Order(1)]
        public void Check_If_TestAndConfigDataFilesExists(string name)
        {
            Assert.IsTrue(File.Exists(name), $"File {name} doesn't exists. Kindly check if the file exists in project directory.");
        }

        [Test]
        public void ReadData_ShouldReturnFileNotFound()
        {
            Assert.Throws<FileNotFoundException>(() => _service.ReadData<object>(@"C:\unknownpath\catalogA.csv"));
        }

        [Test]
        public void MergeProducts_ShouldProductCorrectResults()
        {
            //Arrange
            //Load test output
            List<SuperCatalog> testOutput = _service.ReadData<SuperCatalog>("result_output_test.csv");


            //Act
            var finalResult = _service.MergeProducts(CommonHelper.DataConfig).ToList();
            //Both SKUs and Sources matching count
            int count = (testOutput.Select(m => new { m.SKU, m.Source }).Intersect(finalResult.Select(m => new { m.SKU, m.Source }))).Count();

            //Assert
            Assert.AreEqual(testOutput.Count, finalResult.Count);
            Assert.AreEqual(testOutput.Count, count);
        }

        [Test]
        public void WriteData_ShouldCreateFileWithData()
        {
            //Arrange
            string outPutFile = Path.Combine(CommonHelper.DataConfig.OutputFolderPath, CommonHelper.DataConfig.OutputFileName);

            //Act
            var finalResult = _service.MergeProducts(CommonHelper.DataConfig).ToList();
            _service.WriteData(outPutFile, finalResult);

            //Assert
            Assert.That(finalResult.Count, Is.GreaterThan(0));
            Assert.IsTrue(File.Exists(outPutFile));
        }

        [Test]
        public void FinalResult_ShouldNotHaveDuplicateProducts()
        {
            //Arrange

            //Act
            var finalResult = _service.MergeProducts(CommonHelper.DataConfig).ToList();

            //Assert
            Assert.AreEqual(finalResult.Count, finalResult.GroupBy(m => m.SKU).Select(m => m.FirstOrDefault()).Count());
        }

        [Test]
        public void FinalResult_ShouldHaveSource()
        {
            //Arrange

            //Act
            var finalResult = _service.MergeProducts(CommonHelper.DataConfig).ToList();

            //Assert
            Assert.IsTrue(finalResult.TrueForAll(m => !string.IsNullOrWhiteSpace(m.Source)));
        }

        [Test]
        public void TestIfCompanyASourceIsActuallyFromRespectiveCompanyACatalogs()
        {
            //Arrange
            var companyAConfig = CommonHelper.DataConfig.CompanyFiles.Where(m => m.Source == SourceEnum.A.ToString()).FirstOrDefault();


            //Act
            var finalResult = _service.MergeProducts(CommonHelper.DataConfig).ToList();
            List<Catalog> companyACatalogs = _service.ReadData<Catalog>(Path.Combine(CommonHelper.DataConfig.InputFolderPath, companyAConfig.Catalog));
            var companyAProducts = finalResult.Where(m => m.Source == SourceEnum.A.ToString()).Select(m => m.SKU).Intersect(companyACatalogs.Select(n => n.SKU));

            //Assert
            Assert.AreEqual(finalResult.Count(m => m.Source == SourceEnum.A.ToString()), companyAProducts.Count());
        }

        [Test]
        public void TestIfCompanyBSourceIsActuallyFromRespectiveCompanyBCatalogs()
        {
            //Arrange
            var companyBConfig = CommonHelper.DataConfig.CompanyFiles.Where(m => m.Source == SourceEnum.B.ToString()).FirstOrDefault();


            //Act
            var finalResult = _service.MergeProducts(CommonHelper.DataConfig).ToList();
            List<Catalog> companyBCatalogs = _service.ReadData<Catalog>(Path.Combine(CommonHelper.DataConfig.InputFolderPath, companyBConfig.Catalog));
            var companyBProducts = finalResult.Where(m => m.Source == SourceEnum.B.ToString()).Select(m => m.SKU).Intersect(companyBCatalogs.Select(n => n.SKU));

            //Assert
            Assert.AreEqual(finalResult.Count(m => m.Source == SourceEnum.B.ToString()), companyBProducts.Count());
        }
    }
}