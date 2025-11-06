using Warehouse.Core.Dtos;

namespace Warehouse.DAL
{
    public interface ITransactionTypeRepository
    {
        List<TransactionTypeDto> GetAll();
    }
}