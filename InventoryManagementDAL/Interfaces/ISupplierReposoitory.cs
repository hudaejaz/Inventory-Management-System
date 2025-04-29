using InventoryManagementDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementDAL.Interfaces
{
    public interface ISupplierRepository
    {
        Supplier GetSupplierById(int id);
        Supplier GetSupplierByName(string name);
        void AddSupplier(Supplier supplier);
        IEnumerable<Supplier> GetAllSuppliers();
        void UpdateSupplier(Supplier supplier);
        void DeleteSupplier(int id);
    }
}
