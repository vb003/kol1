using System.ComponentModel.DataAnnotations;

namespace Kolokwium.DTOs;

public class DeliveryRequest
{
    public DateTime date { get; set; }
    public CustomerRequest customer { get; set; }
    public DriverRequest driver { get; set; }
    public List<ProductRequest> Products { get; set; }
}

public class CustomerRequest
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public DateTime dateOfBirth { get; set; }
}

public class DriverRequest
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string licenceNumber { get; set; }
}

public class ProductRequest
{
    public string name { get; set; }
    public double price { get; set; }
    public int amount { get; set; }
}