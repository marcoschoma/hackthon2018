using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.Model
{
    public class ProductRawData
    {
        //DESCRIÇÃO SKU CATEGORIA MARCA    PREÇO DE    PREÇO FINAL    PREÇO PONTO POR DESCONTO    BANDEIRA
        public string Description { get; set; }
        public string Sku { get; set; }
        public string Brand { get; set; }
        public string CostPriceFrom { get; set; }
        public string CostPrice { get; set; }
        public string CostPriceInPoints { get; set; }
        public string Discount { get; set; }
        public string Vendor { get; set; }
        public string Url { get; set; }

    }
}
