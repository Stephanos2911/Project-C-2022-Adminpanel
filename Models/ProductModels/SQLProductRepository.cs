﻿using AdminApplication.Models;

namespace Project_C.Models.ProductModels
{
    public class SQLProductRepository : IProductRepository
    {
        private ApplicationDbContext _context;
        public SQLProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public Product AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public void DeleteProduct(Guid id)
        {
            Product product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products;
        }

        public Product GetProduct(Guid id)
        {
            return _context.Products.Find(id);
        }

        public Product UpdateProduct(Product productChanges)
        {
            var product = _context.Products.Attach(productChanges);
            product.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return productChanges;
        }
    }
}
