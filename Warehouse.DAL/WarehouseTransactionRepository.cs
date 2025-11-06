using Microsoft.EntityFrameworkCore;
using Warehouse.Core;
using Warehouse.Core.Dtos;

namespace Warehouse.DAL
{
    public class WarehouseTransactionRepository : IWarehouseTransactionRepository
    {
        private DataContext _dataContext;

        public WarehouseTransactionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<WarehouseTransactionDTO> GetAll()
        {
            var result = _dataContext.Warehousetransaction.ToList();

            return result;
        }



        public async Task<WarehouseTransactionDTO> AddAsync(WarehouseTransactionDTO warehouseTransactionDTO)
        {

            _dataContext.Warehousetransaction.Add(warehouseTransactionDTO);


            await _dataContext.SaveChangesAsync();

            return warehouseTransactionDTO;
        }


        public List<WarehouseTransactionDTO> GetWarehousetransactionByDate(DateTime date)
        {


            var result = _dataContext.Warehousetransaction
                .Where(t => t.TransactionDate == date)
                .Select(t => new WarehouseTransactionDTO
                {
                    Id = t.Id,
                    TransactionDate = t.TransactionDate,
                    WarehouseId = t.WarehouseId,
                    BlankId = t.BlankId,
                    TransactionTypeId = t.TransactionTypeId,
                    Quantity = t.Quantity,
                    Reference = t.Reference,
                    CreatedBy = t.CreatedBy,
                    CreatedAt = t.CreatedAt,
                    IsCancelled = t.IsCancelled
                })
                .ToList();

            return result;
        }


        public async Task<WarehouseTransactionDTO> GetByIdAsync(int id)
        {
            return await _dataContext.Warehousetransaction.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateAsync(WarehouseTransactionDTO transaction)
        {
            _dataContext.Warehousetransaction.Update(transaction);
            await _dataContext.SaveChangesAsync();
        }

    }
}

