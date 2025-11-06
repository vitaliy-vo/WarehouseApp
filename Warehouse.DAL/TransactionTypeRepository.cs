using Warehouse.Core;
using Warehouse.Core.Dtos;

namespace Warehouse.DAL
{
    public class TransactionTypeRepository : ITransactionTypeRepository
    {
        private DataContext _dataContext;

        public TransactionTypeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<TransactionTypeDto> GetAll()
        {
            var result = _dataContext.TransactionType.OrderBy(x => x.Id).ToList();

            return result;
        }
    }
}
