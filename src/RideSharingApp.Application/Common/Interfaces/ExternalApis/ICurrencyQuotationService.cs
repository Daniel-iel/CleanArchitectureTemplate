namespace RideSharingApp.Infrastructure.Currency
{
    public interface ICurrencyQuotationService
    {
        Task<decimal?> GetDollarQuotationAsync();
    }
}
