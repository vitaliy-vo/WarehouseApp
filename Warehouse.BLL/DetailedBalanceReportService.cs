using Warehouse.Core.Dtos;
using Warehouse.Core.IRepositories;
using Warehouse.DAL;

namespace Warehouse.BLL
{
    public class DetailedBalanceReportService : IDetailedBalanceReportService
    {
        private IDailyBalanceReportRepository _reportRepository;
        private IBlankRepository _blankRepository;
        private IWarehouseTransactionRepository _transactionRepository;
        private IWarehousRepository _warehousRepository;
        private IDailyBalanceReportService _dailyBalanceReportService;

        public DetailedBalanceReportService(
            IDailyBalanceReportRepository reportRepository,
            IBlankRepository blankRepository,
            IWarehouseTransactionRepository transactionRepository,
            IWarehousRepository warehousRepository,
            IDailyBalanceReportService dailyBalanceReportService)
        {
            _reportRepository = reportRepository;
            _blankRepository = blankRepository;
            _transactionRepository = transactionRepository;
            _warehousRepository = warehousRepository;
            _dailyBalanceReportService = dailyBalanceReportService;
        }


        public async Task<List<DetailedBalanceReportDto>> GetDetailedReportAsync(DateTime date, int warehouseId)
        {
            // Получаем базовый отчет
            var dailyReport = await _reportRepository.GetByDateAsync(date, warehouseId);
            var blanks = _blankRepository.GetBlanksByIdWarehouseId(warehouseId);

            // Получаем транзакции за день для детализации
            var transactions = _transactionRepository.GetWarehousetransactionByDate(date);

            var detailedReport = new List<DetailedBalanceReportDto>();

            foreach (var blank in blanks)
            {
                var dailyBalance = dailyReport.FirstOrDefault(r => r.BlankId == blank.Id);
                var blankTransactions = transactions.Where(t => t.BlankId == blank.Id && !t.IsCancelled).ToList();

                // Разбиваем операции по типам
                var receiptTransactions = blankTransactions.Where(t => t.TransactionTypeId == 1).ToList(); // Receipt
                var issueTransactions = blankTransactions.Where(t => t.TransactionTypeId == 2).ToList(); // IssueToProduction
                var transferTransactions = blankTransactions.Where(t => t.TransactionTypeId == 4).ToList(); // TransferToCentral
                var defectiveTransactions = blankTransactions.Where(t => t.TransactionTypeId == 3).ToList(); // DefectiveWriteOff

                // Рассчитываем суммы операций
                var receiptSum = receiptTransactions.Sum(t => t.Quantity);
                var issueSum = issueTransactions.Sum(t => t.Quantity);
                var transferSum = transferTransactions.Sum(t => t.Quantity);
                var defectiveSum = defectiveTransactions.Sum(t => t.Quantity);

               
                bool hasOperations = receiptSum != 0 || issueSum != 0 || transferSum != 0 || defectiveSum != 0;

                // Если есть операции, добавляем в отчет
                if (hasOperations)
                {

                    var item = new DetailedBalanceReportDto
                    {
                        BlankId = blank.Id,
                        BlankName = blank.NameValue,
                        OpeningBalance = dailyBalance?.OpeningBalance ?? 0,
                        ReceiptFromCentral = receiptTransactions.Sum(t => t.Quantity),
                        ReceiptReference = GetReferenceString(receiptTransactions),
                        ReturnDefective = 0, // Всегда 0
                        ReturnDefectiveReference = "—", // Всегда прочерк
                        IssuedToProduction = issueTransactions.Sum(t => t.Quantity),
                        IssueReference = GetReferenceString(issueTransactions),
                        TransferredToOtherWarehouse = transferTransactions.Sum(t => t.Quantity),
                        TransferReference = GetReferenceString(transferTransactions),
                        WrittenOff = defectiveTransactions.Sum(t => t.Quantity),
                        WriteOffReference = GetReferenceString(defectiveTransactions),
                        ClosingBalance = dailyBalance?.ClosingBalance ?? 0
                    };

                    detailedReport.Add(item);
                }
            }

            return detailedReport;
        }

        private string GetReferenceString(List<WarehouseTransactionDTO> transactions)
        {
            if (!transactions.Any()) return "—";

            var references = transactions.Select(t => t.Reference)
                                       .Where(r => !string.IsNullOrEmpty(r))
                                       .Distinct()
                                       .ToList();

            return references.Any() ? string.Join(", ", references) : "—";
        }

        public async Task GenerateDetailedReportAsync(DateTime date)
        {
            
            await _dailyBalanceReportService.GenerateDailyReportAsync(date);
        }
    }
}

