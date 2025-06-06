using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DemoMVC.Controllers
{
    public class ProductController : Controller
    {
        private List<Product> GetProducts()
        {
            var productsJson = TempData["Products"] as string;
            if (!string.IsNullOrEmpty(productsJson))
            {
                return JsonSerializer.Deserialize<List<Product>>(productsJson) ?? new List<Product>();
            }
            return new List<Product>();
        }
        private void SaveProducts(List<Product> products)
        {
            TempData["Products"] = JsonSerializer.Serialize(products);
        }
        public IActionResult Index()
        {
            var products = GetProducts();
            return View(products);
        }


        #region start

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var products = GetProducts();
                product.Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
                products.Add(product);
                SaveProducts(products);
                return RedirectToAction("Index");
            }
            return View(product);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var products = GetProducts();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var products = GetProducts();
                var existingProduct = products.FirstOrDefault(p => p.Id == product.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Price = product.Price;
                    existingProduct.Description = product.Description;
                    existingProduct.Category = product.Category;
                    SaveProducts(products);
                }
                return RedirectToAction("Index");
            }
            return View(product);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var products = GetProducts();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var products = GetProducts();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                products.Remove(product);
                SaveProducts(products);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var products = GetProducts();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        #endregion
    }
}
