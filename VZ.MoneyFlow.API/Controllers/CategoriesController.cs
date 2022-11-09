using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using VZ.MoneyFlow.EFData.Exceptions;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.IData.IServices;
using VZ.MoneyFlow.Models.Models.Dtos.Requests;

namespace VZ.MoneyFlow.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class CategoriesController : BaseAuthController
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper, UserManager<AppUser> userManager) 
            : base(userManager)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();

            var categories = await _categoryService.GetAllWithChildrenAsync(userId);
            
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();

            var category = await _categoryService.GetByIdWithChildrenAsync(id, userId);
            if (category == null) throw new NotFoundException(nameof(GetById), id);
            if (category.UserId == userId) return Ok(category);
            return BadRequest("Provided data is invalid");
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestCreateCategoryDto categoryDto)
        {
            var userId = GetUserId();

            var newCategory = _mapper.Map<Category>(categoryDto);
            newCategory.UserId = userId;                        

            var categories = await _categoryService.GetAllWithChildrenAsync(userId);
            var parentCategory = categories.FirstOrDefault(c => c.UserId == userId && c.Id == newCategory.ParentCategoryId);

            await _categoryService.AddAsync(newCategory);

            if (parentCategory != null && newCategory.ParentCategoryId == parentCategory.Id)
            {
                parentCategory.ChildrenCategories.Add(newCategory);
                await _categoryService.UpdateAsync(parentCategory);
            }
            return Ok(newCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RequestUpdateCategoryDto categoryDto)
        {
            if (categoryDto == null) throw new NotFoundException(nameof(GetById), id);

            var userId = GetUserId();

            var categories = await _categoryService.GetAllWithChildrenAsync(userId);
            var oldCategory = categories.FirstOrDefault(c => c.Id == id && c.UserId == userId);
            var updatedCategory = _mapper.Map(categoryDto, oldCategory);

            await _categoryService.UpdateAsync(updatedCategory);
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var category = await _categoryService.GetByIdWithChildrenAsync(id, userId);

            if (category == null) throw new NotFoundException(nameof(GetById), id);
            if (category.UserId == userId)
            {
                await _categoryService.DeleteAsync(id);
                return Ok("Deleted successfully");
            }
            return BadRequest("Provided data is invalid");
        }
    }
}
