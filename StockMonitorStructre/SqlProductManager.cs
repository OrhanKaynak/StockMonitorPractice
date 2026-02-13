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

                command.Parameters.AddWithValue("@catId", 1);
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
        public void DeleteProduct(int id) { }
        public List<Product> GetCriticalStock() { return new List<Product>(); }
    }
}
