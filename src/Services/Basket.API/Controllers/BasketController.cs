using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
           _basketRepository = basketRepository;
        }

        [HttpGet(template: "{username}", Name = "GetBasket")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> GetUserName([Required]string userName)
        {
            var result = await _basketRepository.GetBasketByUserName(userName);
            return Ok(result ?? new Entities.Cart());  
        }

        [HttpPost(Name = "UpdateBasket")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Cart>> UpdateBasket([FromBody] Entities.Cart cart)
        {
            var option = new DistributedCacheEntryOptions ()
            { 
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(60),
                SlidingExpiration = TimeSpan.FromMinutes(5),
            };

            var result = await _basketRepository.UpdateBasket(cart, option);
            return Ok(result);
        }

        [HttpDelete(template:"{username}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteBasket([Required]string userName)
        {
            var result = await _basketRepository.DeleteBasketByUserName(userName);
            return Ok(result);
        }
    }
}
