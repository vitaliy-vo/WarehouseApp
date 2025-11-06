using Microsoft.EntityFrameworkCore;
using Warehouse.Core;
using Warehouse.Core.Dtos;

namespace Warehouse.DAL
{
    public class DailyBalanceReportRepository : IDailyBalanceReportRepository
    {
        private DataContext _dataContext;

        public DailyBalanceReportRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<DailyBalanceDto>> GetByDateAsync(DateTime date, int warehouseId)
        {
            return await _dataContext.DailyBalanceReport
                .Where(r => r.Date == date.Date && r.WarehouseId == warehouseId)
                .ToListAsync();
        }

        public async Task SaveDailyReportAsync(List<DailyBalanceDto> reports)
        {
            if (!reports.Any()) return;

            var date = reports.First().Date;
            var warehouseId = reports.First().WarehouseId;

            // Удаляем старый отчет за эту дату
            var existingReports = await GetByDateAsync(date, warehouseId);
            if (existingReports.Any())
            {
                _dataContext.DailyBalanceReport.RemoveRange(existingReports);
            }

            await _dataContext.DailyBalanceReport.AddRangeAsync(reports);
            await _dataContext.SaveChangesAsync();
        }


        public async Task<DailyBalanceDto> GetLastReportBeforeDateAsync(int blankId, int warehouseId, DateTime date)
        {
            return await _dataContext.DailyBalanceReport
                .Where(r => r.BlankId == blankId && r.WarehouseId == warehouseId && r.Date < date)
                .OrderByDescending(r => r.Date)
                .FirstOrDefaultAsync();
        }
    }
}
