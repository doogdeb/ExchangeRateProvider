using ExchangeRateProvider.Core.Models;

namespace ExchangeRateProvider.ViewModels
{
    public class ExchangeRateViewModel
    {
        public ExchangeRateViewModel()
        {
            ExchangeRates = new List<ExchangeRate>();
        }
        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }
        
    }
}
