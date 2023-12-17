using MediatR;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand : IRequest<ApiResult<bool>>
    {
        public DeleteOrderCommand(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
