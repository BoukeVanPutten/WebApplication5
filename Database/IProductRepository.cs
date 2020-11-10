using Models;
using System.Collections.Generic;

namespace Database
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        void DeleteProduct(string productCode);
        void EditProduct(Product product);
        IEnumerable<Product> GetAllProducts(SortMethod? method, int page, int pageSize);
        Product GetProduct(string productCode);
        IEnumerable<Product> GetProductsByColour(Colour colour, SortMethod? method, int page, int pageSize);
    }
}