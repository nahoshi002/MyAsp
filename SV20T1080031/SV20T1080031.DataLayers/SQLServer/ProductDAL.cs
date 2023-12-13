using SV20T1080031.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080031.DataLayers.SQLServer
{
    public class ProductDAL : _BaseDAL, IProductDAL
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Product data)
        {
            throw new NotImplementedException();
        }

        public long AddAttribute(ProductAttribute data)
        {
            throw new NotImplementedException();
        }

        public long AddPhoto(ProductPhoto data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int productID)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAttribute(long attributeID)
        {
            throw new NotImplementedException();
        }

        public bool DeletePhoto(long photoID)
        {
            throw new NotImplementedException();
        }

        public Product? Get(int productID)
        {
            throw new NotImplementedException();
        }

        public ProductAttribute? GetAttribute(long attributeID)
        {
            throw new NotImplementedException();
        }

        public ProductPhoto? GetPhoto(long photoID)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int productID)
        {
            throw new NotImplementedException();
        }

        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            throw new NotImplementedException();
        }

        public IList<ProductAttribute> ListAttributes(int productID)
        {
            throw new NotImplementedException();
        }

        public IList<ProductPhoto> ListPhotos(int productID)
        {
            throw new NotImplementedException();
        }

        public bool Update(Product data)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAttribute(ProductAttribute data)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
            throw new NotImplementedException();
        }
    }
}
