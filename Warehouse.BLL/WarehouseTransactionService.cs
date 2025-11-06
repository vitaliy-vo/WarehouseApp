using Mapster;
using Warehouse.Core.Dtos;
using Warehouse.Core.InputModels;
using Warehouse.Core.IRepositories;
using Warehouse.Core.OutPutModels;
using Warehouse.DAL;

namespace Warehouse.BLL
{
    public class WarehouseTransactionService
    {
        private IWarehouseTransactionRepository _warehouseTransactionRepository;
        private IWarehousRepository _warehousRepository;
        private ITransactionTypeRepository _transactionTypeRepository;
        private IBlankRepository _blankRepository;
        private IDailyBalanceReportService _balanceReportService;



        public WarehouseTransactionService(IWarehouseTransactionRepository warehouseTransactionRepository, IWarehousRepository warehousRepository,
            ITransactionTypeRepository transactionTypeRepository, IBlankRepository blankRepository, IDailyBalanceReportService balanceReportService)
        {
            _warehouseTransactionRepository = warehouseTransactionRepository;
            _warehousRepository = warehousRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _blankRepository = blankRepository;
            _balanceReportService = balanceReportService;
        }


        public async Task<WarehouseTransactionInputModel> CreateTransactionAsync(WarehouseTransactionInputModel model)
        {
            var tmp1 = model.Adapt<WarehouseTransactionDTO>();
            var tmp = await _warehouseTransactionRepository.AddAsync(tmp1); // Используем асинхронный вызов
            var result = tmp.Adapt<WarehouseTransactionInputModel>();
            return result;
        }

        public List<WarehouseTransactionInputModel> MapIssueToProduction(WriteOffModelInputModel writeOffModel)
        {
            return writeOffModel.Items.Select(item => new WarehouseTransactionInputModel
            {
                TransactionDate = writeOffModel.TransactionDate,
                WarehouseId = writeOffModel.WarehouseId,
                BlankId = item.BlankId,
                Quantity = -item.Quantity,
                TransactionTypeId = writeOffModel.TransactionTypeId,
                Reference = writeOffModel.Reference,
                CreatedBy = writeOffModel.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                IsCancelled = false
            }).ToList();
        }


        public List<WarehouseTransactionOutModel> GetWarehousetransactionByDate(DateTime date)
        {

            var result = (from transaction in _warehouseTransactionRepository.GetWarehousetransactionByDate(date)
                          join warehouse in _warehousRepository.GetAll() on transaction.WarehouseId equals warehouse.Id
                          join blank in _blankRepository.GetAll() on transaction.BlankId equals blank.Id
                          join transactionType in _transactionTypeRepository.GetAll() on transaction.TransactionTypeId equals transactionType.Id
                          select new WarehouseTransactionOutModel
                          {
                              Id = transaction.Id,
                              TransactionDate = transaction.TransactionDate,
                              WarehousName = warehouse.Name,
                              BlankName = blank.NameValue,
                              TransactionType = transactionType.Operation,
                              Quantity = transaction.Quantity,
                              Reference = transaction.Reference,
                              CreatedBy = transaction.CreatedBy,
                              CreatedAt = transaction.CreatedAt,
                              IsCancelled = transaction.IsCancelled
                          }).ToList();

            return result;
        }

        public async Task<bool> CancelTransactionAsync(int transactionId)
        {
            // 1. Получаем транзакцию через репозиторий
            var transaction = await _warehouseTransactionRepository.GetByIdAsync(transactionId);

            if (transaction == null || transaction.IsCancelled)
                return false;

            // 2. Проверяем бизнес-правила
            if (CanCancelTransaction(transaction)) return false;

            // 3. Выполняем отмену
            transaction.IsCancelled = true;
            await _warehouseTransactionRepository.UpdateAsync(transaction);

            await _balanceReportService.RecalculateBalancesAfterTransactionAsync(
            transaction.WarehouseId,
            transaction.TransactionDate
        );


            return true;
        }

        private bool CanCancelTransaction(WarehouseTransactionDTO transaction)
        {
            // Проверяем, не отменена ли уже
            if (transaction.IsCancelled)
                return false;

            // Проверяем возраст транзакции
            if (transaction.TransactionDate < DateTime.Now.AddDays(-30))
                return false;

            return false;
        }

        public async Task<WarehouseTransactionInputModel> GetTransactionInputModelAsync(WarehouseTransactionOutModel model)
        {
            var entity = await _warehouseTransactionRepository.GetByIdAsync(model.Id);
            var result = entity.Adapt<WarehouseTransactionInputModel>();
            return result;
        }



        public WarehouseTransactionInputModel RecalculateBalances(WarehouseTransactionInputModel model)
        {
            OperationType currentOperation = (OperationType)model.TransactionTypeId;

            int reverseId = (int)GetCancelOperationId(currentOperation);

            model.TransactionTypeId = reverseId;

            model.Quantity = -1 * model.Quantity;

            model.Id = 0;
            model.IsCancelled = false;

            return model;
        }

        public OperationType GetCancelOperationId(OperationType operation)
        {
            return operation switch
            {
                OperationType.Replenishment => OperationType.DefectReplenishment,
                OperationType.WriteOff => OperationType.DefectWriteOff,
                OperationType.DefectiveWriteOff => OperationType.DefectDefectiveWriteOff,
                OperationType.SendToCentral => OperationType.DefectSendToCentral,
                _ => throw new ArgumentException("Неизвестный тип операции")
            };
        }



    }
}
