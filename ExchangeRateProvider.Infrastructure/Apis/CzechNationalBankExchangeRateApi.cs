using System.Text.Json;
using ExchangeRateProvider.Core.Apis;
using ExchangeRateProvider.Core.Models;
using ExchangeRateProvider.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateProvider.Infrastructure.Apis
{
    public class CzechNationalBankExchangeRateApi : ICzechNationalBankExchangeRateApi
    {
        private readonly HttpClient _client;
        private readonly ILogger<CzechNationalBankExchangeRateApi> _logger;
        private readonly CzechNationalBankExchangeRateApiOptions _options;

        public CzechNationalBankExchangeRateApi(HttpClient client, IOptions<CzechNationalBankExchangeRateApiOptions> options,
            ILogger<CzechNationalBankExchangeRateApi> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options is null ? throw new ArgumentNullException(nameof(options)) : options.Value;
        }

        public async Task<CzechNationalBankExchangeRateApiResponse?> GetExchangeRates()
        {
            try
            {
                var result = await _client.GetAsync($"{_options.RequestUri}");
                return !result.IsSuccessStatusCode
                    ? null
                    : JsonSerializer.Deserialize<CzechNationalBankExchangeRateApiResponse>(await result.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "There was a problem retrieving exchange rates");
                return null;
            }
        }
    }
}