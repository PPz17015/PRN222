using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class ProductDAO
    {
        public static List<Product> GetProducts()
        {
            var listProducts = new List<Product>();
            try
            {
                using var db = new MyStoreContext();
                listProducts = db.Products.Include(f => f.Category).ToList();
            }
            catch(Exception e) 
            {
                throw new Exception($"Error getting products: {e.Message}", e);
            }
            return listProducts;
        }

        public static void SaveProduct(Product p)
        {
            try
            {
                using var context = new MyStoreContext();
                context.Products.Add(p);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error saving product: {e.Message}", e);
            }
        }

        public static void UpdateProduct(Product p)
        {
            try
            {
                using var context = new MyStoreContext();
                var existingProduct = context.Products.Find(p.ProductId);
                if (existingProduct == null)
                {
                    throw new Exception($"Product with ID {p.ProductId} not found");
                }
                
                context.Entry(existingProduct).CurrentValues.SetValues(p);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error updating product: {e.Message}", e);
            }
        }

        public static void DeleteProduct(Product p)
        {
            try
            {
                using var context = new MyStoreContext();
                var product = context.Products.Find(p.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with ID {p.ProductId} not found");
                }
                
                context.Products.Remove(product);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting product: {e.Message}", e);
            }
        }

        public static Product GetProductById(int id)
        {
            try
            {
                using var db = new MyStoreContext();
                return db.Products.Include(p => p.Category).FirstOrDefault(c => c.ProductId == id);
            }
            catch (Exception e)
            {
                throw new Exception($"Error getting product by ID: {e.Message}", e);
            }
        }
    }
}
