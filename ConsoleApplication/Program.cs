using CrossCutting.Model;
using CrossCutting.Platform;
using InputCore;
using OfficeOpenXml;
using RetrievalCore;
using System;
using System.Collections.Generic;
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

            IEnumerable<Product> products = null;

            var existingFile = new FileInfo(filePath);
            using (var package = new ExcelPackage(existingFile))
            {
                var worksheet = package.Workbook.Worksheets[1];
                var productListReader = new ProductListReader(worksheet);
                var productInputParser = new ProductInputParser();

                productListReader.LoadProductList();
                products = productListReader.GetData(productInputParser);

                Console.WriteLine($"{products.Count()} produtos encontrados");
            }

            var vendorRetrievers = new List<IPlatformProductRetriever>
            {
                //new MultiplusProductRetriever(),
                //new LiveloProductRetriever(),
                new SmilesProductRetriever()
            };

            if (products != null)
            {
                foreach (var product in products)
                {
                    foreach (var vendorRetriever in vendorRetrievers)
                    {
                        var productRaw = vendorRetriever.Get(product);
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
