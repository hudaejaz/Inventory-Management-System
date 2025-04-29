using InventoryManagementBLL.Interfaces;
using InventoryManagementDAL.Interfaces;
using InventoryManagementDAL.Models;
using InventoryManagementDAL.Repositories;
using InventoryManagementDTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagementBLL
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IProductRepository _productRepository;

        public SupplierService(ISupplierRepository supplierRepository, IProductRepository productRepository)
        {
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
        }

        public IEnumerable<SupplierDTO> GetAllSuppliers()
        {
            return _supplierRepository.GetAllSuppliers().Select(s => new SupplierDTO
            {
                SupplierID = s.SupplierID,
                SupplierName = s.SupplierName,
                ContactNumber = s.ContactNumber
            }).ToList();
        }

        public SupplierDTO GetSupplierById(int supplierId)
        {
            var supplier = _supplierRepository.GetSupplierById(supplierId);
            if (supplier == null)
                throw new ArgumentException("Supplier not found.");

            return new SupplierDTO
            {
                SupplierID = supplier.SupplierID,
                SupplierName = supplier.SupplierName,
                ContactNumber = supplier.ContactNumber
            };
        }

        public void AddSupplier(SupplierDTO supplierDTO)
        {
            if (_supplierRepository.GetSupplierByName(supplierDTO.SupplierName) != null)
                throw new ArgumentException("Supplier name must be unique.");
            if (supplierDTO.ContactNumber.Length < 10)
                throw new ArgumentException("Contact number must be at least 10 digits.");

            var supplier = new Supplier
            {
                SupplierName = supplierDTO.SupplierName,
                ContactNumber = supplierDTO.ContactNumber
            };

            _supplierRepository.AddSupplier(supplier);
        }

        public void UpdateSupplier(SupplierDTO supplierDTO)
        {
            var existingSupplier = _supplierRepository.GetSupplierById(supplierDTO.SupplierID);
            if (existingSupplier == null)
                throw new ArgumentException("Supplier not found.");
            var supplierWithSameName = _supplierRepository.GetSupplierByName(supplierDTO.SupplierName);
            if (supplierWithSameName != null && supplierWithSameName.SupplierID != supplierDTO.SupplierID)
                throw new ArgumentException("Supplier name must be unique.");

            existingSupplier.SupplierName = supplierDTO.SupplierName;
            existingSupplier.ContactNumber = supplierDTO.ContactNumber;

            _supplierRepository.UpdateSupplier(existingSupplier);
        }

        public void DeleteSupplier(int supplierId)
        {
            var supplier = _supplierRepository.GetSupplierById(supplierId);
            if (supplier == null)
                throw new ArgumentException("Supplier not found.");
            var productsLinked = _productRepository.GetAllProducts().Any(p => p.SupplierID == supplierId);
            if (productsLinked)
                throw new InvalidOperationException("Cannot delete supplier with linked products.");

            _supplierRepository.DeleteSupplier(supplierId);
        }

    }
}
