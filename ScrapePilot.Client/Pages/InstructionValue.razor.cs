using Microsoft.AspNetCore.Components;
using ScrapePilot.Client.Models;

namespace ScrapePilot.Client.Pages
{
    public partial class InstructionValue : ComponentBase
    {
        [Parameter]
        public MemberModel? prop { get; set; }
        protected override void OnParametersSet()
        {
            // This method is called whenever the component's parameters are set or changed
            // You can perform additional logic here when parameters change
            base.OnParametersSet();
        }
    }
}
