using RoleBasedAuthorization.Model;
using System.Collections.Generic;

namespace RoleBasedAuthorization.Service
{
    public interface ICartService
    {

        IEnumerable<Cart> ListCart();

        Cart GetCart(int id);

        IEnumerable<Cart>  GetListCartByIdUser(Cart cart);

        Cart AddCart(Cart cart);

        Cart EditCart(Cart Cart);

        void DeleteCart(Cart cart);

        bool CheckQuantityCart(int? quantity, string UserId, string ProductId);

        bool PaymentByCreditCard(Cart cart);

        bool VerificationPaymentByOTP(string otp, int UserId);

        float TotalValuePayment(string UserId);

        int CountProductOnCart(Cart cart);

        bool PayOnDelivery(int UserId);


    }
}
