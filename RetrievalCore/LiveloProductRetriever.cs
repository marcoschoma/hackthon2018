using CrossCutting.Model;
using CrossCutting.Platform;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RetrievalCore
{
    public class LiveloProductRetriever : IPlatformProductRetriever
    {
        private const string baseUrl = "http://www.pontoslivelo.com.br/";
        private string searchUrl = baseUrl + "browse?Ntt=";
        private const string descUrl = "http://www.pontoslivelo.com.br/livelo/produto/";

        public ProductRawData Get(Product product)
        {
            var productRaw = new ProductRawData();

            var searchWebPage = new HtmlWeb();
            var searchDocument = searchWebPage.Load(searchUrl + SearchEncode(product.Description));
            var searchResultNodes = searchDocument.DocumentNode.SelectNodes("//div[contains(@class, 'clpfeatureddesc')]/div[@class='productdiv']");

            if(searchResultNodes == null)
            {
                Console.WriteLine("Produto não encontrado");
            }
            else
            {

                foreach (var node in searchResultNodes)
                {
                    var urlNode = node.SelectSingleNode("a");
                
                    productRaw.Url = urlNode.Attributes["href"].Value;

                    Console.WriteLine(productRaw.Url);
                    //var detailWebPage = new HtmlWeb();
                    //var detailDocument = detailWebPage.Load(productRaw.Url);

                    var searchWebPageDesc = new HtmlWeb();
                    var searchDocumentDesc = searchWebPageDesc.Load(urlNode.Attributes["href"].Value);
                    var searchResultDesc = searchDocument.DocumentNode.SelectNodes("//*[contains(@id, 'itemproddesc')");
                    var urlNodeDesc = node.SelectSingleNode("h1");
                    productRaw.Url = urlNode.Attributes["h1"].Value;

                    Console.WriteLine(productRaw.Url);
                }   
            }
            return productRaw;
        }

        private string SearchEncode(string str)
        {
            return HttpUtility.UrlEncode(str.Replace(' ', '+'));
        }
    }
}
