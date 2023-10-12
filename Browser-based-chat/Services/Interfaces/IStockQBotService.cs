using Microsoft.AspNetCore.Mvc;

namespace Browser_based_chat.Services.Interfaces
{
    public interface IStockQBotService
    {
        public Task<IActionResult> GetStockQuoteAsync(string stockCode);
    }
}
