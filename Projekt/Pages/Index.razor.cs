using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.Metrics;
using WMBlazorSlickCarousel.WMBSC;

namespace Projekt.Pages
{
    public partial class Index
    {

        protected override async Task OnInitializedAsync()
        {
        }

        [Inject] public IJSRuntime JSRuntime { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await JSRuntime.InvokeVoidAsync("Slice");
                await JSRuntime.InvokeVoidAsync("reveal2");
            }

        }

        private async Task OnScroll()
        {
        }



    }
}
