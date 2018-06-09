using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.Model
{
    public class Product
    {
        public string Description { get; set; }
        public string Sku { get; set; }
        public string Brand { get; set; }
        public decimal? CostPriceFrom { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? CostPriceInPoints { get; set; }
        public decimal? Discount { get; set; }
        public string Vendor { get; set; }
        public string Url { get; set; }
    }
}
