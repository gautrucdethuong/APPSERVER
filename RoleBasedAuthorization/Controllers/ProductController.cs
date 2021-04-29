using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;

namespace RoleBasedAuthorization.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {

        private readonly IProductService _product;

        public ProductController(IProductService product)
        {
            _product = product;
        }
        

        [HttpGet]
        public JsonResult ListProduct()
        {
            var product = _product.getAllProduct();
            return Json(JsonResultResponse.ResponseSuccess(product));

        }


        //get by id
        [HttpGet("{id}")]
        public JsonResult GetProduct(int id)
        {
            var product = _product.GetProduct(id);

            if (product == null)
                return Json(JsonResultResponse.ResponseFail("No matching results were found."));

            return Json(JsonResultResponse.ResponseSuccess(product));

        }

        //edit
        [HttpPut("{id}")]
        public JsonResult EditProduct(int id, Product p)
        {
            var checkexist = _product.GetProduct(id);

            if (checkexist != null)
            {
                p.product_id = checkexist.product_id;
                _product.EditProduct(p);
            }
            return Json(JsonResultResponse.ResponseChange("Update Successed."));
        }

        //create
        [HttpPost]
        public JsonResult CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                if (_product.CheckExistProperties(product.product_name) == true)
                {
                    _product.CreateProduct(product);
                    return Json(JsonResultResponse.ResponseSuccess(product));
                }
            }
            return Json(JsonResultResponse.ResponseFail("Input data already exists."));
        }


        //delete
        [HttpDelete("{id}")]
        public JsonResult DeleteProduct(int id)
        {
            var product = _product.GetProduct(id);
            if (product == null)
            {
                return Json(JsonResultResponse.ResponseFail("No matching results were found."));
            }
            _product.DeleteProduct(product);
            return Json(JsonResultResponse.ResponseChange("Delete successed."));
        }


        //get by id
        [HttpGet("seller")]
        public JsonResult GetTopSeller()
        {
            var product = _product.GetTopSeller();
            return Json(JsonResultResponse.ResponseSuccess(product));
        }

    }
}
