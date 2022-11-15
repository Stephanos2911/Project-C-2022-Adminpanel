namespace Project_C.Models.ProductModels
{
    public interface IProductRepository
    {
        Product GetProduct(Guid id);
        IEnumerable<Product> GetAllProducts();

        Product AddProduct(Product product);
        Product UpdateProduct(Product product);
        void DeleteProduct(Guid id);
    }
}
