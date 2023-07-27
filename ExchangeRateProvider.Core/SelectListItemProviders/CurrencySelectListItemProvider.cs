using ExchangeRateProvider.Core.Options;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace ExchangeRateProvider.Core.SelectListItemProviders
{
    public class CurrencySelectListItemProvider : ICurrencySelectListItemProvider
    {
        private readonly SupportedCurrenciesOptions _options;

        public CurrencySelectListItemProvider(IOptions<SupportedCurrenciesOptions> options)
        {
            _options = options is null ? throw new ArgumentNullException(nameof(options)) : options.Value;
        }

        public List<SelectListItem> GetSelectListItems()
        {
            var selectListItems = new List<SelectListItem> { new("--Please select--", string.Empty) };
            selectListItems.AddRange(_options.SupportedCurrencies
                .Select(currency => new SelectListItem(currency, currency[..3])).ToList());

            return selectListItems;
        }
    }
}
