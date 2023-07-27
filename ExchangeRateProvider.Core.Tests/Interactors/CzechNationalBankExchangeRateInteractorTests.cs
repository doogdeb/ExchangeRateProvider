using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateProvider.Core.Apis;
using ExchangeRateProvider.Core.Interactors;
using ExchangeRateProvider.Core.Models;
using ExchangeRateProvider.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ExchangeRateProvider.Core.Tests.Interactors
{
    public class CzechNationalBankExchangeRateInteractorTests
    {
        private readonly Mock<ICzechNationalBankExchangeRateApi> _exchangeRateApi;
        private readonly Mock<AbstractLogger<CzechNationalBankExchangeRateInteractor>> _logger;
        private readonly CzechNationalBankExchangeRateInteractor _interactor;
        
        public CzechNationalBankExchangeRateInteractorTests()
        {
            _exchangeRateApi = new Mock<ICzechNationalBankExchangeRateApi>(MockBehavior.Strict);
            _logger = new Mock<AbstractLogger<CzechNationalBankExchangeRateInteractor>>(MockBehavior.Strict);
            _interactor =
                new CzechNationalBankExchangeRateInteractor(_exchangeRateApi.Object, _logger.Object);
        }

        [Fact]
        public void Ctor_NullExchangeRateApi_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new CzechNationalBankExchangeRateInteractor(default!, _logger.Object));

            Assert.Equal("Value cannot be null. (Parameter 'exchangeRateApi')", exception.Message);
        }

        [Fact]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new CzechNationalBankExchangeRateInteractor(_exchangeRateApi.Object, default!));

            Assert.Equal("Value cannot be null. (Parameter 'logger')", exception.Message);
        }

        [Fact]
        public async Task GetExchangeRates_Exception_LogsReturnsNull()
        {
            //Arrange
            _exchangeRateApi.Setup(s => s.GetExchangeRates())
                .Throws<InvalidOperationException>();
            _logger.Setup(s => s.Log(LogLevel.Error, It.IsAny<InvalidOperationException>(),
                $"There was a problem getting exchange rates"));

            //Act
            var result = await _interactor.GetExchangeRates();

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetExchangeRates_NullExchangeRateResponse_ReturnsNull()
        {
            //Arrange
            _exchangeRateApi.Setup(s => s.GetExchangeRates())
                .ReturnsAsync((CzechNationalBankExchangeRateApiResponse?)null);

            //Act
            var result = await _interactor.GetExchangeRates();

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetExchangeRates_Success_ReturnsMappedExchangeRateModel()
        {
            //Arrange
            var currencyCodeName = "GBP - UK";
            var model = new CzechNationalBankExchangeRateApiResponse
            {
                Rates = new List<ConversionRate> { new() { CurrencyCode = "GBP", Rate = 1, Country = "UK"} }
            };
            _exchangeRateApi.Setup(s => s.GetExchangeRates()).ReturnsAsync(model);

            //Act
            var result = await _interactor.GetExchangeRates();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(model.Rates.First().Rate,
                float.Parse(result.ConversionRateModel.First(s => s.CurrencyCodeName == currencyCodeName)
                    .CurrencyRate));
        }
    }
}