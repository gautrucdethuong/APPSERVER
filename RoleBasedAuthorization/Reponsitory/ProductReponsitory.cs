using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Model;
using RoleBasedAuthorization.Service;
using System.Collections.Generic;
using System.Linq;


namespace RoleBasedAuthorization.Reponsitory
{

    public class ProductReponsitory : IProductService
    {
        private DBContext db;

        public ProductReponsitory(DBContext dbContext)
        {
            db = dbContext;
        }

        public bool CheckExistProperties(string name)
        {
            if (db.Products.Any(x => x.product_name == name))
            {
                return false;
            }
            return true;
        }


        public Product CreateProduct(Product product)
        {
            db.Add(product);
            db.SaveChanges();
            return product;
        }

        public void DeleteProduct(Product product)
        {
            db.Products.Remove(product);
            db.SaveChanges();
        }


        public Product EditProduct(Product product)
        {
            var editproduct = db.Products.Find(product.product_id);

            if (editproduct != null)
            {
                editproduct.product_name = product.product_name;
                editproduct.product_origin = product.product_origin;
                editproduct.product_price = product.product_price;
                editproduct.product_size = product.product_size;
                editproduct.product_description = product.product_description;
                editproduct.product_rating = product.product_rating;
                db.SaveChanges();
            }
            return product;
        }

        public IEnumerable<Product> getAllProduct()
        {
            return db.Products.ToList();
        }

        public Product GetProduct(int id)
        {
            var product = db.Products.Find(id);
            return product;
        }

        public IEnumerable<Product> GetTopSeller()
        {
            var items = db.Products.OrderByDescending(u => u.product_price).Take(6).ToList();
            return items;
        }
    }
}
