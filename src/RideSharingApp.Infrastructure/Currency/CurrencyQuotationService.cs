using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RideSharingApp.Application.Common.Interfaces.ExternalApis;
using System.Text.Json;

namespace RideSharingApp.Infrastructure.Currency;

public class CurrencyQuotationService : ICurrencyQuotationService
{
    private readonly HttpClient _client;
    private readonly CurrencyApiSetting _options;
    private readonly ILogger<CurrencyQuotationService> _logger;

    public CurrencyQuotationService(
        IHttpClientFactory httpClientFactory,
        IOptions<CurrencyApiSetting> options,
        ILogger<CurrencyQuotationService> logger)
    {
        _client = httpClientFactory.CreateClient("CurrencyApi");
        _options = options.Value;
        _logger = logger;
    }

    public async Task<decimal?> GetDollarQuotationAsync()
    {
        try
        {
            var response = await _client.GetAsync(_options.Url);
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
