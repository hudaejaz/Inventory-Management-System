using InventoryManagementBLL.Interface;
using InventoryManagementDAL;
using InventoryManagementDAL.Interfaces;
using InventoryManagementDAL.Models;
using InventoryManagementDTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementBLL
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IInventoryTransactionRepository _transactionRepository; 

        public ProductService(IProductRepository repository, IInventoryTransactionRepository transactionRepository)
        {
            _repository = repository;
            _transactionRepository = transactionRepository;
        }

        public void AddProduct(ProductDTO productDto)
        {
            if (productDto.Price <= 0 || productDto.StockQuantity < 0)
                throw new ArgumentException("Invalid product details.");

            var product = new Product
            {
                ProductName = productDto.ProductName,
                Category = productDto.Category,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                SupplierID = productDto.SupplierID
            };

            _repository.AddProduct(product);

            if (productDto.StockQuantity > 0)
            {
                ProcessStockTransaction(product.ProductID, productDto.StockQuantity, "IN");
            }
        }


        public void UpdateProduct(ProductDTO productDto)
        {
            var product = _repository.GetProduct(productDto.ProductID);
            if (product == null) throw new Exception("Product not found.");

            int quantityChange = productDto.StockQuantity - product.StockQuantity;

            product.ProductName = productDto.ProductName;
            product.Category = productDto.Category;
            product.Price = productDto.Price;
            product.StockQuantity = productDto.StockQuantity;

            _repository.UpdateProduct(product);
        }

        public void DeleteProduct(int id)
        {
            var product = _repository.GetProduct(id);
            if (product == null) throw new Exception("Product not found.");

            if (product.StockQuantity > 0)
            {
                ProcessStockTransaction(id, product.StockQuantity, "OUT");
            }

            _repository.DeleteProduct(id);
        }


        public ProductDTO? GetProduct(int id)
        {
            var product = _repository.GetProduct(id);
            return product == null ? null : new ProductDTO
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Category = product.Category,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                SupplierID = product.SupplierID
            };
        }

        public List<ProductDTO> GetAllProducts()
        {
            return _repository.GetAllProducts().Select(p => new ProductDTO
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Category = p.Category,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                SupplierID = p.SupplierID
            }).ToList();
        }
        public void ProcessStockTransaction(int productId, int quantity, string transactionType)
        {
            var product = _repository.GetProduct(productId);
            if (product == null)
                throw new Exception("Product not found.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (transactionType == "OUT" && product.StockQuantity < quantity)
                throw new InvalidOperationException("Insufficient stock.");

            if (transactionType == "IN")
            {
                product.StockQuantity += quantity;
            }
            else if (transactionType == "OUT")
            {
                product.StockQuantity -= quantity;
            }
            else
            {
                throw new ArgumentException("Invalid transaction type. Use 'IN' or 'OUT'.");
            }

            _repository.UpdateProduct(product);

            var transaction = new InventoryTransaction
            {
                ProductID = product.ProductID,
                Quantity = quantity,
                TransactionType = transactionType,
                TransactionDate = DateTime.UtcNow
            };

            _transactionRepository.AddTransaction(transaction);
        }

    }
}
