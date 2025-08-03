using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RideSharingApp.Application.Common.Interfaces.ExternalApis;
using System.Text.Json;

namespace RideSharingApp.Infrastructure.Currency
{
    public class CurrencyQuotationService : ICurrencyQuotationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CurrencyApiSetting _options;
        private readonly ILogger<CurrencyQuotationService> _logger;

        public CurrencyQuotationService(IHttpClientFactory httpClientFactory, IOptions<CurrencyApiSetting> options, ILogger<CurrencyQuotationService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<decimal?> GetDollarQuotationAsync()
        {
            var client = _httpClientFactory.CreateClient("CurrencyApi");
            try
            {
                var response = await client.GetAsync(_options.Url);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonDocument.Parse(json);
                // Ajuste conforme o formato da resposta da API externa
                var value = result.RootElement.GetProperty("USDBRL").GetProperty("bid").GetDecimal();
                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter cotação do dólar");
                return null;
            }
        }
    }
}
