using System.Text.Json.Serialization;

namespace ExchangeRateProvider.Core.Models
{
    /// <summary>
    /// Czech national bank api response model
    /// </summary>
    public class CzechNationalBankExchangeRateApiResponse
    {
        public CzechNationalBankExchangeRateApiResponse()
        {
            Rates = new List<ConversionRate>();
        }

        [JsonPropertyName("rates")]
        public List<ConversionRate> Rates { get; set; }

    }
}


