using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magazine.Core;

namespace Magazine.Core
{
    public interface IProductService
    {
        void Add(Product product);
        void Remove(int ID);
        void Edit(Product product);
        Product Search(int ID);
    }
}

