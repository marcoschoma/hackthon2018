using CrossCutting.Model;
using CrossCutting.Platform;
using Infra.DataAccess;
using InputCore;
using MongoDB.Driver;
using OfficeOpenXml;
using RetrievalCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            var filePath = "";
            if (args == null || args.Length == 0)
                filePath = "Lista de Produtos - Desafio.xlsx";
            else
                filePath = args[0];

            var mongoClient = new MongoClient(ConfigurationManager.ConnectionStrings["defaultCS"].ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase("MongoWebApp");

            var repository = new ProductRepository(mongoDatabase);
            var repositoryRaw = new ProductRawRepository(mongoDatabase);

            IEnumerable<Product> products = null;
            var productInputParser = new ProductInputParser();

            var existingFile = new FileInfo(filePath);
            using (var package = new ExcelPackage(existingFile))
            {
                var worksheet = package.Workbook.Worksheets[1];
                var productListReader = new ProductListReader(worksheet);

                productListReader.LoadProductList();
                products = productListReader.GetData(productInputParser);

                Console.WriteLine($"{products.Count()} produtos encontrados");
                foreach (var product in products)
                {
                    repository.Save(product);
                }
            }

            var vendorRetrievers = new List<IPlatformProductRetriever>
            {
                new MultiplusProductRetriever(),
                new LiveloProductRetriever(),
                new SmilesProductRetriever()
            };

            if (products != null)
            {
                foreach (var product in products)
                {
                    foreach (var vendorRetriever in vendorRetrievers)
                    {
                        var productRaw = vendorRetriever.Get(product);

                        repositoryRaw.Save(productRaw);
                        //repository.Save(productInputParser.GetProduct(productRaw));
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
