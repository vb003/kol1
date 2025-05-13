using System.ComponentModel.DataAnnotations;

namespace Kolokwium.DTOs;

public class DeliveryDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    [StringLength(17)]
    public string LicenceNumber { get; set; }
    public List<ProductDto> Products { get; set; }
}

public class ProductDto
{
    [StringLength(100)]
    public string Name { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Amount { get; set; }
}