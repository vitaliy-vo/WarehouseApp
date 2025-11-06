using Warehouse.Core.Dtos;

namespace Warehouse.Core.IRepositories
{
    public interface IBlankRepository
    {
        List<BlankDto> GetAll();
        List<BlankDto> GetBlanksByIdWarehouseId(int warehouseId);
        BlankDto AddBlank(BlankDto blank);
        Task<BlankDto> GetByIdAsync(int id);
        Task UpdateAsync(BlankDto blank);
        Task SaveChangesAsync();
    }
}
