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
        private const string PlatformName = "Smiles";
        private const string baseUrl = "https://www.shoppingsmiles.com.br/smiles/";
        private string searchUrl = baseUrl + "/super_busca.jsf?b=";

        public IList<ProductRawData> Get(Product product)
        {
            var result = new List<ProductRawData>();

            var searchWebPage = new HtmlWeb();
            var searchDocument = searchWebPage.Load(searchUrl + product.Description + "&a=false"); // SearchEncode(product.Description));
            var searchResultNodes = searchDocument.DocumentNode.SelectNodes("//*/span[@class='itens-section']/a");            
            if (searchResultNodes == null)
            {
                Console.WriteLine($"Produto {product.Sku} não encontrado");
                return null;// buscar apenas por códigos [a-zA-Z0-9].?[0-9][0-9]
            }
            else
            {
                foreach (var node in searchResultNodes)
                {
                    var productRaw = new ProductRawData();
                    var url = node.Attributes["href"].DeEntitizeValue;
                    productRaw.Url = baseUrl + url;

                    productRaw.Sku = GetSkuFromUrl(productRaw.Url);
                    productRaw.Vendor = GetVendorFromSKU(productRaw.Sku);
                    productRaw.Platform = PlatformName;

                    productRaw.CostPriceFrom = node.SelectSingleNode("//span[@class='block-from-price-value']").InnerText;
                    productRaw.CostPrice = node.SelectSingleNode("//span[@class='item-main-pricing']").InnerText;

                    result.Add(productRaw);
                }
            }
            return result;
        }

        private void LoadProductDetails(ProductRawData productRaw, string url)
        {
            productRaw.Url = url;
            var detailWebPage = new HtmlWeb();
            var detailDocument = detailWebPage.Load(productRaw.Url);

            var descriptionNode = detailDocument.DocumentNode.SelectSingleNode("//*[@id='produto:formProduto:produtoNome']");
            if (descriptionNode == null)
                return;

            productRaw.Description = descriptionNode?.InnerText;
            
            productRaw.CostPriceFrom = detailDocument.DocumentNode.SelectSingleNode("//span[contains(@class, 'produto-reference-price')]/strike").InnerText;
            productRaw.CostPrice = detailDocument.DocumentNode.SelectSingleNode("//span[contains(@class, 'produto-reference-price')]/span[@class='preco-reais-acumulo']").InnerText;
        }

        private string GetSkuFromUrl(string url)
        {
            var startIndex = url.IndexOf("&p=");

            var sku = url.Substring(startIndex+3);

            return sku.Substring(0, sku.IndexOf("&"));
        }

        private string GetVendorFromSKU(string sku)
        {
            switch (sku[sku.Length-1])
            {
                case '3':
                    return "Extra";
                case '4':
                    return "Ponto Frio";
                case '7':
                    return "Casas Bahia";
                default:
                    return null;
            }
        }
    }
}
