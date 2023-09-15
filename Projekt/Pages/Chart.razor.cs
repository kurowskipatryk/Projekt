using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Models;
using MudBlazor;
using NBPApi;
using System.Globalization;

namespace Projekt.Pages
{
    public partial class Chart
    {

        [Inject] INBPClient _nbpClient { get; set; }
        string type;
        private NBPTable NBPTable = new NBPTable();
        private LineChart lineChart = default!;
        private LineChartOptions lineChartOptions = default!;
        private ChartData chartData = default!;
        private DateRange _dateRange = new DateRange(DateTime.Now.Date.AddMonths(-1), DateTime.Now.Date);
        private MudDateRangePicker _picker;

        private MudSelect<string> _mudSelect;
        //protected override async Task OnInitializedAsync()
        //{
        //    await SelectedDate(_dateRange);
        //}
        public async Task GetTable()
        {
            NBPTable = await _nbpClient.GetCurrency(type);
        }

        private async Task Change(string selectedType)
        {
            if (chartData != null)
            {
                chartData.Datasets.Clear();
                chartData.Labels.Clear();
                await lineChart.UpdateAsync(chartData, lineChartOptions);
                numberOfCurr = 1;
                code = "";
                _mudSelect.Clear();
                _mudSelect.Text = "";
            }

            type = selectedType;
            await GetTable();
            if (NBPTable.Table != null && code != null)
            {
                await FillChart(code, 0);
            }
        }

        string code;
        private async Task ChangeCur(string selectedCode, int i)
        {
            code = selectedCode;
            await FillChart(code, i);
        }

        private string GetRandomColor()
        {
            Random rnd = new Random();

            var color1 = rnd.Next(250);
            var color2 = rnd.Next(250);
            var color3 = rnd.Next(250);

            return $"rgba({color1}, {color2}, {color3})";
        }
        private async Task FillChart(string code, int i)
        {
            var res = await _nbpClient.GetLastCurrencies(type, code, _dateRange.Start.Value, _dateRange.End.Value);


            List<double> bid = new List<double>();
            List<double> ask = new List<double>();
            List<double> mid = new List<double>();
            List<string> dates = new List<string>();

            foreach (var item in res)
            {
                bid.Add(item.Bid);
                ask.Add(item.Ask);
                mid.Add(item.Mid);
                dates.Add(item.EffectiveDate.ToString("yyyy-MM-dd"));
            }

            var colors = ColorBuilder.CategoricalTwelveColors;
            var datasets = new List<IChartDataset>();




            if (type == "C")
            {
                var rnd1 = GetRandomColor();
                var rnd2 = GetRandomColor();
                var dataset1 = new LineChartDataset
                {
                    Label = $"{code} BID",
                    Data = bid,
                    BackgroundColor = new List<string> { rnd1 },
                    BorderColor = new List<string> { rnd1 },
                    BorderWidth = new List<double> { 2 },
                    HoverBorderWidth = new List<double> { 4 },
                    PointBackgroundColor = new List<string> { rnd1 },
                    //PointRadius = new List<int> { 0 }, // hide points
                    PointHoverRadius = new List<int> { 4 }
                };
                datasets.Add(dataset1);

                var dataset2 = new LineChartDataset
                {
                    Label = $"{code} ASK",
                    Data = ask,
                    BackgroundColor = new List<string> { rnd2 },
                    BorderColor = new List<string> { rnd2 },
                    BorderWidth = new List<double> { 2 },
                    HoverBorderWidth = new List<double> { 4 },
                    PointBackgroundColor = new List<string> { rnd2 },
                    //PointRadius = new List<int> { 0 }, // hide points
                    PointHoverRadius = new List<int> { 4 }
                };
                datasets.Add(dataset2);
            }
            else
            {
                var rnd1 = GetRandomColor();
                var dataset1 = new LineChartDataset
                {
                    Label = $"{code} MID",
                    Data = mid,
                    BackgroundColor = new List<string> { rnd1 },
                    BorderColor = new List<string> { rnd1 },
                    BorderWidth = new List<double> { 2 },
                    HoverBorderWidth = new List<double> { 4 },
                    PointBackgroundColor = new List<string> { rnd1 },
                    //PointRadius = new List<int> { 0 }, // hide points
                    PointHoverRadius = new List<int> { 4 }
                };
                datasets.Add(dataset1);
            }

            if (i > 1 && chartData != null)
            {
                foreach (var item in datasets)
                {
                    chartData.Datasets.Add(item);
                }
            }
            else
            {

                chartData = new ChartData
                {
                    Labels = dates,
                    Datasets = datasets
                };
            }

            lineChartOptions = new();
            lineChartOptions.Responsive = true;
            lineChartOptions.Interaction = new Interaction { Mode = InteractionMode.Index };

            lineChartOptions.Scales.Y.Title.Text = "Price zł";
            lineChartOptions.Scales.Y.Title.Display = true;

            await lineChart.UpdateAsync(chartData, lineChartOptions);

            StateHasChanged();
        }


        private async Task SelectedDate(string value)
        {
            var a = value.Split(";");
            _dateRange.Start = DateTime.Parse(a[0].Substring(1));
            _dateRange.End = DateTime.Parse(a[1].Substring(0, a[1].Length - 1));
            if (NBPTable.Table != null && code != null)
            {
                await FillChart(code, 0);
            }
        }
        int numberOfCurr = 1;
        private async Task AddCur()
        {
            numberOfCurr++;
        }

        private async Task RemoveCur()
        {
            if (chartData != null)
            {

                if (type == "C")
                {
                    chartData.Datasets.RemoveAt(chartData.Datasets.Count - 1);
                    chartData.Datasets.RemoveAt(chartData.Datasets.Count - 1);
                }
                else
                {
                    chartData.Datasets.RemoveAt(chartData.Datasets.Count - 1);
                }
                numberOfCurr--;
                await lineChart.UpdateAsync(chartData, lineChartOptions);
            }
        }
    }
}
