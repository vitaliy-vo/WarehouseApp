using Mapster;
using Warehouse.Core.OutPutModels;
using Warehouse.DAL;

namespace Warehouse.BLL
{
    public class WarehousServise
    {
        private IWarehousRepository _warehousRepository;

        public WarehousServise(IWarehousRepository warehousRepository)
        {
            _warehousRepository = warehousRepository;
        }

        public List<WarehouseOutPutModel> GetAll()
        {
            var tmp = _warehousRepository.GetAll();
            var result = tmp.Adapt<List<WarehouseOutPutModel>>();
            return result;
        }
    }
}
