using Mapster;
using Warehouse.Core.OutPutModels;
using Warehouse.DAL;

namespace Warehouse.BLL
{
    public class TransactionTypeService
    {
        private ITransactionTypeRepository _transactionTypeRepository;

        public TransactionTypeService(ITransactionTypeRepository transactionTypeRepository)
        {
            _transactionTypeRepository = transactionTypeRepository;
        }

        public List<TransactionTypeOutModel> GetAll()
        {
            var tmp = _transactionTypeRepository.GetAll();
            var result = tmp.Adapt<List<TransactionTypeOutModel>>();
            return result;
        }
    }
}
