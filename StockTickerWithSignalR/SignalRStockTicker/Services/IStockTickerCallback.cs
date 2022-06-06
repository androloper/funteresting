using SignalRStockTicker.Services.Model;
using System.Threading.Tasks;

namespace SignalRStockTicker.Services
{
    public interface IStockTickerCallback
    {
        Task OnMarketStateChanged(MarketState state);
        Task OnMarketReset();
        Task OnStockChanged(Stock stock);
    }
}