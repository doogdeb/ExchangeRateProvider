using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateProvider.Core.Models;
using ExchangeRateProvider.Core.Options;
using ExchangeRateProvider.Infrastructure.Apis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace ExchangeRateProvider.Infrastructure.Tests.Apis
{
    public class CzechNationalBankExchangeRateApiTests
    {
        private readonly Mock<AbstractLogger<CzechNationalBankExchangeRateApi>> _logger;
        private readonly IOptions<CzechNationalBankExchangeRateApiOptions> _options;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly CzechNationalBankExchangeRateApi _api;
        private const string BaseUrl = "https://test.com/";

        public CzechNationalBankExchangeRateApiTests()
        {
            _logger = new Mock<AbstractLogger<CzechNationalBankExchangeRateApi>>(MockBehavior.Strict);
            _options = Options.Create(new CzechNationalBankExchangeRateApiOptions { RequestUri = "api/v3" });
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{'id':1,'value':'1'}]"),
                })
                .Verifiable();

            // use real http client with mocked handler here
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri(BaseUrl),
            };

            _api = new CzechNationalBankExchangeRateApi(_httpClient, _options, _logger.Object);
        }
        
        [Fact]
        public void Ctor_NullHttpClient_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new CzechNationalBankExchangeRateApi(default!, _options, _logger.Object));

            Assert.Equal("Value cannot be null. (Parameter 'client')", exception.Message);
        }

        [Fact]
        public void Ctor_NullOptions_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new CzechNationalBankExchangeRateApi(_httpClient, default!, _logger.Object));

            Assert.Equal("Value cannot be null. (Parameter 'options')", exception.Message);
        }

        [Fact]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new CzechNationalBankExchangeRateApi(_httpClient, _options, default!));

            Assert.Equal("Value cannot be null. (Parameter 'logger')", exception.Message);
        }
        
        [Fact]
        public async Task GetExchangeRates_NotSuccessful_ReturnsNull()
        {
            //Arrange
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("[{'id':1,'value':'1'}]"),
                })
                .Verifiable();

            //Act
            var result = await _api.GetExchangeRates();

            //Assert
            Assert.Null(result);

            //Verify
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get // we expected a GET request
                        && req.RequestUri ==
                        new Uri($"{BaseUrl}{_options.Value.RequestUri}") // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetExchangeRates_Successful_ReturnsApiResponse()
        {
            //Arrange
            var response = new CzechNationalBankExchangeRateApiResponse
                { Rates = new List<ConversionRate> { new ConversionRate() } };
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(response))
                })
                .Verifiable();

            //Act
            var result = await _api.GetExchangeRates();

            //Assert
            Assert.NotNull(result);
            Assert.Single(result.Rates);

            //Verify
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get // we expected a GET request
                        && req.RequestUri ==
                        new Uri($"{BaseUrl}{_options.Value.RequestUri}") // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task GetExchangeRates_Exception_LogsReturnsNull()
        {
            //Arrange
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Throws<InvalidOperationException>()
                .Verifiable();
            _logger.Setup(s => s.Log(LogLevel.Error, It.IsAny<InvalidOperationException>(),
                "There was a problem retrieving exchange rates"));

            //Act
            var result = await _api.GetExchangeRates();

            //Assert
            Assert.Null(result);
            
            //Verify
            _logger.Verify(s => s.Log(LogLevel.Error, It.IsAny<InvalidOperationException>(),
                "There was a problem retrieving exchange rates"), Times.Once);
        }
    }
}