using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Magazine.Core;

namespace Magazine.WebApi
{
    public class ProductService : IProductService
    {
        private readonly string PathProductsFile; 
        private List<Product> products;
    
        //Конструктор
        public ProductService(IConfiguration cinfig) {
            products = LoadProducts();
            PathProductsFile = cinfig["DBPATH"];
        }


        public void Add(Product product) {
            product.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1;
            products.Add(product);
            SaveProducts();
        }

        public void Remove(int ID)
        {
            var product = products.FirstOrDefault(p => p.Id == ID); 
            if (product != null)
            {
                products.Remove(product);
                SaveProducts();
            }
        }

        public void Edit(Product product)
        {

        }

        public Product Search(int ID)
        {
            return products.FirstOrDefault(p => p.Id == ID);
        }

        /*
         * Ддя корректной сериализации создаём файл, в котором массив JSON объектов
         */
        private List<Product> LoadProducts() {
            
            var products = new List<Product>();

            //Проверяем существует ли файл
            if (!File.Exists(PathProductsFile)) return products;

            try
            {
                //Сериализация файла
                var json = File.ReadAllText(PathProductsFile);

                //Десериализация списка продуктов
                products = JsonConvert.DeserializeObject <List<Product>> (json);
            }
            catch 
            {
                Console.WriteLine($"Ошибка загрузки продуктов!");
            }
            
            return products;
        }

        private void SaveProducts() {
            try
            {
                //Сериализуем, только с отступами, чтобы соответствовать ожидаемому формату
                var json = JsonConvert.SerializeObject(products, Formatting.Indented);

                //Записываем JSON в файл
                File.WriteAllText(PathProductsFile, json);
            }
            catch 
            {
                Console.WriteLine($"Ошибка сохранения продуктов!");
            }
        }
    }
}
