using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DataAccessObjects;
using Services;

namespace ProductManagementMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _contextProduct;
        private readonly ICategoryService _contextCategory;

        public ProductsController(IProductService contextProduct, ICategoryService contextCategory)
        {
            _contextProduct = contextProduct;
            _contextCategory = contextCategory;
        }

        public IActionResult Index()
        {
            var products = _contextProduct.GetProducts();
            return View(products);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _contextProduct.GetProductById((int)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProductName,CategoryId,UnitStock,UnitPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _contextProduct.SaveProduct(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving product: " + ex.Message);
                }
            }
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _contextProduct.GetProductById((int)id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductId,ProductName,CategoryId,UnitStock,UnitPrice")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contextProduct.UpdateProduct(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error updating product: " + ex.Message);
                    }
                }
            }
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _contextProduct.GetProductById((int)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var product = _contextProduct.GetProductById(id);
                if (product != null)
                {
                    _contextProduct.DeleteProduct(product);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting product: " + ex.Message);
                return View();
            }
        }

        private bool ProductExists(int id)
        {
            return _contextProduct.GetProductById(id) != null;
        }
    }
}
