using Kolokwium.DTOs;

namespace Kolokwium.Repositories;

public interface IDeliveriesRepository
{
    // do  endpointu 1:
    Task<CustomerRequest> GetCustomer(int deliveryId);
    Task<DriverRequest> GetDriver(int deliveryId);
    Task<List<ProductRequest>> GetProductsForDelivery(int deliveryId);
    Task<DeliveryRequest> GetDelivery(int deliveryId);
    
    Task<bool> DoesDeliveryExist(int deliveryId); // do 1 i 2
    
    // do endpointu 2:
    Task<bool> DoesCustomerExist(int customerId);
    
    Task<bool> DoesDriverExist(string licenceNumber);
    Task<bool> DoesProductExist(string name);
    Task<int> GetDriverId(string licenceNumber);
    
    Task<bool> CreateDelivery(DeliveryDto deliveryDto);
    
}