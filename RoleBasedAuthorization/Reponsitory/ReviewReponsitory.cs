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
            if(db.Users.Any(x => x.user_id.ToString() == review.UserId))
            {
                db.Add(review);
                db.SaveChanges();
                return true;
            }
            return false;
            
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

        public IEnumerable<Review> GetReviewByIdProduct(int ID_Product)
        {
            return db.Reviews.Where(x => x.ProductId == ID_Product).ToList();
        }
    }
}
