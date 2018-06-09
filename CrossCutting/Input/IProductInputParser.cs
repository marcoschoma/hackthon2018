﻿using CrossCutting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting.Input
{
    public interface IProductInputParser
    {
        Product GetProduct(ProductRawData productRaw);
    }
}
