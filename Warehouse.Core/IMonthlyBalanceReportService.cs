using Warehouse.Core.Dtos;

namespace Warehouse.BLL
{
    public interface IMonthlyBalanceReportService
    {
        Task GenerateMonthlyReportAsync(DateTime date);
        Task<List<MonthlyBalanceDto>> GetMonthlyReportAsync(DateTime date, int warehouseId);
        Task<List<MonthlyBalanceDto>> GetMonthlyReportByPeriodAsync(DateTime startDate, DateTime endDate, int warehouseId);
    }
}