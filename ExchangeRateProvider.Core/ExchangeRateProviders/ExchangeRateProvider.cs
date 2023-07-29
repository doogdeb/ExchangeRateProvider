using ExchangeRateProvider.Core.Apis;
using ExchangeRateProvider.Core.Models;
using ExchangeRateProvider.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateProvider.Core.ExchangeRateProviders
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICzechNationalBankExchangeRateApi _czechNationalBankExchangeRateApi;
        private readonly ILogger<ExchangeRateProvider> _logger;
        private readonly ExchangeRateProviderOptions _options;

        public ExchangeRateProvider(ICzechNationalBankExchangeRateApi czechNationalBankExchangeRateApi,
            IOptions<ExchangeRateProviderOptions> options,
            ILogger<ExchangeRateProvider> logger)
        {
            _czechNationalBankExchangeRateApi = czechNationalBankExchangeRateApi ??
                                                throw new ArgumentNullException(
                                                    nameof(czechNationalBankExchangeRateApi));
           _options = options is null ? throw new ArgumentNullException(nameof(options)) : options.Value;
           _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null) throw new ArgumentNullException(nameof(currencies));

            var exchangeRates = new List<ExchangeRate>();

            try
            {
                var results = await _czechNationalBankExchangeRateApi.GetExchangeRates();
                if (results == null)
                    return exchangeRates;

                exchangeRates.AddRange(from currency in currencies
                    select results.Rates.FirstOrDefault(w => w.CurrencyCode == currency.Code)
                    into currencyRate
                    where currencyRate != null
                    select new ExchangeRate(_options.SourceCurrency, new Currency(currencyRate.CurrencyCode),
                        currencyRate.Rate));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "There was a problem getting exchange rates.");
            }
            return exchangeRates;
        }
    }
}
