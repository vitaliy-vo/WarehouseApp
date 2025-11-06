using Warehouse.Core.Dtos;

namespace Warehouse.DAL
{
    public interface IWarehouseTransactionRepository
    {
        Task<WarehouseTransactionDTO> AddAsync(WarehouseTransactionDTO warehouseTransactionDTO);
        List<WarehouseTransactionDTO> GetAll();
        List<WarehouseTransactionDTO> GetWarehousetransactionByDate(DateTime date);
        Task<WarehouseTransactionDTO> GetByIdAsync(int id);
        Task UpdateAsync(WarehouseTransactionDTO transaction);
    }
}