using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateProvider.Core.Apis;
using ExchangeRateProvider.Core.Models;
using ExchangeRateProvider.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ExchangeRateProvider.Core.Tests.ExchangeRateProviders
{
    public class ExchangeRateProviderTests : IDisposable
    {
        private readonly Mock<ICzechNationalBankExchangeRateApi> _czechNationalBankExchangeRateApi;
        private readonly Mock<AbstractLogger<Core.ExchangeRateProviders.ExchangeRateProvider>> _logger;
        private readonly IOptions<ExchangeRateProviderOptions >_options;
        private readonly Core.ExchangeRateProviders.ExchangeRateProvider _provider;
        private List<Currency> _currencies;

        public ExchangeRateProviderTests()
        {
            _currencies = new List<Currency>{new("GBP")};
            _czechNationalBankExchangeRateApi = new Mock<ICzechNationalBankExchangeRateApi>(MockBehavior.Strict);
            _logger = new Mock<AbstractLogger<Core.ExchangeRateProviders.ExchangeRateProvider>>(MockBehavior.Strict);
            _options = Microsoft.Extensions.Options.Options.Create(new ExchangeRateProviderOptions());

            _provider = new Core.ExchangeRateProviders.ExchangeRateProvider(_czechNationalBankExchangeRateApi.Object,
                _options, _logger.Object);
        }

        public void Dispose()
        {
            _czechNationalBankExchangeRateApi.VerifyAll();
            _logger.VerifyAll();
        }

        [Fact]
        public void Ctor_NullApi_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new Core.ExchangeRateProviders.ExchangeRateProvider(default!,
                    _options, _logger.Object));

            Assert.Equal("Value cannot be null. (Parameter 'czechNationalBankExchangeRateApi')", exception.Message);
        }

        [Fact]
        public void Ctor_NullOptions_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new Core.ExchangeRateProviders.ExchangeRateProvider(_czechNationalBankExchangeRateApi.Object,
                    default!, _logger.Object));

            Assert.Equal("Value cannot be null. (Parameter 'options')", exception.Message);
        }

        [Fact]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new Core.ExchangeRateProviders.ExchangeRateProvider(_czechNationalBankExchangeRateApi.Object,
                    _options, default!));

            Assert.Equal("Value cannot be null. (Parameter 'logger')", exception.Message);
        }

        [Fact]
        public async Task GetExchangeRates_NullCurrencies_ThrowsArgumentNullException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _provider.GetExchangeRates(default!));

            Assert.Equal("Value cannot be null. (Parameter 'currencies')", exception.Message);
        }

        [Fact]
        public async Task GetExchangeRates_NullExchangeRates_ReturnsNoExchangeRates()
        {
            _czechNationalBankExchangeRateApi.Setup(s => s.GetExchangeRates()).ReturnsAsync((CzechNationalBankExchangeRateApiResponse?)null);
            var result = await _provider.GetExchangeRates(_currencies);

            Assert.False(result.Any());
        }

        [Fact]
        public async Task GetExchangeRates_ReturnsFilteredExchangeRates()
        {
            var gbpRate = 1.15;
            _czechNationalBankExchangeRateApi.Setup(s => s.GetExchangeRates()).ReturnsAsync(
                new CzechNationalBankExchangeRateApiResponse
                {
                    Rates = new List<ConversionRate>
                    {
                        new() { CurrencyCode = "GBP", Rate = gbpRate },
                        new() { CurrencyCode = "USD", Rate = 1.5 }
                    }
                });
            var result = await _provider.GetExchangeRates(_currencies);

            Assert.NotNull(result);
            var exchangeRates = result as ExchangeRate[] ?? result.ToArray();
            Assert.Single(exchangeRates);
            Assert.Equal(gbpRate, exchangeRates.First().Value);
        }

        [Fact]
        public async Task GetExchangeRates_Exception_LogsReturnsNoExchangeRates()
        {
            _logger.Setup(s => s.Log(LogLevel.Error, It.IsAny<InvalidOperationException>(),
                "There was a problem getting exchange rates."));
            _czechNationalBankExchangeRateApi.Setup(s => s.GetExchangeRates()).Throws<InvalidOperationException>();
            
            var result = await _provider.GetExchangeRates(_currencies);

            Assert.NotNull(result);
            var exchangeRates = result as ExchangeRate[] ?? result.ToArray();
            Assert.False(exchangeRates.Any());
        }

       
    }
}
