using System.Diagnostics;
using ExchangeRateProvider.Core.ExchangeRateProviders;
using ExchangeRateProvider.Core.Options;
using ExchangeRateProvider.Models;
using ExchangeRateProvider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ExchangeRateProvider.Controllers
{
    public class HomeController : Controller
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly HomeControllerOptions _options;

        public HomeController(IExchangeRateProvider exchangeRateProvider, IOptions<HomeControllerOptions>? options)
        {
            _exchangeRateProvider = exchangeRateProvider ?? throw new ArgumentNullException(nameof(exchangeRateProvider));
            _options = options is null ? throw new ArgumentNullException(nameof(options)) : options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _exchangeRateProvider.GetExchangeRates(_options.Currencies);
            return View(new ExchangeRateViewModel { ExchangeRates = result });
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}