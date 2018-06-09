using CrossCutting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.Platform
{
    public interface IPlatformProductRetriever
    {
        ProductRawData Get(Product product);

    }
}
