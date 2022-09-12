using BilgeShop.Business.Types;
using BilgeShop.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Services
{
    public interface ICategoryService
    {
        ServiceMessage AddCategory(CategoryDto category);
        List<CategoryDto> GetCategories();
        CategoryDto GetCategoryById(int id);
        void EditCategory(CategoryDto category);
        void DeleteCategory(int id);
    }
}
