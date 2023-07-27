using System.Diagnostics;
using ExchangeRateProvider.Core.Interactors;
using ExchangeRateProvider.Models;
using ExchangeRateProvider.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateProvider.Controllers
{
    public class HomeController : Controller
    {
        private readonly IExchangeRateInteractor _exchangeRateInteractor;
        
        public HomeController(IExchangeRateInteractor exchangeRateInteractor)
        {
            _exchangeRateInteractor = exchangeRateInteractor ?? throw new ArgumentNullException(nameof(exchangeRateInteractor));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _exchangeRateInteractor.GetExchangeRates();
            return View(new ExchangeRateViewModel { ExchangeRate = result });
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}