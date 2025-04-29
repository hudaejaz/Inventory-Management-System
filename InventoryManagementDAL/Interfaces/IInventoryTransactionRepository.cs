using InventoryManagementDAL.Models;
using InventoryManagementDTOs;
using System.Collections.Generic;

namespace InventoryManagementDAL.Interfaces
{
    public interface IInventoryTransactionRepository
    {
        void AddTransaction(InventoryTransaction transaction);
        void ProcessTransaction(InventoryTransaction transaction);
    }
}
