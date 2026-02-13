using System;
using System.Collections.Generic;
using System.Linq;

namespace StockMonitorStructre
{
    public enum StockStatus
    {
        OutOfStock = 0,
        Active = 1,
        Critical = 2,
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            IProductService productService = new SqlProductManager();

            string kategoriAdi = "Elektronik";

            int catId = productService.GetCategoryIdByName(kategoriAdi);
            if (catId > 0)
            {
                Console.WriteLine($"Kategori bulundu ID: {catId}");
            }

            productService.AddProduct(new Product
            {
                Id = 1,
                ProductName = "Laptop",
                Price = 25000,
                StockQuantity = 50,
                Status = StockStatus.Active
            });
            productService.AddProduct(new Product
            {
                Id= 2,
                ProductName = "Mouse",
                Price = 150,
                StockQuantity = 5,
                Status = StockStatus.Critical
            });

            productService.AddProduct(new Product
            {
                Id = 3,
                ProductName = "Kulaklık",
                Price = 1500,
                StockQuantity = 100,
                Status = StockStatus.Active
            });

            Console.WriteLine("\n--- Tüm Ürünler ---");

            foreach (var item in productService.GetAllProducts())
            {
                Console.WriteLine($"ID: {item.Id} | Ürün: {item.ProductName} | Stok: {item.StockQuantity} | Durum: {item.Status}");
            }

            Console.WriteLine("\n--- Kritik Stoktakiler (Metot Testi) ---");
            var criticals = productService.GetCriticalStock();
            foreach (var item in criticals)
            {
                Console.WriteLine($"Dikkat: {item.ProductName} tükenmek üzere! (Adet: {item.StockQuantity})");
            }
            Console.ReadLine();
        }
    }

    public class Product : BaseEntity
    {
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public StockStatus Status { get; set; }
    }

    public class ProductManager : IProductService
    {
        private List<Product> _products;

        public ProductManager()
        {
            _products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
            Console.WriteLine($"{product.ProductName} başarıyla eklendi.");
        }

        public void DeleteProduct(int id)
        {
            var productToDelete = _products.FirstOrDefault(p => p.Id == id);

            if (productToDelete != null)
            {
                _products.Remove(productToDelete);
                Console.WriteLine($"ID: {id} olan ürün silindi.");
            }
            else 
            {
                Console.WriteLine("Ürün bulunamadı!");
            }
        }

        public List<Product> GetAllProducts() 
        {
            return _products;
        }

        public List<Product> GetCriticalStock()
        {
            return _products.Where(p => p.StockQuantity < 10).ToList();
        }
        public int GetCategoryIdByName(string categoryName)
        {
            return 1;
        }
    }
}
