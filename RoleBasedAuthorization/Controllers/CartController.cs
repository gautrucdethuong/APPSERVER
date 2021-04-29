using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService _cart;

        public CartController(ICartService cart)
        {
            _cart = cart;
        }

        //get all list porduct in Cart
        [HttpGet]
        public async Task<JsonResult> ListCart()
        {
            var result = _cart.ListCart();
            await Task.Delay(1000);
            return Json(JsonResultResponse.ResponseSuccess(result));
        }


        //edit cart 
        [HttpPut("{id}")]
        public async Task<JsonResult> EditCart(int id, Cart cart)
        {
            var result = _cart.GetCart(id);

            if (result != null)
            {
                 cart.CartId = result.CartId;
                _cart.EditCart(cart);
            }
            await Task.Delay(1000);
            return Json(JsonResultResponse.ResponseChange("Update Successed."));
        }

        //create cart
        [HttpPost]
        public async Task<JsonResult> CreateCart(Cart cart)
        {
            if (ModelState.IsValid)
            {
                if (_cart.CheckQuantityCart(cart.Quantity, cart.UserId, cart.ProductId.ToString()) == true)
                {
                    _cart.AddCart(cart);
                    await Task.Delay(1000);
                    return Json(JsonResultResponse.ResponseSuccess(cart));
                }
            }
            await Task.Delay(1000);
            return Json(JsonResultResponse.ResponseFail("Input data already exists."));
        }


        //delete cart 
        [HttpDelete("{id}")]
        public async Task<JsonResult> DeleteCart(int id)
        {
            var result = _cart.GetCart(id);
            if (result == null)
            {
                await Task.Delay(1000);
                return Json(JsonResultResponse.ResponseFail("No matching results were found."));
            }
            _cart.DeleteCart(result);
            await Task.Delay(1000);
            return Json(JsonResultResponse.ResponseChange("Delete successed."));
        }

        // filter by role
        [HttpPost]
        [Route("GetListByIdUser")]
        public async Task<JsonResult> GetListCartByIdUser(Cart cart)
        {
            var result = _cart.GetListCartByIdUser(cart);

            if (result == null)
            {
                await Task.Delay(1000);
                return Json(JsonResultResponse.ResponseFail("No matching results were found."));
            }
            await Task.Delay(1000);
            return Json(JsonResultResponse.ResponseSuccess(result));
        }

        //payment credit send otp
        [HttpPost]
        [Route("PaymentByCreditCard")]
        public async Task<JsonResult> PaymentByCreditCard(Cart cart)
        {
            var result = _cart.PaymentByCreditCard(cart);

            if (result == false)
            {
                await Task.Delay(1000);
                return Json(JsonResultResponse.ResponseFail("Send otp failed."));
            }
            await Task.Delay(1000);
            return Json(JsonResultResponse.ResponseChange("Send otp successed."));
        }

        //verify otp
        [HttpPost]
        [Route("VerificationPayment")]
        public async Task<JsonResult> VerificationPaymentByOTP([FromBody] VerifyModel verify)
        {
            var result = _cart.VerificationPaymentByOTP(verify.otp, verify.UserId);

            if (result == false)
            {
                await Task.Delay(1000);
                return Json(JsonResultResponse.ResponseFail("Verify failed."));
            }
            await Task.Delay(1000);
            return Json(JsonResultResponse.ResponseChange("Verify otp successed."));
        }

        //verify otp
        [HttpPost]
        [Route("PayOnDelivery"), AllowAnonymous]
        public async Task<JsonResult> PayOnDelivery([FromBody] VerifyModel verify)
        {
            var result = _cart.PayOnDelivery(verify.UserId);

            if (result == false)
            {
                await Task.Delay(1000);
                return Json(JsonResultResponse.ResponseFail("Payment failed."));
            }
            await Task.Delay(1000);
            return Json(JsonResultResponse.ResponseChange("Payment successed."));
        }

        // total value payment
        [HttpPost]
        [Route("TotalPayment"), AllowAnonymous]
        public async Task<JsonResult> TotalValuePayment(Cart cart)
        {
            var result = _cart.TotalValuePayment(cart.UserId);

            if(result == 0)
            {
                await Task.Delay(1000);
                return Json(JsonResultResponse.ResponseFail("Payment not exist."));
            }
            await Task.Delay(1000);
            return Json(JsonResultResponse.ResponseSuccess(result));
        }

        // count product on cart by id user
        [HttpPost]
        [Route("CountProduct")]
        public async Task<JsonResult> CountProductOnCart(Cart cart)
        {
            var result = _cart.CountProductOnCart(cart);

            await Task.Delay(1000);
            return Json(JsonResultResponse.ResponseSuccess(result));
        }

    }
}
