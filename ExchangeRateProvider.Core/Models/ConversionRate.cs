using System.Text.Json.Serialization;

namespace ExchangeRateProvider.Core.Models
{
    /// <summary>
    /// Czech national bank api response model
    /// </summary>
    public class ConversionRate
    {
        public ConversionRate()
        {
            ValidFor = string.Empty;
            Country = string.Empty;
            Currency = string.Empty;
            CurrencyCode = string.Empty;
        }
        [JsonPropertyName("rates")]
        public string ValidFor { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("rate")]
        public float Rate { get; set; }
    }
}
