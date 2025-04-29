using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementDAL.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }

        [Required]
        public string SupplierName { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Contact number must be at least 10 digits.")]
        public string ContactNumber { get; set; }
    }

}
