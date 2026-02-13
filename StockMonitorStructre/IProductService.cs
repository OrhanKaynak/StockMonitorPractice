using System;
using System.Collections.Generic;
using System.Text;

namespace StockMonitorStructre
{
    public interface IProductService
    {
        void AddProduct(Product product);
        void DeleteProduct(int id);
        List<Product> GetAllProducts();
        List<Product> GetCriticalStock();
        int GetCategoryIdByName(string categoryName);
    }
}
