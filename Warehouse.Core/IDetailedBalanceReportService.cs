using Warehouse.Core.Dtos;

namespace Warehouse.BLL
{
    public interface IDetailedBalanceReportService
    {
        Task GenerateDetailedReportAsync(DateTime date);
        Task<List<DetailedBalanceReportDto>> GetDetailedReportAsync(DateTime date, int warehouseId);
    }
}