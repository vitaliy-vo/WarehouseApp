using Warehouse.Core.Dtos;

namespace Warehouse.DAL
{
    public interface IWarehousRepository
    {
        List<WarehouseDTO> GetAll();
    }
}