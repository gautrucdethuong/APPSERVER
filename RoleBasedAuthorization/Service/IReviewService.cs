using RoleBasedAuthorization.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAuthorization.Service
{
    public interface IReviewService
    {
        public bool CrateReview(Review review);

        public Review EditReview(Review review);

        public void DeleteReview(Review review);

        public IEnumerable<Review> GetAllReview();

        public Review GetReview(int id);

        //public bool CheckCodeProductUserExist(int ProductId, int UserId);

        public IEnumerable<Review> GetReviewByIdProduct(int ID_Product);
    }
}
