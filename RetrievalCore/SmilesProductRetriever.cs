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
    public class SmilesProductRetriever : IPlatformProductRetriever
    {
        private const string baseUrl = "https://www.shoppingsmiles.com.br/smiles/";
        private string searchUrl = baseUrl + "/super_busca.jsf?b=";

        public ProductRawData Get(Product product)
        {
            var productRaw = new ProductRawData();

            var searchWebPage = new HtmlWeb();
            //recover the description of the product from the inserted .xml
            //and insert as a param to the search index
            var searchDocument = searchWebPage.Load(searchUrl + product.Description); // SearchEncode(product.Description));
            var searchResultNodes = searchDocument.DocumentNode.SelectNodes("//*/span[@class='itens-section']");            
            if (searchResultNodes == null)
            {
                Console.WriteLine($"Produto {product.Sku} não encontrado");
            }
            else
            {
                foreach (var node in searchResultNodes)
                {
                    var urlNode = node.SelectSingleNode("a");

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

            productRaw.Description = detailDocument.DocumentNode.SelectSingleNode("//h1[@class='plain-name']").InnerText;

        }

        /*public void LoadProductPrice() {

        }*/

        private string SearchEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }
    }
}
