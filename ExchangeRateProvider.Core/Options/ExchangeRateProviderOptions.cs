using System.ComponentModel.DataAnnotations;
using ExchangeRateProvider.Core.Models;

namespace ExchangeRateProvider.Core.Options
{
    public class ExchangeRateProviderOptions
    {
        [Required]
        public Currency? SourceCurrency { get; set; }
    }
}
