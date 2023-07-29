using ExchangeRateProvider.Core.Models;

namespace ExchangeRateProvider.Core.ExchangeRateProviders;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
}