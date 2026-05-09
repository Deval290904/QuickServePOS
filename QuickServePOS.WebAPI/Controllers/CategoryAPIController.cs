using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            var data = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return Ok(data);
        }

        // GET: api/category/5
        [HttpGet("{id}")]
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

        [HttpPost]
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
        [HttpPut]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.Categories
                .GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new
                {
                    Message = "Category not found."
                });
            }

            _unitOfWork.Categories.Delete(category);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new
            {
                Message = "Category deleted successfully."
            });
        }
    }
}
