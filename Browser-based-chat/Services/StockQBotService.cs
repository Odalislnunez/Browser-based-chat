using Browser_based_chat.Models;
using Browser_based_chat.Services.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Browser_based_chat.Services
{
    public class StockQBotService : IStockQBotService
    {
        public HttpClient _client;
        public StockQBotService(HttpClient httpClient)
        {
            _client = httpClient;
        }
        public async Task<IActionResult> GetStockQuoteAsync(string stockCode)
        {
            var stockQuote = new StockQuote();

            try
            {
                using (var response = await _client.GetAsync($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (var stream = response.Content.ReadAsStreamAsync().Result)
                        using (var reader = new StreamReader(stream))
                        {
                            var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
                            stockQuote = csv.GetRecords<StockQuote>().FirstOrDefault();

                            var media = (stockQuote.Open + stockQuote.High + stockQuote.Low + stockQuote.Close) / 4;
                            return new JsonResult($"{stockQuote.Symbol} quote is ${media.ToString("n2")} per share");
                        }
                    }
                }
                throw new Exception("Failed to retrieve CSV data from the API. Check stock quote code.");
            }
            catch (Exception)
            {
                throw new Exception("Failed to connect to the API or retrieving data. Check stock quote code.");
            }
        }
    }
}