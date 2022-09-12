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
    public class CategoryManager : ICategoryService
    {
        private readonly IRepository<CategoryEntity> _categoryRepository;

        public CategoryManager(IRepository<CategoryEntity> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        

        public ServiceMessage AddCategory(CategoryDto category)
        {
            var hasCategory = _categoryRepository.GetAll(x => x.Name.ToLower() == category.Name.ToLower()).ToList();

            if(hasCategory.Any()) // hasCategory != null
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu isimde bir kategori zaten mevcut."
                };
            }

            var categoryEntity = new CategoryEntity
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            _categoryRepository.Add(categoryEntity);

            return new ServiceMessage
            {
                IsSucceed = true
            };
        }

        public void DeleteCategory(int id)
        {
           _categoryRepository.Delete(id);
        }

        public void EditCategory(CategoryDto category)
        {
            var categoryEntity = _categoryRepository.GetById(category.Id);

            categoryEntity.Name = category.Name;
            categoryEntity.Description = category.Description;

            _categoryRepository.Update(categoryEntity);
        }

        public List<CategoryDto> GetCategories()
        {
            var categoryEntities = _categoryRepository.GetAll().OrderBy(x => x.Name);

            var categoryList = categoryEntities.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList();

            return categoryList;
        }

        public CategoryDto GetCategoryById(int id)
        {
            var categoryEntity = _categoryRepository.GetById(id);

            var categoryDto = new CategoryDto
            {
                Id = categoryEntity.Id,
                Name = categoryEntity.Name,
                Description = categoryEntity.Description
            };

            return categoryDto;
        }
    }
}
