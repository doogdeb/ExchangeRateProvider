using ExchangeRateProvider.Core.Apis;
using ExchangeRateProvider.Core.ExchangeRateProviders;
using ExchangeRateProvider.Core.Models;
using ExchangeRateProvider.Core.Options;
using ExchangeRateProvider.Infrastructure.Apis;

namespace ExchangeRateProvider
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
               
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            builder.Services.AddHttpClient<ICzechNationalBankExchangeRateApi, CzechNationalBankExchangeRateApi>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration["ExchangeRateApi:BaseAddress"]);
            });

            builder.Services.AddOptions<CzechNationalBankExchangeRateApiOptions>()
                .Configure(opts =>
                {
                    opts.RequestUri = builder.Configuration["ExchangeRateApi:RequestUri"];
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddOptions<HomeControllerOptions>()
                .Configure(opts =>
                {
                    var currencies = builder.Configuration.GetSection("Currencies").Get<List<string>>();
                    opts.Currencies = currencies.Select(s => new Currency(s)).ToList();
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddOptions<ExchangeRateProviderOptions>()
                .Configure(opts =>
                {
                    opts.SourceCurrency = new Currency(builder.Configuration["SourceCurrency"]);
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddTransient<IExchangeRateProvider, Core.ExchangeRateProviders.ExchangeRateProvider>();
           
            var app = builder.Build();

            // Confige the HTTP request pipeline.
            if(!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}