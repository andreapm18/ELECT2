using System;
using System.Collections.Generic;

namespace InventoryOrderingSystem.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerCode { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
