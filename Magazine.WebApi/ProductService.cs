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
        private Dictionary<int, Product> products;

        public ProductService(IConfiguration config)
        {
            PathProductsFile = config["DBPATH"];
            products = LoadProducts();
        }

        public Product Add(Product product)
        {
            try
            {
                product.Id = products.Any() ? products.Keys.Max() + 1 : 1;
                products[product.Id] = product; // Добавляем в словарь
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
                if (products.Remove(ID, out var product)) // Удаляем из словаря
                {
                    SaveProducts();
                    return product;
                }
                return null;
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
                if (products.ContainsKey(product.Id))
                {
                    products[product.Id] = product; // Обновляем словарь
                    SaveProducts();
                    return product;
                }
                return null;
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
                return products.TryGetValue(ID, out var product) ? product : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске продукта: {ex.Message}");
                return null;
            }
        }

        public List<Product> GetAll()
        {
            return products.Values.ToList();
        }


        private Dictionary<int, Product> LoadProducts()
        {
            if (!File.Exists(PathProductsFile)) return new Dictionary<int, Product>();

            try
            {
                var json = File.ReadAllText(PathProductsFile);
                var productList = JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
                return productList.ToDictionary(p => p.Id); // Преобразуем список в словарь
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки продуктов: {ex.Message}");
                return new Dictionary<int, Product>();
            }
        }

        private void SaveProducts()
        {
            try
            {
                var json = JsonConvert.SerializeObject(products.Values.ToList(), Formatting.Indented);
                File.WriteAllText(PathProductsFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения продуктов: {ex.Message}");
            }
        }
    }
}
