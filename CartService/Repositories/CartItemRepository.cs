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
    public class CartItemRepository: GenericRepository<CartItem>
    {
        public CartItemRepository()
        {
        }

        public CartItemRepository(CartServiceDBContext context) => _context = context;


        public async Task<List<CartItem>> GetCartItemsByCartIdAsync(Guid cartId)
        {
            return await _context.CartItems.Where(ci => ci.CartId == cartId).ToListAsync();
        }

        public async Task<int> CountItemsByCartIdAsync(Guid cartId)
        {
            var result =  await _context.CartItems
            .Where(ci => ci.CartId == cartId)
            .CountAsync();
            Console.WriteLine("repo: " + result);
            return result;
        }
       


    }
}
