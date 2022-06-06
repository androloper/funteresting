using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRStockTicker.Services;
using SignalRStockTicker.Services.Model;

namespace SignalRStockTicker.Hubs
{
    internal class StockTickerCallback : IStockTickerCallback
    {
        private readonly IClientProxy _proxy;

        public StockTickerCallback(IClientProxy proxy)
        {
            _proxy = proxy;
        }

        public async Task OnMarketStateChanged(MarketState state)
        {
            await _proxy?.SendAsync("OnMarketStateChanged", state.ToString());
        }

        public async Task OnMarketReset()
        {
            await _proxy?.SendAsync("OnReset");
        }

        public async Task OnStockChanged(Stock stock)
        {
            await _proxy?.SendAsync("OnStockChanged", stock);
        }
    }
}