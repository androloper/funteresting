using SignalRStockTicker.Services.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRStockTicker.Services
{
    public interface IStockTickerService
    {
        IStockTickerCallback Callback { get; set; }

        MarketState MarketState { get; }
        IEnumerable<Stock> GetAllStocks();

        Task OpenMarket();
        Task CloseMarket();
        Task Reset();
    }
}