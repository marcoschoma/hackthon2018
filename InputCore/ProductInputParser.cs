using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.Input;
using CrossCutting.Model;

namespace InputCore
{
    public class ProductInputParser : IProductInputParser
    {
        public Product GetProduct(ProductRawData productRaw)
        {
            return new Product()
            {
                Brand = productRaw.Brand,
                Vendor = productRaw.Vendor,
                Url = productRaw.Url,
                Sku = productRaw.Sku,
                Discount = decimal.Parse(productRaw.Discount, NumberStyles.Any, CultureInfo.CurrentCulture),
                Description = productRaw.Description,
                CostPriceInPoints = decimal.Parse(productRaw.CostPriceInPoints, NumberStyles.Any, CultureInfo.CurrentCulture),
                CostPriceFrom = decimal.Parse(productRaw.CostPriceFrom, NumberStyles.Any, CultureInfo.CurrentCulture),
                CostPrice = decimal.Parse(productRaw.CostPrice, NumberStyles.Any, CultureInfo.CurrentCulture),
            };
        }
    }
}
