using CrossCutting.Model;
using CrossCutting.Platform;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetrievalCore
{
    public class LiveloProductRetriever : IPlatformProductRetriever
    {
        private const string PlatformName = "Livelo";
        private const string baseUrl = "http://www.pontoslivelo.com.br";
        private string searchUrl = baseUrl + "/browse?Ntt=";

        public IList<ProductRawData> Get(Product product)
        {
            var result = new List<ProductRawData>();

            var searchWebPage = new HtmlWeb();
            var searchDocument = searchWebPage.Load(searchUrl + product.Description);
            var searchResultNodes = searchDocument.DocumentNode.SelectNodes("//div[@class='productdiv']/a");
            if (searchResultNodes == null)
            {
                Console.WriteLine($"Produto {product.Sku} não encontrado");
                return null;// buscar apenas por códigos [a-zA-Z0-9].?[0-9][0-9]
            }
            else
            {
                foreach (var node in searchResultNodes)
                {


                    var url = node.Attributes["href"].DeEntitizeValue;


                    //productRaw.Sku = GetSkuFromUrl(productRaw.Url);
                    //productRaw.Vendor = GetVendorFromSKU(productRaw.Sku);
                    //productRaw.Platform = PlatformName;

                    //productRaw.CostPriceFrom = node.SelectSingleNode("//span[@class='block-from-price-value']").InnerText;
                    //productRaw.CostPrice = node.SelectSingleNode("//span[@class='item-main-pricing']").InnerText;

                    result.AddRange(LoadProductDetails(url));
                }
            }
            return result;
        }

        private List<ProductRawData> LoadProductDetails(string url)
        {
            var result = new List<ProductRawData>();
            // productRaw.Url = url;
            // $x("//div[contains(@class, 'pdpparceirosinfo')]")
            var detailWebPage = new HtmlWeb();
            var detailDocument = detailWebPage.Load(url);

            var nodes = detailDocument.DocumentNode.SelectNodes("//div[contains(@class, 'pdpparceirosinfo')]");
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var productRaw = new ProductRawData();
                    productRaw.Url = url;

                    productRaw.CostPriceInPoints = detailDocument.DocumentNode.SelectSingleNode("//span[@class='ptrsku-price')]").InnerText;
                    productRaw.CostPriceFrom = productRaw.CostPriceInPoints;
                    productRaw.CostPrice = productRaw.CostPriceInPoints;

                    var imageUrl = detailDocument.DocumentNode.SelectSingleNode("//span[@class='ptr-image')]/img").Attributes["src"].DeEntitizeValue;
                    if (imageUrl.EndsWith("casasbahia.png"))
                        productRaw.Vendor = "Casas Bahia";
                    else if (imageUrl.EndsWith("pontofrio.png"))
                        productRaw.Vendor = "Ponto Frio";
                    else if (imageUrl.EndsWith("extra.png"))
                        productRaw.Vendor = "Ponto Frio";

                    result.Add(productRaw);
                }
            }
            else
            {
                var productRaw = new ProductRawData();
                var priceNode = detailDocument.DocumentNode.SelectSingleNode("//span[@class='prodprice']");
                if (priceNode != null)
                {
                    productRaw.CostPriceInPoints = priceNode.InnerText;
                    productRaw.CostPriceFrom = productRaw.CostPriceInPoints;
                    productRaw.CostPrice = productRaw.CostPriceInPoints;
                    productRaw.Platform = PlatformName;
                    result.Add(productRaw);
                }
            }

            return result;
        }
    }
}
