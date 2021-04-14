using RoleBasedAuthorization.Model;
using System.Collections.Generic;

namespace RoleBasedAuthorization.Service
{
    public interface ICartService
    {

        IEnumerable<Cart> ListCart();

        Cart GetCart(int id);

        Cart AddCart(Cart cart);

        Cart EditCart(Cart Cart);

        void DeleteCart(Cart cart);

        bool CheckQuantityCart(int quantity);
    }
}
