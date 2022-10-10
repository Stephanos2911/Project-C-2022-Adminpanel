using System.Security.Cryptography.X509Certificates;

namespace AdminApplication.Models
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _productList;

        public ProductRepository()
        {
            _productList = new List<Product>();
        }

        public Product AddProduct(Product product)
        {
            product.Id = _productList.Max(x => x.Id) + 1;
            _productList.Add(product);
            return product;
        }

        public void DeleteProduct(int id)
        {
            Product product = _productList.FirstOrDefault(x => x.Id == id);
            if(product != null)
            {
                _productList.Remove(product);
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productList;
        }

        public Product GetProduct(int id)
        {
            return _productList.FirstOrDefault(x => x.Id == id);
        }

        public Product UpdateProduct(Product productChanges)
        {
            Product updateProduct = _productList.FirstOrDefault(x => x.Id == productChanges.Id);
            if (updateProduct != null)
            {
                updateProduct.Name = productChanges.Name;
                updateProduct.Description = productChanges.Description;
                updateProduct.Price = productChanges.Price;
                updateProduct.Place = productChanges.Place;
                updateProduct.VideoLink = productChanges.VideoLink;
                return updateProduct;
            }
            return null;
        }
    }
}
