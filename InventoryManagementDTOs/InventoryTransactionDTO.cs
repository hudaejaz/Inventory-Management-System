using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementDTOs
{
    public class InventoryTransactionDTO
    {
        public int TransactionID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string TransactionType { get; set; } 
        public int Quantity { get; set; }
        public string TransactionDate { get; set; } 
    }
}
