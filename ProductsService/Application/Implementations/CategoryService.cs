using Application.Common;
using Application.Contracts;
using Application.Models.DTO.Category;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations
{
    public class CategoryService : ICategoryService
    {
        public Task<ServiceResult<int>> AddCategoyAsync(CategoryCreationDTO category)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<int>> DeleteCategoyAsync(Guid category_id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<List<CategoryDTO>>> GetAllCategoryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<CategoryDTO>> GetCategoryAsync(Guid category_id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<int>> UpdateCategoyAsync(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
