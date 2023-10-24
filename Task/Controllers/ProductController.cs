using AutoMapper;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductTask.Models;

namespace ProductTask.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ProductController(IProductRepository productRepository , IMapper mapper , UserManager<ApplicationUser> userManager)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public IActionResult Index(int CategoryId = 0)
        {
            List<Product> products;
            List<ProductViewModel> productsViewModel;
            ViewBag.Categories = productRepository.GetCategories();
            if (CategoryId == 0)
                products = productRepository.GetAll();
            else
                products = productRepository.GetProductsByCategory(CategoryId);
            productsViewModel = mapper.Map<List<ProductViewModel>>(products);
            return View(productsViewModel);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Categories = productRepository.GetCategories();
            return View(new ProductViewModel());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                productViewModel.CreationDate = DateTime.Now;
                var product = mapper.Map<Product>(productViewModel);
                product.CreatedByUserId = userManager.GetUserId(User);
                productRepository.Add(product);
                return RedirectToAction("Index");
            }
            return View(productViewModel);
        }
        public IActionResult Details(int id)
        {
            if (id == null)
                return NotFound();
            var product = productRepository.GetById(id);
            if (product is null)
                return NotFound();
            var productViewModel = mapper.Map<ProductViewModel>(product);
            ViewBag.CategoryName = productRepository.GetCategories()
                                   .Find(category => category.Id == product.CategoryId).Name;
            return View(productViewModel);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            if (id == null)
                return NotFound();
            var product = productRepository.GetById(id);
            if (product is null)
                return NotFound();
            var productViewModel = mapper.Map<ProductViewModel>(product);
            ViewBag.Categories = productRepository.GetCategories();
            return View(productViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Update(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var product = mapper.Map<Product>(productViewModel);
                productRepository.Update(product);
                return RedirectToAction("Index");
            }
            return View(productViewModel);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            if (id == null)
                return NotFound();
            var product = productRepository.GetById(id);
            if(product is null)
                return NotFound();
            try
            {
                productRepository.Delete(product);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
