using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Dtos;
using Warehouse.Core.IRepositories;
using Warehouse.DAL;

namespace Warehouse.BLL
{
    public class MonthlyBalanceReportService : IMonthlyBalanceReportService
    {
        private IDailyBalanceReportRepository _dailyReportRepository;
        private IMonthlyBalanceReportRepository _monthlyReportRepository;
        private IBlankRepository _blankRepository;
        private IWarehousRepository _warehousRepository;

        public MonthlyBalanceReportService(
        IDailyBalanceReportRepository dailyReportRepository,
        IMonthlyBalanceReportRepository monthlyReportRepository,
        IBlankRepository blankRepository,
        IWarehousRepository warehousRepository)
        {
            _dailyReportRepository = dailyReportRepository;
            _monthlyReportRepository = monthlyReportRepository;
            _blankRepository = blankRepository;
            _warehousRepository = warehousRepository;
        }
        public async Task GenerateMonthlyReportAsync(DateTime date)
        {
            // Определяем начало и конец месяца
            var startDate = new DateTime(date.Year, date.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);


            var warehouses = _warehousRepository.GetAll().Select(x => x.Id).ToList();

            foreach (var warehouseId in warehouses)
            {
                await GenerateWarehouseMonthlyReportAsync(warehouseId, startDate, endDate);
            }
        }
        private async Task GenerateWarehouseMonthlyReportAsync(int warehouseId, DateTime startDate, DateTime endDate)
        {

            var allBlanks = _blankRepository.GetAll().Where(w => w.IdWarehouse == warehouseId).ToList();

            var monthlyReportEntries = new List<MonthlyBalanceDto>();

            foreach (var blank in allBlanks)
            {
                // Получаем все ежедневные отчеты за месяц для данной заготовки
                var monthlyDailyReports = await _dailyReportRepository.GetByDateRangeAsync(startDate, endDate, warehouseId, blank.Id);

                if (monthlyDailyReports.Any())
                {
                    // Первый отчет месяца - для OpeningBalance
                    var firstDayReport = monthlyDailyReports
                        .OrderBy(r => r.Date)
                        .First();

                    // Последний отчет месяца - для ClosingBalance
                    var lastDayReport = monthlyDailyReports
                        .OrderBy(r => r.Date)
                        .Last();

                    // Суммируем показатели за месяц
                    var monthlyBalance = new MonthlyBalanceDto
                    {
                        Date = endDate, // Последний день месяца
                        WarehouseId = warehouseId,
                        BlankId = blank.Id,
                        OpeningBalance = firstDayReport.OpeningBalance,
                        TotalReceipts = monthlyDailyReports.Sum(r => r.TotalReceipts),
                        TotalIssues = monthlyDailyReports.Sum(r => r.TotalIssues),
                        DefectiveCount = monthlyDailyReports.Sum(r => r.DefectiveCount),
                        ClosingBalance = lastDayReport.ClosingBalance
                    };

                    monthlyReportEntries.Add(monthlyBalance);
                }
            }

            // Сохраняем месячный отчет
            if (monthlyReportEntries.Any())
            {
                await _monthlyReportRepository.SaveMonthlyReportAsync(monthlyReportEntries);
            }
        }
        public async Task<List<MonthlyBalanceDto>> GetMonthlyReportAsync(DateTime date, int warehouseId)
        {
            // Определяем конец месяца для поиска
            var endDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

            return await _monthlyReportRepository.GetByDateAsync(endDate, warehouseId);
        }
        public async Task<List<MonthlyBalanceDto>> GetMonthlyReportByPeriodAsync(DateTime startDate, DateTime endDate, int warehouseId)
        {
            return await _monthlyReportRepository.GetByDateRangeAsync(startDate, endDate, warehouseId);
        }
    }
}
