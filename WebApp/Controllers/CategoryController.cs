using AutoMapper;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;
//[Authorize]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoryController(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Categories()
    {
       var categories = await _categoryService.GetAll();
       var vm = _mapper.Map<List<CategoryVM>>(categories);
       return View(vm);
   }

   public async Task<IActionResult> EditCategory(string guid)
   {
       var category = await _categoryService.Get(Guid.Parse(guid));
       var vm = _mapper.Map<CategoryVM>(category);
       return View(vm);
   }

   public IActionResult CreateCategory()
   {
       return View();
   }

   public async Task<IActionResult> EditCategoryAction(CategoryVM newCate)
   {
       var newCategory = _mapper.Map<Category>(newCate);
       await _categoryService.Create(newCategory);
       return Redirect(nameof(Categories));
   }
}