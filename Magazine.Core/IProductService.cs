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
        Product Add(Product product);  // Теперь метод возвращает Product
        Product Remove(int ID);
        Product Edit(Product product);
        Product Search(int ID);
    }
}

