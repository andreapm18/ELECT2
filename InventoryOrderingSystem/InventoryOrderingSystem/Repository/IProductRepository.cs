using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryOrderingSystem.Models;

namespace InventoryOrderingSystem.Repository.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task UpdateAsync(Product product);
}