using System.ComponentModel.DataAnnotations;
using ExchangeRateProvider.Core.Models;

namespace ExchangeRateProvider.Core.Options
{
    public class HomeControllerOptions
    {
        public HomeControllerOptions()
        {
            Currencies = new List<Currency>();
        }
        [Required, MinLength(1, ErrorMessage = "Currencies not populated")]
        public List<Currency> Currencies { get; set; }
    }
}
