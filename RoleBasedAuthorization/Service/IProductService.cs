using RoleBasedAuthorization.Model;
using System.Collections.Generic;


namespace RoleBasedAuthorization.Service
{
    public interface IProductService
    {

        IEnumerable<Product> getAllProduct();

        Product GetProduct(int id);

        Product CreateProduct(Product product);

        Product EditProduct(Product product);

        void DeleteProduct(Product product);

        IEnumerable<Product> GetTopSeller();

        // IEnumerable<Product> searchUser(string name, string phone);

        //  IEnumerable<Product> filterUserByRole(string role);

        bool CheckExistProperties(string name);
    }
}
