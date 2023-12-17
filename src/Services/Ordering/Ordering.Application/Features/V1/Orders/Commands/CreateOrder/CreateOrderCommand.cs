using AutoMapper;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<ApiResult<long>>, IMapFrom<Order>
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string InvoiceAddress { get; set; }
        //public string? InvoiceAddress
        //{
        //    get => _invoiceAddress;
        //    set => _invoiceAddress = value ?? ShippingAddress;
        //}

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrderDto, CreateOrderCommand>();
            profile.CreateMap<CreateOrderCommand, Order>();
        }
    }
}
