using Browser_based_chat;
using Browser_based_chat.Services;
using Browser_based_chat.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Browser_based_chat.Test
{
    [TestClass]
    public class StockQBotUnitTest
    {
        [TestMethod]
        public void Consume_Stock_Quote_API()
        {
            var _client = new HttpClient();
            IStockQBotService stockQBotService = new StockQBotService(_client);
            var result = stockQBotService.GetStockQuoteAsync("aapl.us").Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }
    }
}