using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System.Collections.Generic;
using System.Linq;

namespace RoleBasedAuthorization.Reponsitory
{
    public class CartRepository : ICartService
    {

        private readonly DBContext db;

        public CartRepository(DBContext dbContext)
        {
            db = dbContext;
        }


        public Cart AddCart(Cart cart)
        {
            db.Add(cart);
            db.SaveChanges();
            return cart;
        }

        public bool CheckQuantityCart(int quantity)
        {
            if(quantity > 0)
            {
                return true;
            }
            return false;
        }

        public void DeleteCart(Cart cart)
        {
            db.Carts.Remove(cart);
            db.SaveChanges();
        }


        public Cart EditCart(Cart cart)
        {
            var cartexist = db.Carts.Find(cart.cart_id);

            if (cartexist != null)
            {
                cartexist.quantity = cart.quantity;
                
                db.SaveChanges();
            }
            return cart;

        }

        public Cart GetCart(int id)
        {
            var cart = db.Carts.Find(id);
            return cart;
        }

        public IEnumerable<Cart> ListCart()
        {
            return db.Carts.ToList();
        }

    }
}
