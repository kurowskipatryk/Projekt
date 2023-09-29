using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using MudBlazor.Services;
using NBPApi;
using Projekt.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(opt => { opt.DetailedErrors = true; });
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddControllers();
builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");
builder.Services.AddScoped<INBPClient, NBPClient>();
//builder.Services.AddWMBSC();
builder.Services.AddBlazorBootstrap();

builder.Services.AddMudServices();
var app = builder.Build();

RequestLocalizationOptions GetLocalizationOptions()
{
    var cultures = builder.Configuration.GetSection("Cultures")
               .GetChildren().ToDictionary(x => x.Key, x => x.Value);

    var supportedCultures = cultures.Keys.ToArray();

    var localizationOptions = new RequestLocalizationOptions()
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);

    return localizationOptions;
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRequestLocalization(GetLocalizationOptions());

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
