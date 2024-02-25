using Microsoft.AspNetCore.Components;

namespace ScrapePilot.Client.Pages
{
    public partial class Arguments : ComponentBase
    {
        [Parameter]
        public dynamic? Object { get; set; }

        [Parameter]
        public dynamic? ObjectType { get; set; }

        [Parameter]
        public Action StateAndResultJSONReload { get; set; }

        [Parameter]
        public SmartValuePicker SmartValuePicker { get; set; }
    }
}
