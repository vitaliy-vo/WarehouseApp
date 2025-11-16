using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core;
using Warehouse.Core.Dtos;

namespace Warehouse.BLL
{
    public class MonthlyBalanceReportRepository : IMonthlyBalanceReportRepository
    {
        private DataContext _dataContext;

        public MonthlyBalanceReportRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<MonthlyBalanceDto>> GetByDateAsync(DateTime date, int warehouseId)
        {
            return await _dataContext.MonthlyBalanceReport
                .Where(r => r.Date == date && r.WarehouseId == warehouseId)
                .ToListAsync();
        }
        public async Task<List<MonthlyBalanceDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int warehouseId)
        {
            return await _dataContext.MonthlyBalanceReport
                .Where(r => r.Date >= startDate && r.Date <= endDate && r.WarehouseId == warehouseId)
                .ToListAsync();
        }
        public async Task SaveMonthlyReportAsync(List<MonthlyBalanceDto> reports)
        {
            if (!reports.Any()) return;

            var date = reports.First().Date;
            var warehouseId = reports.First().WarehouseId;

            // Удаляем старый отчет за этот месяц
            await DeleteMonthlyReportAsync(date, warehouseId);

            // Сохраняем новый отчет
            await _dataContext.MonthlyBalanceReport.AddRangeAsync(reports);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteMonthlyReportAsync(DateTime date, int warehouseId)
        {
            var existingReports = await GetByDateAsync(date, warehouseId);
            if (existingReports.Any())
            {
                _dataContext.MonthlyBalanceReport.RemoveRange(existingReports);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}
