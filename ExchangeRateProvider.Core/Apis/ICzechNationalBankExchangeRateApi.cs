using ExchangeRateProvider.Core.Models;

namespace ExchangeRateProvider.Core.Apis;

public interface ICzechNationalBankExchangeRateApi
{
    Task<CzechNationalBankExchangeRateApiResponse?> GetExchangeRates();
}