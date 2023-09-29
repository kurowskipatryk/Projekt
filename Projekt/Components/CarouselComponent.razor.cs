using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Projekt.Components
{
    public partial class CarouselComponent
    {

        [Inject] public IJSRuntime JSRuntime { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("Slice2");
            }

        }
    }
}
