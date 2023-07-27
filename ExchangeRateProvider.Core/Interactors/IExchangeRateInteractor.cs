using ExchangeRateProvider.Core.Models;

namespace ExchangeRateProvider.Core.Interactors;

public interface IExchangeRateInteractor
{
    Task<ExchangeRateModel?> GetExchangeRates();
}