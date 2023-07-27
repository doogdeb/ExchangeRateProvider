using System.ComponentModel.DataAnnotations;

namespace ExchangeRateProvider.Core.Options
{
    public class CzechNationalBankExchangeRateApiOptions
    {
        public CzechNationalBankExchangeRateApiOptions()
        {
            RequestUri = string.Empty;
        }

        [Required]
        public string RequestUri { get; set; }
    }
}
