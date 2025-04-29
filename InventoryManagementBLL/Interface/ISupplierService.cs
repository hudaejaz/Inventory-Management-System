using InventoryManagementDTOs;
using System.Collections.Generic;

namespace InventoryManagementBLL.Interfaces
{
    public interface ISupplierService
    {
        IEnumerable<SupplierDTO> GetAllSuppliers();
        SupplierDTO GetSupplierById(int supplierId);
        void AddSupplier(SupplierDTO supplierDTO);
        void UpdateSupplier(SupplierDTO supplierDTO);
        void DeleteSupplier(int supplierId);
    }
}
