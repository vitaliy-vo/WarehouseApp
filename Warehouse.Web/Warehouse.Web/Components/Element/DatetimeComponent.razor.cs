using Microsoft.AspNetCore.Components;

namespace Warehouse.Web.Components.Element
{
    public class DatetimeComponentModel: ComponentBase
    {
        [Parameter]
        public DateTime DateTime { get; set; }

        [Parameter]
        public string DateTimeFormat { get; set; } = "dd.MM.yyyy";
    }
}
