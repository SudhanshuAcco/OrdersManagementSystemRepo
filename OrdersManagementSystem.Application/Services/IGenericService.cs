namespace OrdersManagementSystem.Application.Services
{
    public interface IGenericService<TDto, TModel>
    {
        Task<TDto> GetByIdAsync(Guid id);
        Task<TDto> CreateAsync(TDto dto);
        Task UpdateAsync(Guid id, TDto model);
        Task DeleteAsync(Guid id);  
        Task<IEnumerable<TDto>> GetAllAsync();
    }
}
