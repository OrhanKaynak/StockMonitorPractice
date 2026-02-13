using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace StockMonitorStructre
{
    public class SqlProductManager : IProductService
    {
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=StokTakipDB;Integrated Security=True";

        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlSorgusu = "SELECT * FROM Products";

                SqlCommand command = new SqlCommand(sqlSorgusu, connection);

                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Product p = new Product();

                        p.Id = Convert.ToInt32(reader["Id"]);
                        p.ProductName = reader["ProductName"].ToString();
                        p.Price = Convert.ToDecimal(reader["Price"]);
                        p.StockQuantity = Convert.ToInt32(reader["StockQuantity"]);
                        p.Status = (StockStatus)Convert.ToInt32(reader["Status"]);

                        products.Add(p);
                    }
                    reader.Close();
                }
                catch (Exception ex) 
                {
                    Console.WriteLine("Hata oluştu: " + ex.Message);
                }
            }
            return products;
        }

        public void AddProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlSorgusu = "INSERT INTO Products (CategoryId, ProductName, Price, StockQuantity, Status) " + "VALUES (@catId, @name, @price, @stock, @status)";

                SqlCommand command = new SqlCommand (sqlSorgusu, connection);

                command.Parameters.AddWithValue("@catId", product.CategoryId);
                command.Parameters.AddWithValue("@name", product.ProductName);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@stock", product.StockQuantity);
                command.Parameters.AddWithValue("@status", (int)product.Status);

                try
                {
                    connection.Open();
                    int etkilenenSatir = command.ExecuteNonQuery();

                    if (etkilenenSatir > 0)
                        Console.WriteLine("Ürün veritabanına başarıyla kaydedildi!");
                }
                catch (Exception ex) 
                {
                    Console.WriteLine("Kayıt Hatası!" + ex.Message);
                }
            }
        }

        public int GetCategoryIdByName(string categoryName)
        {
            int catId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Id FROM Categories Where categoryName = @name";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@name", categoryName);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        catId = (int)result;
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine("Kategori ID bulunurken hata: " + ex.Message);
                }
            }
            return catId;
        }
        public int CreateCategory(string categoryName)
        {
            int newId= 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Categories (CategoryName) VALUES (@name); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@name", categoryName);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        newId = Convert.ToInt32(result);
                        Console.WriteLine($"Yeni kategori veritabanına eklendi: {categoryName} (ID: {newId})");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Kategori oluşturma hatası: " + ex.Message);
                }
            }
            return newId;
        }

        public bool IsProductExist(string productName)
        {
            bool varMi = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM Products WHERE ProductName = @name";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@name", productName);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        varMi = true;
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine("Ürün kontrol hatası: " + ex.Message);
                }
            }
            return varMi;
        }
        public void DeleteProduct(int id) 
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM Products WHERE Id = @Id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    connection.Open();
                    int etkilenenSatir = command.ExecuteNonQuery();

                    if (etkilenenSatir > 0)
                    {
                        Console.WriteLine($"ID: {id} olan ürün başarıyla silindi.");
                    }
                    else
                    {
                        Console.WriteLine($"ID: {id} olan ürün bulunamadığı için silinemedi.");
                    }
                }
                catch(Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public List<Product> GetCriticalStock() 
        {
            List<Product> criticalProducts = new List<Product>();
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                string sql = "SELECT * FROM Products WHERE StockQuantity <= 10";
                SqlCommand command = new SqlCommand(sql, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Product p = new Product();
                        p.Id = Convert.ToInt32(reader["ID"]);
                        p.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                        p.ProductName = reader["ProductName"].ToString();
                        p.Price = Convert.ToDecimal(reader["Price"]);
                        p.StockQuantity = Convert.ToInt32(reader["StockQuantity"]);
                        p.Status = (StockStatus)Convert.ToInt32(reader["status"]);

                        criticalProducts.Add(p);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Kritik stok getirme hatası" + ex.Message);
                }
            }
            return criticalProducts;
        }
    }
}
