using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExchangeRateProvider.Core.SelectListItemProviders;

public interface ICurrencySelectListItemProvider
{
    List<SelectListItem> GetSelectListItems();
}