using ExchangeRateProvider.Core.Models;

namespace ExchangeRateProvider.ViewModels
{
    public class ExchangeRateViewModel
    {
        public ExchangeRateViewModel()
        {
            ExchangeRate = new ExchangeRateModel();
        }
        public ExchangeRateModel? ExchangeRate { get; set; }
        
    }
}
