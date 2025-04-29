using InventoryManagementDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace InventoryManagementBLL.Interface
{
    public interface IProductService
    {
        void AddProduct(ProductDTO product);
        ProductDTO GetProduct(int id);
        List<ProductDTO> GetAllProducts();
        void UpdateProduct(ProductDTO product);
        void DeleteProduct(int id);
    }

}
