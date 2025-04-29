using InventoryManagementDAL.Interfaces;
using InventoryManagementDAL.Models;
using System;

namespace InventoryManagementDAL.Repositories
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly InventoryDbContext _context;
        private readonly IProductRepository _productRepository;

        public InventoryTransactionRepository(InventoryDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }

        public void AddTransaction(InventoryTransaction transaction)
        {
            _context.InventoryTransactions.Add(transaction);
            _context.SaveChanges();
        }

        public void ProcessTransaction(InventoryTransaction transaction)
        {
            var product = _productRepository.GetProduct(transaction.ProductID);
            if (product == null)
                throw new ArgumentException("Invalid Product ID.");

            if (transaction.TransactionType == "IN")
            {
                product.StockQuantity += transaction.Quantity;
            }
            else if (transaction.TransactionType == "OUT")
            {
                if (product.StockQuantity < transaction.Quantity)
                    throw new InvalidOperationException("Not enough stock available.");

                product.StockQuantity -= transaction.Quantity;
            }
            else
            {
                throw new ArgumentException("Invalid Transaction Type (must be IN or OUT).");
            }

            _context.InventoryTransactions.Add(transaction);
            _productRepository.UpdateProduct(product);
            _context.SaveChanges();
        }
    }
}
