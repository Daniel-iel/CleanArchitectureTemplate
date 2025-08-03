namespace RideSharingApp.Application.Common.Interfaces.ExternalApis
{
    public interface ICurrencyQuotationService
    {
        Task<decimal?> GetDollarQuotationAsync();
    }
}
