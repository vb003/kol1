using System.Data.Common;
using Kolokwium.DTOs;
using Kolokwium.Repositories;
using Microsoft.Data.SqlClient;

namespace Kolokwium.Services;

public class DeliveriesService : IDeliveriesService
{
    private readonly IDeliveriesRepository _deliveriesRepository;

    public DeliveriesService(IDeliveriesRepository deliveriesRepository)
    {
        _deliveriesRepository = deliveriesRepository;
    }

    public async Task<DeliveryRequest> GetDelivery(int id)
    {
        if (await _deliveriesRepository.DoesDeliveryExist(id)== false)
        {
            throw new InvalidOperationException("Delivery does not exists");
        }
        
        var result = await _deliveriesRepository.GetDelivery(id);
        return result;
    }
    
    public async Task<bool> CreateDelivery(DeliveryDto deliveryDto)
    {
        if (await _deliveriesRepository.DoesDeliveryExist(deliveryDto.Id))
        {
            throw new InvalidOperationException("Delivery already exists");
        }

        if (await _deliveriesRepository.DoesCustomerExist(deliveryDto.CustomerId) == false)
        {
            throw new InvalidOperationException("Customer not found");
        }
        
        if (await _deliveriesRepository.DoesDriverExist(deliveryDto.LicenceNumber) == false)
        {
            throw new InvalidOperationException("Driver not found");
        }
        
        foreach (ProductDto product in deliveryDto.Products){
            if (await _deliveriesRepository.DoesProductExist(product.Name) == false)
                throw new InvalidOperationException("Product not found");
        }
        
        bool result = await _deliveriesRepository.CreateDelivery(deliveryDto);
        return result;
    }
    
}