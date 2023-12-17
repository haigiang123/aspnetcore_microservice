using AutoMapper;
using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<Order>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        private const string MethodName = "UpdateOrderCommandHandler";

        public UpdateOrderCommandHandler(ILogger logger, IMapper mapper, IOrderRepository orderRepository) 
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<ApiResult<Order>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
            if (orderEntity is null) throw new NotFoundException(nameof(Order), request.Id);

            _logger.Information($"BEGIN: {MethodName} - Order: {request.Id}");

            orderEntity = _mapper.Map(request, orderEntity);
            await _orderRepository.UpdateAsync(orderEntity);

            _logger.Information($"END: {MethodName} - Order: {request.Id}");
            return new ApiSuccessResult<Order>(orderEntity ?? new Order());
        }
    }
}
