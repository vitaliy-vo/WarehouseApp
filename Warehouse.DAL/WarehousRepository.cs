using Warehouse.Core;
using Warehouse.Core.Dtos;

namespace Warehouse.DAL
{
    public class WarehousRepository : IWarehousRepository
    {
        private DataContext _dataContext;

        public WarehousRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<WarehouseDTO> GetAll()
        {
            var result = _dataContext.Warehouses.OrderBy(x => x.Id).ToList();
            return result;
        }
    }
}
