using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders;
using Shared.Services;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ordering.API
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISmtpEmailService _smtpEmailService;

        public OrderController(IMediator mediator, ISmtpEmailService smtpEmailService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _smtpEmailService = smtpEmailService ?? throw new ArgumentNullException();
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

        [HttpGet("TestEmail")]
        public async Task<IActionResult> TestEmail()
        {
            var message = new MailRequest()
            {
                Body = "<h1>Hello<h1>",
                Subject = "Test",
                ToAddress = "haigt110895@gmail.com"
            };

            await _smtpEmailService.SendEmailAsync(message);
            return Ok();
        }

    }
}
