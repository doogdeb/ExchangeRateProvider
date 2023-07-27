using System.ComponentModel.DataAnnotations;

namespace ExchangeRateProvider.Core.Options
{
    public class SupportedCurrenciesOptions
    {
        public SupportedCurrenciesOptions()
        {
            SupportedCurrencies = new List<string>();
        }

        [Required, MinLength(1, ErrorMessage = "At least one item required")]
        public List<string> SupportedCurrencies { get; set; }
    }
}
