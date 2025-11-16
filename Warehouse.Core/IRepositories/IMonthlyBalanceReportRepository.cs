using Warehouse.Core.Dtos;

namespace Warehouse.BLL
{
    public interface IMonthlyBalanceReportRepository
    {
        Task DeleteMonthlyReportAsync(DateTime date, int warehouseId);
        Task<List<MonthlyBalanceDto>> GetByDateAsync(DateTime date, int warehouseId);
        Task<List<MonthlyBalanceDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int warehouseId);
        Task SaveMonthlyReportAsync(List<MonthlyBalanceDto> reports);
    }
}