using CrossCutting.Input;
using CrossCutting.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputCore
{
    public class ProductListReader : IProductListReader
    {
        public ExcelWorksheet _worksheet;
        private List<ProductRawData> _productRawDataList = new List<ProductRawData>();

        public ProductListReader(ExcelWorksheet worksheet)
        {
            _worksheet = worksheet;
        }

        public IEnumerable<Product> GetData(IProductInputParser productInputParser)
        {
            var resultingProducts = new List<Product>();
            foreach (var item in _productRawDataList)
            {
                resultingProducts.Add(productInputParser.GetProduct(item));
            }
            return resultingProducts;
        }

        public void LoadProductList()
        {
            for (int row = 3; _worksheet.Cells[row, 1].Value != null; row++)
            {
                var productRawData = new ProductRawData();
                productRawData.Description = _worksheet.Cells[row, 1].Value.ToString();
                productRawData.Sku = _worksheet.Cells[row, 2].Value.ToString();
                productRawData.Category = _worksheet.Cells[row, 3].Value.ToString();
                productRawData.Brand = _worksheet.Cells[row, 4].Value.ToString();
                productRawData.CostPriceFrom = _worksheet.Cells[row, 5].Value.ToString();
                productRawData.CostPrice = _worksheet.Cells[row, 6].Value.ToString();
                productRawData.CostPriceInPoints = _worksheet.Cells[row, 7].Value.ToString();
                productRawData.Discount = _worksheet.Cells[row, 8].Value.ToString();
                productRawData.Vendor = _worksheet.Cells[row, 9].Value.ToString();
                productRawData.Url = _worksheet.Cells[row, 10].Value.ToString();
                _productRawDataList.Add(productRawData);
            }
        }
    }
}
