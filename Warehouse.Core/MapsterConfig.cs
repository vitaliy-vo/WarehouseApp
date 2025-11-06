using Mapster;
using Warehouse.Core.Dtos;
using Warehouse.Core.OutPutModels;

namespace Warehouse.Core
{
    public class MapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<BlankDto, BlankOutPutModel>()
                .Map(p=>p.NameValue, dto=> dto.NameValue.ToUpper());
        }
    }
}
