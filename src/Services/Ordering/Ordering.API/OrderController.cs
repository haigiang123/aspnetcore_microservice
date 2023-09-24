using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ordering.API
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        private static class RouteNames
        {
            public const string GetOrders = nameof(GetOrders); 
        } 

        [HttpGet(template:"{userName}", Name = RouteNames.GetOrders )]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrder([Required]string userName)
        {
            var query = new GetOrdersQuery(userName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
