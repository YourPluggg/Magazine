using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Magazine.Core;

namespace Magazine.WebApi
{
    public class ProductService : IProductService
    {
        private const string PathProductsFile = "products.txt"; 
        private List<Product> products;
    
        //Конструктор
        public ProductService() {
            products = LoadProducts(); 
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
         * Загрузка списка продуктов из файла 
         * Будем работать пока без JSON 
         * Данные об одном продукте хранятся в одной строке 
         * Ожидаем что формат Id;Name;Definition;Price
         */
        private List<Product> LoadProducts() {
            var products = new List<Product>();

            //Проверяем существует ли файл
            if (!File.Exists(PathProductsFile)) return products;

            var lines = File.ReadAllLines(PathProductsFile);

            foreach (var line in lines)
            {
                var parts = line.Split(';');
                if (parts.Length == 4)
                { 
                    /*
                     * Так как у нас два значения как int и double надо преобразовать их из строки
                     */
                    if (int.TryParse(parts[0], out int id) && double.TryParse(parts[3], out double price)) {
                        products.Add(new Product { 
                            Id = id,
                            Name = parts[1],
                            Definition = parts[2],
                            Price = price
                        });
                    }
                }
            }
            return products;
        }

        private void SaveProducts() {
            var lines = products.Select(p => $"{p.Id};{p.Name};{p.Definition};{p.Price}").ToList();
            File.WriteAllLines(PathProductsFile, lines);
        }
    }
}
