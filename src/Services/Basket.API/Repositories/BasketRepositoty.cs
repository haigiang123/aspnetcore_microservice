using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using ILogger = Serilog.ILogger;

namespace Basket.API.Repositories
{
    public class BasketRepositoty : IBasketRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ISerializeService _serializeService;
        private readonly ILogger _logger;

        public BasketRepositoty(IDistributedCache distributedCache, ISerializeService serializeService, ILogger logger)
        {
            _distributedCache = distributedCache;
            _serializeService = serializeService;
            _logger = logger;
        }

        public async Task<bool> DeleteBasketByUserName(string userName)
        {
            _logger.Information($"BEGIN: DeleteBasketByUserName {userName}");
            try
            {
                await _distributedCache.RemoveAsync(userName);
                _logger.Information($"END: DeleteBasketByUserName {userName}");

                return true;
            }
            catch(Exception ex)
            {
                _logger.Error("Error message: " + ex.Message);
                throw;
            }
            
        }

        public async Task<Cart?> GetBasketByUserName(string userName)
        {
            _logger.Information($"BEGIN: GetBasketByUserName {userName}");
            var basket = await _distributedCache.GetStringAsync(userName);
            _logger.Information($"END: GetBasketByUserName {userName}");

            return string.IsNullOrEmpty(basket) ? null : _serializeService.Deserialize<Cart?>(basket);
        }

        public async Task<Cart?> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
        {
            _logger.Information($"BEGIN: UpdateBasket {cart.UserName}");
            if (options != null)
            {
                await _distributedCache.SetStringAsync(cart.UserName, _serializeService.Serialize(cart), options);
            }
            else
            {
                await _distributedCache.SetStringAsync(cart.UserName, _serializeService.Serialize(cart));
            }
            _logger.Information($"END: UpdateBasket {cart.UserName}");

            return await GetBasketByUserName(cart.UserName);
        }
    }
}
