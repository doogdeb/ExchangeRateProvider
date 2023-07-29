namespace ExchangeRateProvider.Core.Models
{
    public class ExchangeRate
    {
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, double value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        public Currency SourceCurrency { get; }

        public Currency TargetCurrency { get; }

        public double Value { get; }

        public string FormattedCurrency()
        {
            return $"{SourceCurrency.Code}/{TargetCurrency.Code}";
        }

    }
}
