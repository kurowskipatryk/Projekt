using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Projekt.Pages
{
    public partial class Index
    {
        public WMBSCInitialSettings configurations;


        protected override async Task OnInitializedAsync()
        {
        }

        [Inject] public IJSRuntime JSRuntime { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await JSRuntime.InvokeVoidAsync("Slice");
            }
        }
    }
}
