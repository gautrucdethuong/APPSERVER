using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;


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
        public JsonResult ListCart()
        {
            var product = _cart.ListCart();
            return Json(JsonResultResponse.ResponseSuccess(product));
        }

     
        //edit cart 
        [HttpPut("{id}")]
        public JsonResult EditCart(int id, Cart cart)
        {
            var checkexist = _cart.GetCart(id);

            if (checkexist != null)
            {
                cart.cart_id = checkexist.cart_id;
                _cart.EditCart(cart);
            }
            return Json(JsonResultResponse.ResponseChange("Update Successed."));
        }

        //create cart
        [HttpPost, AllowAnonymous]
        public JsonResult CreateCart(Cart cart)
        {
            if (ModelState.IsValid)
            {
                if (_cart.CheckQuantityCart(cart.quantity) == true)
                {
                    _cart.AddCart(cart);
                    return Json(JsonResultResponse.ResponseSuccess(cart));
                }
            }
            return Json(JsonResultResponse.ResponseFail("Input data already exists."));
        }


        //delete cart 
        [HttpDelete("{id}")]
        public JsonResult DeleteCart(int id)
        {
            var cart = _cart.GetCart(id);
            if (cart == null)
            {
                return Json(JsonResultResponse.ResponseFail("No matching results were found."));
            }
            _cart.DeleteCart(cart);
            return Json(JsonResultResponse.ResponseChange("Delete successed."));
        }
        
    }
}
