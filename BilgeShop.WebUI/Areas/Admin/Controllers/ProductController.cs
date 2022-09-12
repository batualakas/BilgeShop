using BilgeShop.Business.Services;
using BilgeShop.Data.Dto;
using BilgeShop.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _environment;
        public ProductController(ICategoryService categoryService, IProductService productService, IWebHostEnvironment environment)
        {
            _categoryService = categoryService;
            _productService = productService;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var products = _productService.GetProducts();

            var viewModel = products.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UnitPrice = x.UnitPrice,
                UnitInStock = x.UnitInStock,
                CategoryId = x.CategoryId,
                ImagePath = x.ImagePath,
                CategoryName = x.CategoryName
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult New()
        {
            var categories = _categoryService.GetCategories();
            ViewBag.Categories = categories;

            return View("form", new ProductFormViewModel());
        }

        [HttpPost]
        public IActionResult Save(ProductFormViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                var categories = _categoryService.GetCategories();
                ViewBag.Categories = categories;
                return View("form", formData);
            }

            var allowedFileContentTypes = new string[] { "image/jpeg", "image/jpg", "image/png" , "image/jfif" };
            var allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png" ,".jfif"};

            var fileContentType = formData.File.ContentType;
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(formData.File.FileName);
            var fileExtension = Path.GetExtension(formData.File.FileName);

            // GÜVENLİK ÖNLEMİ
            if (formData.File == null || formData.File.Length == 0 || !allowedFileContentTypes.Contains(fileContentType) || !allowedFileExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("file", "Lütfen jpg , jpeg, jfif veya png tipinde geçerli bir dosya yükleyiniz.");
                var categories = _categoryService.GetCategories();
                ViewBag.Categories = categories;
                return View("Form", formData);

            }

            var newFileName = fileNameWithoutExtension + "_" + Guid.NewGuid() + fileExtension;
            var folderPath = Path.Combine("images", "products");
            var wwwRootFolderPath = Path.Combine(_environment.WebRootPath, folderPath);
            var wwwRootFilePath = Path.Combine(wwwRootFolderPath, newFileName);

            Directory.CreateDirectory(wwwRootFolderPath);

            using (var fileStream = new FileStream(wwwRootFilePath, FileMode.Create))
            {
                formData.File.CopyTo(fileStream);
            }
            // using bitiminde, içerisinde oluşturulan(newlenen) nesne , silinir.



            if (formData.Id == 0)
            {

                var productDto = new ProductDto()
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description,
                    ImagePath = newFileName,
                    UnitPrice = formData.UnitPrice,
                    UnitInStock = formData.UnitInStock,
                    CategoryId = formData.CategoryId
                };

                var response = _productService.AddProduct(productDto);

                if (response.IsSucceed)
                {
                    return RedirectToAction("index");
                }
                else
                {

                    var categories = _categoryService.GetCategories();
                    ViewBag.Categories = categories;

                    ViewBag.ErrorMessage = response.Message;
                    return View("form", formData);
                }
            }
            else
            {
                var productDto = new ProductDto()
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description,
                    UnitPrice = formData.UnitPrice,
                    UnitInStock = formData.UnitInStock,
                    CategoryId = formData.CategoryId
                };

                if (formData.File != null)
                    productDto.ImagePath = newFileName;

                _productService.EditProduct(productDto);
            }
            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            var categories = _categoryService.GetCategories();
            ViewBag.Categories = categories;

            var product = _productService.GetProductById(id);

            var viewModel = new ProductFormViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                UnitInStock = product.UnitInStock,
                CategoryId = product.CategoryId,
            };

            ViewBag.Image = product.ImagePath;

            return View("form", viewModel);
        }

        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);

            return RedirectToAction("index");
        }

    }
}
