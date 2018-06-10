using CrossCutting.Model;
using CrossCutting.Platform;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace RetrievalCore
{
    public class MultiplusProductRetriever : IPlatformProductRetriever
    {
        private const string baseUrl = "http://www.pontosmultiplus.com.br/";
        private string searchUrl = baseUrl + "busca?Ntt=";

        public IList<ProductRawData> Get(Product product)
        {
            var result = new List<ProductRawData>();

            var searchWebPage = new HtmlWeb();
            var searchDocument = searchWebPage.Load(searchUrl + product.Description); // SearchEncode(product.Description));
            var searchResultNodes = searchDocument.DocumentNode.SelectNodes("//div[contains(@class, 'cartela produto')]");
            if (searchResultNodes == null)
            {
                Console.WriteLine($"Produto {product.Sku} não encontrado");
            }
            else
            {
                foreach (var node in searchResultNodes)
                {
                    var productRaw = new ProductRawData();
                    var urlNode = node.SelectSingleNode("//ul[contains(@class, 'lista-cartelas')]//a[not(@class)]");
                    productRaw.Url = baseUrl + urlNode.Attributes["href"].Value;

                    var getSkuRegex = new Regex("_/.*\\?");
                    var skuMatch = getSkuRegex.Match(productRaw.Url);
                    if (skuMatch.Value != null)
                    {
                        productRaw.Sku = (productRaw.Sku = skuMatch.Value.Substring(2)).Substring(0, productRaw.Sku.Length-1);
                    }

                    productRaw.CostPrice = node.SelectSingleNode("//p[@class='product-price']/strong")?.InnerText;
                    productRaw.CostPriceFrom = node.SelectSingleNode("//p[@class='product-price-from ']")?.InnerText;

                    if (!string.IsNullOrEmpty(productRaw.CostPriceFrom))
                    {
                        var getNumbersRegex = new Regex("\\d*\\.\\d*");
                        productRaw.CostPriceFrom = getNumbersRegex.Match(productRaw.CostPriceFrom).Value;
                    }
                    productRaw.Vendor = node.SelectSingleNode("//img[@class='logo-parceiro']")?.Attributes["alt"].DeEntitizeValue;

                    result.Add(productRaw);
                }
            }
            return result;
        }

        //private void LoadProductDetails(ProductRawData productRaw, string url)
        //{
        //    productRaw.Url = url;
        //    var detailWebPage = new HtmlWeb();
        //    var detailDocument = detailWebPage.Load(productRaw.Url);

        //    productRaw.Description = detailDocument.DocumentNode.SelectSingleNode("//h1[@class='nome-produto-nome']").InnerText;
        //    //productRaw.CostPrice = 
        //}

        private string SearchEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }
    }
}
