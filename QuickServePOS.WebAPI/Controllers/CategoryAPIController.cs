using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServePOS.Models.DTO.Common;
using QuickServePOS.Models.DTO.Menu;
using QuickServePOS.Models.Entities.Menu;
using QuickServePOS.Repositories.IUnitofWork;

namespace QuickServePOS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Owner")]
    public class CategoryAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryAPIController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            var data = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return Ok(data);
        }

        // GET: api/category/5
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    Message = "Category not found."
                });
            }

            var data = _mapper.Map<CategoryDto>(category);

            return Ok(data);
        }

        [HttpPost("Create-Category")]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var exists = await _unitOfWork.Categories
                .ExistsAsync(dto.Name);

            if (exists)
            {
                return BadRequest(new
                {
                    Message = "Category name already exists."
                });
            }

            var category = _mapper.Map<CategoryEntity>(dto);

            await _unitOfWork.Categories.AddAsync(category);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                Message = "Category created successfully."
            });
        }

        // PUT: api/category
        [HttpPut("Update-Category")]
        public async Task<IActionResult> Update(UpdateCategoryDto dto)
        {
            var category = await _unitOfWork.Categories
                .GetByIdAsync(dto.Id);

            if (category == null)
            {
                return NotFound(new
                {
                    Message = "Category not found."
                });
            }

            var exists = await _unitOfWork.Categories
                .ExistsAsync(dto.Name, dto.Id);

            if (exists)
            {
                return BadRequest(new
                {
                    Message = "Category name already exists."
                });
            }

            _mapper.Map(dto, category);

            category.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Categories.Update(category);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                Message = "Category updated successfully."
            });
        }

        [HttpDelete("SoftDelete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
          
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Category not found."
                });
            }
            
            if (category.MenuItems.Any(x => !x.IsDeleted))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Cannot delete category with active menu items."
                });
            }

            _unitOfWork.Categories.Delete(category);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Category deleted successfully."
            });
        }

        [HttpGet("TrashList")]
        public async Task<IActionResult> GetDeletedCategories()
        {
            var categories =await _unitOfWork.Categories.GetDeletedCategoriesAsync();

            var data =_mapper.Map<List<CategoryDto>>(categories);

            return Ok(data);
        }

        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var category =
                await _unitOfWork.Categories.GetByIdIgnoreQueryFilterAsync(id);

            if (category == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Category not found."
                });
            }

            category.IsDeleted = false;

            _unitOfWork.Categories.Update(category);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Category restored successfully."
            });
        }
    }
}
