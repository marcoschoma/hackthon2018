using CrossCutting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.Input
{
    public interface IProductListReader
    {
        IEnumerable<Product> GetData(IProductInputParser productInputParser);
        void LoadProductList();

    }
}
