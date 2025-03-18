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

        public ProductService(IConfiguration config)
        {
            PathProductsFile = config["DBPATH"];
            products = LoadProducts();
        }

        public Product Add(Product product)
        {
            try
            {
                product.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1;
                products.Add(product);
                SaveProducts();
                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении продукта: {ex.Message}");
                return null;
            }
        }

        public Product Remove(int ID)
        {
            try
            {
                var product = products.FirstOrDefault(p => p.Id == ID);
                if (product != null)
                {
                    products.Remove(product);
                    SaveProducts();
                }
                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении продукта: {ex.Message}");
                return null;
            }
        }

        public Product Edit(Product product)
        {
            try
            {
                var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Definition = product.Definition;
                    existingProduct.Price = product.Price;
                    SaveProducts();
                }
                return existingProduct;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при редактировании продукта: {ex.Message}");
                return null;
            }
        }

        public Product Search(int ID)
        {
            try
            {
                return products.FirstOrDefault(p => p.Id == ID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске продукта: {ex.Message}");
                return null;
            }
        }

        private List<Product> LoadProducts()
        {
            if (!File.Exists(PathProductsFile)) return new List<Product>();

            try
            {
                var json = File.ReadAllText(PathProductsFile);
                return JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки продуктов: {ex.Message}");
                return new List<Product>();
            }
        }

        private void SaveProducts()
        {
            try
            {
                var json = JsonConvert.SerializeObject(products, Formatting.Indented);
                File.WriteAllText(PathProductsFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения продуктов: {ex.Message}");
            }
        }
    }
}
/*
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

        
        // Ддя корректной сериализации создаём файл, в котором массив JSON объектов
         
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
}*/
