using System.Globalization;
using ExchangeRateProvider.Core.Apis;
using ExchangeRateProvider.Core.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProvider.Core.Interactors
{
    /// <summary>
    /// Interactor for handling the mapping of czech national bank exchange rate data to our exchange rate model.
    /// </summary>
    public class CzechNationalBankExchangeRateInteractor : IExchangeRateInteractor
    {
        private readonly ICzechNationalBankExchangeRateApi _exchangeRateApi;
        private readonly ILogger<CzechNationalBankExchangeRateInteractor> _logger;
        
        public CzechNationalBankExchangeRateInteractor(ICzechNationalBankExchangeRateApi exchangeRateApi,
            ILogger<CzechNationalBankExchangeRateInteractor> logger)
        {
            _exchangeRateApi = exchangeRateApi ?? throw new ArgumentNullException(nameof(exchangeRateApi));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ExchangeRateModel?> GetExchangeRates()
        {
            try
            {
                var result = await _exchangeRateApi.GetExchangeRates();
                return result == null
                    ? null
                    : new ExchangeRateModel
                    {
                        ConversionRateModel = MapConversionRates(result.Rates)
                    };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "There was a problem getting exchange rates");
                return null;
            }
        }

        private List<ConversionRateModel> MapConversionRates(List<ConversionRate> conversionRates)
        {
            return conversionRates.Select(rate => new ConversionRateModel
            {
                CurrencyCodeName = $"{rate.CurrencyCode} - {rate.Country.ToUpper()}",
                CurrencyRate = rate.Rate.ToString(CultureInfo.InvariantCulture)
            }).ToList();
        }
    }
}
