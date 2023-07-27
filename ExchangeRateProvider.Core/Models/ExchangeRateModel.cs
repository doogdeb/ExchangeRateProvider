namespace ExchangeRateProvider.Core.Models
{
    /// <summary>
    /// Map api response to <see cref="ExchangeRateModel"/>
    /// </summary>
    public class ExchangeRateModel
    {
        public ExchangeRateModel()
        {
            ConversionRateModel = new List<ConversionRateModel>();
        }
        public List<ConversionRateModel> ConversionRateModel { get; set; }
    }
}
