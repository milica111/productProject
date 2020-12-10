using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.DAL.JsonAccess;
using Project.Entities;
using ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class JsonProductController : Controller
    {
        private readonly ILogger<JsonProductController> _logger;
        private readonly IProductService<IProductJsonAccess> _productService;

        public JsonProductController(ILogger<JsonProductController> logger, IProductService<IProductJsonAccess> productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductAsync();
            return View(products);
        }
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return View(product);
        }

        public async Task<IActionResult> Update(Product product)
        {
            await _productService.UpdateProductAsync(product);

            var products = await _productService.GetAllProductAsync();
            return View("Index", products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            await _productService.CreateProductAsync(product);
            var products = await _productService.GetAllProductAsync();
            return View("Index", products);
        }
    }
}
