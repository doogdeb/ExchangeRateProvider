namespace ExchangeRateProvider.Core.Models
{
    /// <summary>
    /// Map api response to fields required by client.
    /// </summary>
    public class ConversionRateModel
    {
        public ConversionRateModel()
        {
            CurrencyCodeName = string.Empty;
            CurrencyRate = string.Empty;
        }

        public string CurrencyCodeName { get; set; }

        public string CurrencyRate { get; set; }
    }
}
