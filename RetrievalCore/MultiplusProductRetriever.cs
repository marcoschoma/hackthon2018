using CrossCutting.Model;
using CrossCutting.Platform;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RetrievalCore
{
    public class MultiplusProductRetriever : IPlatformProductRetriever
    {
        private const string baseUrl = "http://www.pontosmultiplus.com.br/";
        private string searchUrl = baseUrl + "/busca?Ntt=";

        public ProductRawData Get(Product product)
        {
            var productRaw = new ProductRawData();

            var searchWebPage = new HtmlWeb();
            var searchDocument = searchWebPage.Load(searchUrl + product.Description); // SearchEncode(product.Description));
            var searchResultNodes = searchDocument.DocumentNode.SelectNodes("//ul[contains(@class, 'lista-cartelas')]");
            if (searchResultNodes == null)
            {
                Console.WriteLine($"Produto {product.Sku} não encontrado");
            }
            else
            {
                foreach (var node in searchResultNodes)
                {
                    var urlNode = node.SelectSingleNode("//ul[contains(@class, 'lista-cartelas')]//a[not(@class)]");

                    LoadProductDetails(productRaw, baseUrl + urlNode.Attributes["href"].Value);

                    Console.WriteLine(productRaw.Url);
                }
            }
            return productRaw;
        }

        private void LoadProductDetails(ProductRawData productRaw, string url)
        {
            productRaw.Url = url;
            var detailWebPage = new HtmlWeb();
            var detailDocument = detailWebPage.Load(productRaw.Url);

            productRaw.Description = detailDocument.DocumentNode.SelectSingleNode("//h1[@class='nome-produto-nome']").InnerText;

        }

        private string SearchEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }
    }
}
