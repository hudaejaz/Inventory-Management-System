using InventoryManagementDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementDAL.Interfaces
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        Product GetProduct(int id);
        List<Product> GetAllProducts();
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }

}
