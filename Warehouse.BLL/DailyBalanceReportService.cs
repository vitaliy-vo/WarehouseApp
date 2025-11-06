using Warehouse.Core.Dtos;
using Warehouse.Core.IRepositories;
using Warehouse.DAL;

namespace Warehouse.BLL
{
    public class DailyBalanceReportService : IDailyBalanceReportService
    {
        private IDailyBalanceReportRepository _reportRepository;
        private IBlankRepository _blankRepository;
        private IWarehouseTransactionRepository _transactionRepository;
        private IWarehousRepository _warehousRepository;

        public DailyBalanceReportService(IDailyBalanceReportRepository reportRepository, IBlankRepository blankRepository, IWarehouseTransactionRepository transactionRepository, IWarehousRepository warehousRepository)
        {
            _reportRepository = reportRepository;
            _blankRepository = blankRepository;
            _transactionRepository = transactionRepository;
            _warehousRepository = warehousRepository;
        }

        public async Task GenerateDailyReportAsync(DateTime date)
        {
            // Получаем все склады
            var warehouses = new[] { 1, 2 }; // или из базы данных

            foreach (var warehouseId in warehouses)
            {
                await GenerateWarehouseDailyReportAsync(warehouseId, date);
            }
        }

        private async Task GenerateWarehouseDailyReportAsync(int warehouseId, DateTime date)
        {
            var allBlanks = _blankRepository.GetAll().Where(w => w.IdWarehouse == warehouseId).ToList();

            var previousDayReport = await _reportRepository.GetByDateAsync(date.AddDays(-1), warehouseId);

            var daysTransactions = _transactionRepository.GetWarehousetransactionByDate(date).Where(w => w.WarehouseId == warehouseId);


            var reportEntries = new List<DailyBalanceDto>();

            foreach (var blank in allBlanks)
            {

                var previousBalance = previousDayReport?
                    .FirstOrDefault(r => r.BlankId == blank.Id)?.ClosingBalance ?? 0;


                var blankTransactions = daysTransactions
                    .Where(t => t.BlankId == blank.Id && !t.IsCancelled)
                    .ToList();


                var totalReceipts = blankTransactions
                    .Where(t => t.TransactionTypeId == 1) // Receipt
                    .Sum(t => t.Quantity);

                var totalIssues = blankTransactions
                    .Where(t => t.TransactionTypeId == 2 || t.TransactionTypeId == 3) // IssueToProduction или TransferToCentral
                    .Sum(t => t.Quantity);

                var defectiveCount = blankTransactions
                    .Where(t => t.TransactionTypeId == 4) // DefectiveWriteOff
                    .Sum(t => t.Quantity);

                var closingBalance = previousBalance + totalReceipts + totalIssues + defectiveCount;

                var reportEntry = new DailyBalanceDto
                {
                    Date = date,
                    WarehouseId = warehouseId,
                    BlankId = blank.Id,
                    OpeningBalance = previousBalance,
                    TotalReceipts = totalReceipts,
                    TotalIssues = totalIssues,
                    DefectiveCount = defectiveCount,
                    ClosingBalance = closingBalance
                };

                reportEntries.Add(reportEntry);
            }


            await _reportRepository.SaveDailyReportAsync(reportEntries);
        }

        public async Task RecalculateBalancesAfterTransactionAsync(int warehouseId, DateTime transactionDate)
        {
            var currentDate = transactionDate.Date;
            var today = DateTime.Today;

            while (currentDate <= today)
            {
                await GenerateWarehouseDailyReportAsync(warehouseId, currentDate);
                currentDate = currentDate.AddDays(1);
            }
        }

        public async Task<List<DailyBalanceDto>> GetDailyReportAsync(DateTime date, int warehouseId)
        {
            return await _reportRepository.GetByDateAsync(date, warehouseId);
        }

        public async Task UpdateBlanksCountFromReportAsync(DateTime date)
        {

            var warehouses = _warehousRepository.GetAll().Select(w => w.Id).ToList();

            foreach (var warehouseId in warehouses)
            {
                var report = await _reportRepository.GetByDateAsync(date, warehouseId);

                foreach (var reportItem in report)
                {

                    var blank = await _blankRepository.GetByIdAsync(reportItem.BlankId);
                    if (blank != null)
                    {
                        blank.Count = reportItem.ClosingBalance;
                        await _blankRepository.UpdateAsync(blank);
                    }
                }
            }

            await _blankRepository.SaveChangesAsync();
        }
    }
}
