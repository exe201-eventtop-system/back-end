using Application.Common;
using Application.Models.DTO.Category;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ICategoryService
    {
        public Task<ServiceResult<List<CategoryDTO>>> GetAllCategoryAsync();

        public Task<ServiceResult<CategoryDTO>> GetCategoryAsync(Guid category_id);

        public Task<ServiceResult<int>> AddCategoyAsync(CategoryCreationDTO category);

        public Task<ServiceResult<int>> UpdateCategoyAsync(Category category);

        public Task<ServiceResult<int>> DeleteCategoyAsync(Guid category_id);
    }
}
