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
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId && !ci.IsDeleted)
                .ToListAsync();

            return cartItems;
        }


        public async Task<int> CountItemsByCartIdAsync(Guid cartId)
        {
            var result =  await _context.CartItems
            .Where(ci => ci.CartId == cartId && ci.IsDeleted == false )
            .CountAsync();
            return result;
        }

        public async Task<bool> Delete(Guid cartItemId)
        {
            var cart = await _context.CartItems.FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (cart == null)
            {
                return false;
            }

            cart.IsDeleted = true;
            _context.CartItems.Update(cart);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
