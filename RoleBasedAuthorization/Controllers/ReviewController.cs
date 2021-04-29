using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoleBasedAuthorization.Helper;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewService _review;

        public ReviewController(IReviewService reviewService)
        {
            _review = reviewService;
        }

        //get all list, allow only admin get alllist        
        [HttpGet]
        public JsonResult ListReview()
        {
            var reviews = _review.GetAllReview();
            return Json(JsonResultResponse.ResponseSuccess(reviews));

        }

        //get by id
        [HttpGet("{id}")]
        public JsonResult GetReview(int id)
        {
            var review = _review.GetReview(id);

            if (review == null)
                return Json(JsonResultResponse.ResponseFail("No matching results were found."));

            return Json(JsonResultResponse.ResponseSuccess(review));

        }

        //edit
        [HttpPut("{id}")]
        public JsonResult EditReview(int id, Review review)
        {
            var checkexist = _review.GetReview(id);

            if (checkexist != null)
            {
                review.ReviewId = checkexist.ReviewId;
                _review.EditReview(review);
            }
            return Json(JsonResultResponse.ResponseChange("Update Successed."));
        }

        //create
        [HttpPost]
        public JsonResult CreateReview(Review review)
        {
            if (ModelState.IsValid)
            {
                if (_review.CrateReview(review) == true)
                {                    
                    return Json(JsonResultResponse.ResponseSuccess(review));
                }                                
            }
            return Json(JsonResultResponse.ResponseFail("User reviewed. Not Comment"));
        }


        //delete
        [HttpDelete("{id}")]
        public JsonResult DeleteReview(int id)
        {
            var review = _review.GetReview(id);
            if (review == null)
            {
                return Json(JsonResultResponse.ResponseFail("No matching results were found."));
            }
            _review.DeleteReview(review);
            return Json(JsonResultResponse.ResponseChange("Delete successed."));
        }

        //[HttpGet("getIdProduct")]
        [HttpPost("GetReviewByIdProduct")]
        public JsonResult GetReviewByIdProduct(Review review)
        {
            var search = _review.GetReviewByIdProduct(review.ProductId);

            if (search == null)
            {
                return Json(JsonResultResponse.ResponseFail("No matching results were found."));
            }
            return Json(JsonResultResponse.ResponseSuccess(search));
        }
    }
}
