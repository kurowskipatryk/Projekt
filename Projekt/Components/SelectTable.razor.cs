using Microsoft.AspNetCore.Components;

namespace Projekt.Components
{
    public partial class SelectTable
    {
        [Parameter]
        public EventCallback<string> OnSelect { get; set; }
        string type;


        //string type = "A";
        private async Task Change(string selectedType)
        {
            type = selectedType;
            //await GetTable();
            OnSelect.InvokeAsync(selectedType);
        }

    }
}
