using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magazine.Core;

namespace Magazine.Core
{
    public class Product
    {
        public int Id { get; set; }
        public string Definition { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        /*
         * Сделать потом добавление изображения карточки товара
         * если прикручю фронтенд
        public void AddImages() { }
        */
        //Метод для удобного вывода из json
        public override string ToString()
        {
            return $"ID: {Id}, Definition: {Definition}, Name: {Name}, Price: {Price}";
        }
    }
}


