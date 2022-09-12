using BilgeShop.Business.Services;
using BilgeShop.Business.Types;
using BilgeShop.Data.Dto;
using BilgeShop.Data.Entities;
using BilgeShop.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Managers
{
    public class ProductManager : IProductService
    {
        private readonly IRepository<ProductEntity> _productRepository;

        public ProductManager(IRepository<ProductEntity> productRepository)
        {
            _productRepository = productRepository;
        }

        public ServiceMessage AddProduct(ProductDto product)
        {
            var hasProduct = _productRepository.GetAll(x => (x.Name).ToLower() == (product.Name).ToLower()).ToList();
            // TODO : tolower'da bug var. Çöz

            if(hasProduct.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu isimde bir ürün mevcut."
                };
            }

            var productEntity = new ProductEntity()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                UnitInStock = product.UnitInStock,
                UnitPrice = product.UnitPrice,
                ImagePath = product.ImagePath,
                CategoryId = product.CategoryId
            };

            _productRepository.Add(productEntity);

            return new ServiceMessage
            {
                IsSucceed = true
            };
        }

        public void DeleteProduct(int id)
        {
            _productRepository.Delete(id);
        }

        public void EditProduct(ProductDto product)
        {
            var productEntity = _productRepository.GetById(product.Id);

            productEntity.Name = product.Name;
            productEntity.Description = product.Description;
            productEntity.UnitPrice = product.UnitPrice;
            productEntity.UnitInStock = product.UnitInStock;
            productEntity.CategoryId = product.CategoryId;

            if(product.ImagePath != null)
            productEntity.ImagePath = product.ImagePath;

            _productRepository.Update(productEntity);
        }

        public ProductDto GetProductById(int id)
        {
            var productEntity = _productRepository.GetById(id);

            var productDto = new ProductDto
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Description = productEntity.Description,
                UnitInStock = productEntity.UnitInStock,
                UnitPrice = productEntity.UnitPrice,
                ImagePath = productEntity.ImagePath,
                CategoryId = productEntity.CategoryId,
            };

            return productDto;
        }

        public List<ProductDto> GetProducts(int? categoryId = null)
        {
            var query = _productRepository.GetAll();

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            var productEntities = query.OrderBy(x => x.Category.Name).ThenBy(x => x.Name);


            var productList = productEntities.Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UnitPrice = x.UnitPrice,
                UnitInStock = x.UnitInStock,
                CategoryId = x.CategoryId,
                ImagePath = x.ImagePath,
                CategoryName = x.Category.Name
            }).ToList();

            return productList;
        }
    }
}
