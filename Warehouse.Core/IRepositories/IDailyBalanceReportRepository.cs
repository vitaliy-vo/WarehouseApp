using Warehouse.Core.Dtos;

namespace Warehouse.DAL
{
    public interface IDailyBalanceReportRepository
    {
        Task<List<DailyBalanceDto>> GetByDateAsync(DateTime date, int warehouseId);
        Task<DailyBalanceDto> GetLastReportBeforeDateAsync(int blankId, int warehouseId, DateTime date);
        Task SaveDailyReportAsync(List<DailyBalanceDto> reports);
    }
}