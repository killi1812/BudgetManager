using Data.Models;

namespace Data.Services;

public interface ICategoryService
{
    Task<Category> Create(Category newCategory);
    Task<Category> Get(Guid guid);
    Task Delete(Guid guid);
    Task<Category> Create(Guid guid,Category newCategory);
    Task<List<Category>> GetAll();
}

public class CategoryService : ICategoryService
{
    public Task<Category> Create(Category newCategory)
    {
        throw new NotImplementedException();
    }

    public Task<Category> Get(Guid guid)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid guid)
    {
        throw new NotImplementedException();
    }

    public Task<Category> Create(Guid guid, Category newCategory)
    {
        throw new NotImplementedException();
    }

    public Task<List<Category>> GetAll()
    {
        throw new NotImplementedException();
    }
}