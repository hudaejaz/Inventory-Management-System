using InventoryManagementBLL;
using InventoryManagementBLL.Interface;
using InventoryManagementDAL;
using InventoryManagementDAL.Interfaces;
using InventoryManagementDAL.Models;
using InventoryManagementDAL.Repositories;
using InventoryManagementDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InventoryManagement;Integrated Security=True"))
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>()
            .BuildServiceProvider();

        var productService = serviceProvider.GetRequiredService<IProductService>();
        var transactionService = serviceProvider.GetRequiredService<IInventoryTransactionRepository>();

        while (true)
        {
            Console.WriteLine("\n****** Inventory Management System ******");
            Console.WriteLine("1. List All Products");
            Console.WriteLine("2. Add Product");
            Console.WriteLine("3. Update Product");
            Console.WriteLine("4. Delete Product");
            Console.WriteLine("5. Process Inventory Transaction (IN/OUT)");
            Console.WriteLine("6. View Low Stock Alerts");
            Console.WriteLine("7. Search Products");
            Console.WriteLine("8. Exit");
            Console.Write("Enter your choice: ");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input! Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    ListAllProducts(productService);
                    break;
                case 2:
                    AddProduct(productService);
                    break;
                case 3:
                    UpdateProduct(productService);
                    break;
                case 4:
                    DeleteProduct(productService);
                    break;
                case 5:
                    ProcessTransaction(transactionService);
                    break;
                case 6:
                    ViewLowStockAlerts(productService);
                    break;
                case 7:
                    SearchProducts(productService);
                    break;
                case 8:
                    Console.WriteLine("Exiting the system. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }
        }
    }

    static void ListAllProducts(IProductService productService)
    {
        Console.WriteLine("\n****** Product List ******");
        var products = productService.GetAllProducts();
        if (!products.Any())
        {
            Console.WriteLine("No products found.");
            return;
        }

        foreach (var product in products)
        {
            Console.WriteLine($"ID: {product.ProductID}, Name: {product.ProductName}, " +
                $"Category: {product.Category}, Price: {product.Price}, Stock: {product.StockQuantity}");
        }
    }

    static void AddProduct(IProductService productService)
    {
        try
        {
            Console.Write("\nEnter Product Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Category: ");
            string category = Console.ReadLine();
            Console.Write("Enter Price: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Enter Stock Quantity: ");
            int stock = int.Parse(Console.ReadLine());
            Console.Write("Enter Supplier ID: ");
            int supplierId = int.Parse(Console.ReadLine());

            var newProduct = new ProductDTO
            {
                ProductName = name,
                Category = category,
                Price = price,
                StockQuantity = stock,
                SupplierID = supplierId
            };

            productService.AddProduct(newProduct);
            Console.WriteLine("Product added successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void UpdateProduct(IProductService productService)
    {
        try
        {
            Console.Write("\nEnter Product ID to update: ");
            int productId = int.Parse(Console.ReadLine());

            var existingProduct = productService.GetProduct(productId);
            if (existingProduct == null)
            {
                Console.WriteLine("Product not found!");
                return;
            }

            Console.Write("Enter New Product Name (Leave blank to keep current): ");
            string newName = Console.ReadLine();
            Console.Write("Enter New Price (Leave blank to keep current): ");
            string newPrice = Console.ReadLine();
            Console.Write("Enter New Stock Quantity (Leave blank to keep current): ");
            string newStock = Console.ReadLine();

            existingProduct.ProductName = string.IsNullOrEmpty(newName) ? existingProduct.ProductName : newName;
            existingProduct.Price = string.IsNullOrEmpty(newPrice) ? existingProduct.Price : decimal.Parse(newPrice);
            existingProduct.StockQuantity = string.IsNullOrEmpty(newStock) ? existingProduct.StockQuantity : int.Parse(newStock);

            productService.UpdateProduct(existingProduct);
            Console.WriteLine("Product updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void DeleteProduct(IProductService productService)
    {
        try
        {
            Console.Write("\nEnter Product ID to delete: ");
            int productId = int.Parse(Console.ReadLine());

            var existingProduct = productService.GetProduct(productId);
            if (existingProduct == null)
            {
                Console.WriteLine("Product not found!");
                return;
            }

            productService.DeleteProduct(productId);
            Console.WriteLine("Product deleted successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ProcessTransaction(IInventoryTransactionRepository transactionService)
    {
        try
        {
            Console.Write("\nEnter Product ID: ");
            int productId = int.Parse(Console.ReadLine());
            Console.Write("Transaction Type (IN/OUT): ");
            string transactionType = Console.ReadLine().ToUpper();
            Console.Write("Enter Quantity: ");
            int quantity = int.Parse(Console.ReadLine());

            transactionService.ProcessTransaction(new InventoryTransaction
            {
                ProductID = productId,
                TransactionType = transactionType,
                Quantity = quantity
            });

            Console.WriteLine("Transaction Processed Successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ViewLowStockAlerts(IProductService productService)
    {
        var lowStockProducts = productService.GetAllProducts().Where(p => p.StockQuantity < 5);

        if (!lowStockProducts.Any())
        {
            Console.WriteLine("No Low Stock Alerts!");
            return;
        }

        Console.WriteLine("\n Low Stock Products");
        foreach (var product in lowStockProducts)
        {
            Console.WriteLine($"ID: {product.ProductID}, Name: {product.ProductName}, Stock: {product.StockQuantity}");
        }
    }

    static void SearchProducts(IProductService productService)
    {
        Console.Write("\nEnter search term (Product Name, Category, or Supplier): ");
        string searchTerm = Console.ReadLine().ToLower();

        var results = productService.GetAllProducts().Where(p =>
            p.ProductName.ToLower().Contains(searchTerm) ||
            p.Category.ToLower().Contains(searchTerm));

        if (!results.Any())
        {
            Console.WriteLine("No matching products found.");
            return;
        }

        Console.WriteLine("\n ****** Search Results ******");
        foreach (var product in results)
        {
            Console.WriteLine($"ID: {product.ProductID}, Name: {product.ProductName}, Category: {product.Category}, Price: {product.Price}, Stock: {product.StockQuantity}");
        }
    }
}
