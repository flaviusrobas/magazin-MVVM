using Magazin.Library.Models;

namespace Magazin.Library.DataAccess
{
    public interface IProductData
    {
        ProductModel GetProductById(int productId);
        List<ProductModel> GetProductsById();
    }
}