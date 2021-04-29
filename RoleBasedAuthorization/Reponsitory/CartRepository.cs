using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace RoleBasedAuthorization.Reponsitory
{
    public class CartRepository : ICartService
    {

        private readonly DBContext db;

        public CartRepository(DBContext dbContext)
        {
            db = dbContext;
        }

        // add cart
        public Cart AddCart(Cart cart)
        {
            var getData = db.Carts.FirstOrDefault(x => x.ProductId == cart.ProductId && x.UserId == cart.UserId);
            
            if (getData != null)
            {
                getData.Quantity = cart.Quantity + getData.Quantity;                
            }
            else
            {
                db.Add(cart);
            }
            db.SaveChanges();
            return cart;
        }
        
        // check input cart
        public bool CheckQuantityCart(int? quantity, string UserId, string ProductId)
        {
            var checkIdExist = db.Carts.Where(x => x.User.user_id.ToString() == UserId &&  x.Product.product_id.ToString() == ProductId);
           
            if (quantity > 0 && checkIdExist != null)
            {                
                return true;
            }
            return false;
        }

        // delete cart
        public void DeleteCart(Cart cart)
        {
            db.Carts.Remove(cart);
            db.SaveChanges();
        }

        // update quantity product on cart
        public Cart EditCart(Cart cart)
        {
            var cartexist = db.Carts.Find(cart.CartId);

            if (cartexist != null)
            {
                cartexist.Quantity = cart.Quantity;
                db.SaveChanges();
            }
            return cart;

        }

        // get cart by id
        public Cart GetCart(int id)
        {
            var cart = db.Carts.Find(id);
            return cart;
        }

        // get list cart by id
        public IEnumerable<Cart> GetListCartByIdUser(Cart cart)
        {          
            return db.Carts.Where(x => x.UserId == cart.UserId).ToList();
        }

        // total cart
        public IEnumerable<Cart> ListCart()
        {
            return db.Carts.ToList();
        }

        // send otp when payment credit cart
        public bool PaymentByCreditCard(Cart cart)
        {

            TwilioClient.Init(Constant.accountSid, Constant.authToken);

            int otp = GenerationOTP.GenerationRandomOTP();

            var entity = db.Users.Find(Int32.Parse(cart.UserId));
            if(entity != null)
            {
                entity.user_otp = otp.ToString();
                db.SaveChanges();

                MessageResource.Create(
                    body: otp.ToString() + Constant.OPTMessage,
                    from: new Twilio.Types.PhoneNumber(Constant.contactSystems),
                    to: new Twilio.Types.PhoneNumber(Constant.contactCustomer)
                );
                return true;
            }
            return false;
            
        }

        // total price checkout
        public float TotalValuePayment(string UserId)
        {
            float total = 0;
            var list = db.Carts.FirstOrDefault(x => x.UserId == UserId);

            if (list != null)
            {
                var dataCart = db.Carts
             .Join(db.Products,
                   cart => cart.ProductId,
                   pro => pro.product_id,
                   (cart, pro) => new { cart, pro })   
             .Where(z => z.cart.UserId == UserId)
             .Select(z => new
             {
                 ProductId = z.pro.product_id,
                 Quantity = z.cart.Quantity,
                 Price = z.pro.product_price
             }).ToList();

                foreach (var item in dataCart)
                {
                    total += (float)(item.Quantity * item.Price);
                }

                return total;
            }
            return 0;
        }

        // count product on cart
        public int CountProductOnCart(Cart cart)
        {
            int count = db.Carts.Where(item => item.UserId == cart.UserId).Count();

            return count;
        }

        //verify otp when payment 
        public bool VerificationPaymentByOTP(string otp, int UserId)
        {
            // check opt exist in database              
            var entity = db.Users.FirstOrDefault(item => item.user_otp == otp && item.user_id == UserId);
            var carts = db.Carts.Where(x => x.UserId == UserId.ToString()).ToList();

            if (entity != null)
            {
               
                foreach (var item in carts)
                {
                    db.Carts.Remove(item);
                }
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // payment and clear product when user payment
        public bool PayOnDelivery(int UserId)
        {
            // check user id exist in database              
            var entity = db.Users.FirstOrDefault(item => item.user_id == UserId);
            var carts = db.Carts.Where(x => x.UserId == UserId.ToString()).ToList();

            if (entity != null)
            {
                foreach (var item in carts)
                {
                    db.Carts.Remove(item);
                }
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
