using Mapster;
using Warehouse.Core.Dtos;
using Warehouse.Core.InputModels;
using Warehouse.Core.IRepositories;
using Warehouse.Core.OutPutModels;

namespace Warehouse.BLL
{
    public class BlankServise
    {
        private IBlankRepository _blankRepository;

        public BlankServise(IBlankRepository blankRepository)
        {
            _blankRepository = blankRepository;
        }

        public List<BlankOutPutModel> GetAll()
        {
            var tmp = _blankRepository.GetAll();
            var result = tmp.Adapt<List<BlankOutPutModel>>();
            return result;
        }

        public BlankOutPutModel Add(BlankInputModel model)
        {
            var tmp = _blankRepository.AddBlank(model.Adapt<BlankDto>());
            var result = tmp.Adapt<BlankOutPutModel>();
            return result;
        }

    }
}
