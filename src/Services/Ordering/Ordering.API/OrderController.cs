using AutoMapper;
using Contracts.Messages;
using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders;
using Ordering.Application.Features.V1.Orders.Commands.CreateOrder;
using Ordering.Application.Features.V1.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.V1.Orders.Commands.UpdateOrder;
using Ordering.Domain.Entities;
using Shared.SeedWork;
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
        private readonly IMapper _mapper;

        private readonly IOrderRepository _orderRepository;
        private readonly IMessagePublisher _messageProducer;

        public OrderController(IMediator mediator, ISmtpEmailService smtpEmailService, IMapper mapper, IOrderRepository orderRepository, IMessagePublisher messageProducer)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _smtpEmailService = smtpEmailService ?? throw new ArgumentNullException(nameof(_smtpEmailService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _orderRepository = orderRepository;
            _messageProducer = messageProducer;
        }

        private static class RouteNames
        {
            public const string GetOrders = nameof(GetOrders);
            public const string GetOrder = nameof(GetOrder);
            public const string CreateOrder = nameof(CreateOrder);
            public const string UpdateOrder = nameof(UpdateOrder);
            public const string DeleteOrder = nameof(DeleteOrder);
            public const string DeleteOrderByDocumentNo = nameof(DeleteOrderByDocumentNo);
        }

        [HttpGet(template:"{userName}", Name = RouteNames.GetOrders )]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrder([Required]string userName)
        {
            var query = new GetOrdersQuery(userName);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        //[HttpPost(Name = RouteNames.CreateOrder)]
        //[ProducesResponseType(typeof(long), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult> CreateOrder([FromBody]OrderDto orderDto)
        //{
        //    var command = _mapper.Map<CreateOrderCommand>(orderDto);
        //    var result = await _mediator.Send(command);
        //    return Ok(result);
        //}

        [HttpPost]
        public async Task<ActionResult> Create(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            var result = _mapper.Map<OrderDto>(await _orderRepository.CreateOrder(order));
            _messageProducer.SendMessage<OrderDto>(result);
            return Ok(result);
        }

        [HttpPut("{id:long}", Name = RouteNames.UpdateOrder)]
        [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderDto>> UpdateOrder([Required] long id, [FromBody] UpdateOrderCommand command)
        {
            command.SetId(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id:long}", Name = RouteNames.DeleteOrder)]
        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteOrder([Required] long id)
        {
            var command = new DeleteOrderCommand(id);
            await _mediator.Send(command);
            return NoContent();
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
