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
            var product = new Product();
            product.Brand = productRaw.Brand;
            product.Vendor = productRaw.Vendor;
            product.Url = productRaw.Url;
            product.Sku = productRaw.Sku;
            product.Description = productRaw.Description;

            decimal discount;
            decimal costPriceInPoints;
            decimal costPriceFrom;
            decimal costPrice;

            if (decimal.TryParse(productRaw.Discount, out discount)) product.Discount = discount;
            if (decimal.TryParse(productRaw.CostPriceInPoints, out costPriceInPoints)) product.CostPriceInPoints = costPriceInPoints;
            if (decimal.TryParse(productRaw.CostPriceFrom, out costPriceFrom)) product.CostPriceFrom = costPriceFrom;
            if (decimal.TryParse(productRaw.CostPrice, out costPrice)) product.CostPrice = costPrice;

            //product.Discount = decimal.Parse(, NumberStyles.Any, CultureInfo.CurrentCulture);
            //product.CostPriceInPoints = decimal.Parse(productRaw.CostPriceInPoints, NumberStyles.Any, CultureInfo.CurrentCulture);
            //product.CostPriceFrom = decimal.Parse(productRaw.CostPriceFrom, NumberStyles.Any, CultureInfo.CurrentCulture);
            //product.CostPrice = decimal.Parse(productRaw.CostPrice, NumberStyles.Any, CultureInfo.CurrentCulture);
            return product;
        }
    }
}
