using Microsoft.EntityFrameworkCore;
using Repositories.Basic;
using Repositories.DBContext;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CartRepository: GenericRepository<Cart>
    {
        public CartRepository()
        {
        }

        public CartRepository(CartServiceDBContext context) => _context = context;


        public async Task<Cart?> GetCartByCustomerIdAsync(Guid customerId)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

    }
}
