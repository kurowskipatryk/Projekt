using Microsoft.AspNetCore.Components;
using Models;

using NBPApi;

namespace Projekt.Pages
{
    public partial class TablesCurrency
    {
        [Inject] INBPClient _nbpClient { get; set; }
        string type;
        bool load;
        private NBPTable NBPTable = new NBPTable();

        public async Task GetTable()
        {
            load = true;
            NBPTable = await _nbpClient.GetCurrency(type);
            load = false;
        }

        private async Task Change(string selectedType)
        {
            type = selectedType;
            await GetTable();
        }

        private string searchString = "";
        private Func<Rate, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (x.Currency.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (x.Code.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (x.Mid.Equals(searchString))
                return true;

            return false;
        };
    }
}
