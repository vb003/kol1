using Kolokwium.DTOs;

namespace Kolokwium.Services;

public interface IDeliveriesService
{
    Task<DeliveryRequest> GetDelivery(int id);
    Task<bool> CreateDelivery(DeliveryDto deliveryDto);
}