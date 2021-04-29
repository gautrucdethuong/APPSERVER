using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Reponsitory
{
    public class ReviewReponsitory : IReviewService
    {
        
        private readonly DBContext db;

        public ReviewReponsitory(DBContext dbContext)
        {
            db = dbContext;
        }        

        // create review post
        public bool CrateReview(Review review) 
        {
            var check = db.Reviews.FirstOrDefault(x => x.ProductId == review.ProductId);
            if(check != null)
            {
                return false;
            }
            else
            {
                db.Add(review);
                db.SaveChanges();
                return true;
            }            
        }

        //delete review post
        public void DeleteReview(Review review)
        {
            db.Reviews.Remove(review);
            db.SaveChanges();
        }

        // edit review post
        public Review EditReview(Review review)
        {
            var edit_review = db.Reviews.Find(review.ReviewId);
            if(edit_review != null)
            {
                edit_review.Rating = review.Rating;
                edit_review.Content = review.Content;
                DateTime sentnow = DateTime.Now;
                edit_review.Sent = sentnow;                
                db.SaveChanges();
            }
            return review;
        }

        // get all review post
        public IEnumerable<Review> GetAllReview()
        {
            return db.Reviews.ToList();
        }

        //get review post by id
        public Review GetReview(int id)
        {
            var review = db.Reviews.Find(id);
            return review;
        }

        public IEnumerable<Review> GetReviewByIdProduct(int id)
        {

            var list = db.Reviews.FirstOrDefault(x => x.ProductId == id);

            return db.Reviews.Where(x => x.ProductId == id).ToList();
            
        }
    }
}
