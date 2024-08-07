﻿using MyBasket.Domain;
using MyBasket.Domain.Models;
using MyBasket.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBasket.Infrastructure.Implementation
{
	public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
	{
        private readonly ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderDetail Orderdetail)
        {
            _context.OrderDetails.Update(Orderdetail);
        }
    }
}
