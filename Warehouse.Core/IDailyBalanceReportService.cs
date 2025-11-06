using Warehouse.Core.Dtos;

namespace Warehouse.BLL
{
    public interface IDailyBalanceReportService
    {
        Task GenerateDailyReportAsync(DateTime date);
        Task<List<DailyBalanceDto>> GetDailyReportAsync(DateTime date, int warehouseId);
        Task RecalculateBalancesAfterTransactionAsync(int warehouseId, DateTime transactionDate);
        Task UpdateBlanksCountFromReportAsync(DateTime date);
    }
}