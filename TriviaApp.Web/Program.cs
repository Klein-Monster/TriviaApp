using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TriviaApp.Adapter.Models;
using TriviaApp.Adapter.Services;
using TriviaApp.Domain.Services;
using TriviaApp.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<TriviaAdapterSettings>(builder.Configuration.GetSection("TriviaAdapter"));

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<ITriviaDataProvider, TriviaDataProvider>();
builder.Services.AddScoped<ITriviaDataProvider, TriviaDataProvider>();

builder.Services.AddSingleton<IQuestionsMemoryCache, QuestionsMemoryCache>();

builder.Services.AddScoped<ITriviaService, TriviaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
