using Kolokwium.DTOs;
using Kolokwium.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Kolokwium.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveriesController : ControllerBase
{
    private readonly IDeliveriesService _deliveriesService;

    public DeliveriesController(IDeliveriesService deliveriesService)
    {
        _deliveriesService = deliveriesService;
    }
    
    // Endpoint 1:
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDelivery(int id)
    {
        try
        {
            var result = await _deliveriesService.GetDelivery(id);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occured: "+ ex.Message);
        }
    }
    
    // Endpoint 2: Dodaj dostawę:
    [HttpPost]
    public async Task<IActionResult> CreateDelivery([FromBody] DeliveryDto deliveryDto)
    {
        try
        {
            var result = await _deliveriesService.CreateDelivery(deliveryDto);
            if (result) 
                return Ok("Dodano dostawe");
            else
            {
                return BadRequest("Nie udalo sie dodac dostawy");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occured: " + ex.Message);
        }
        
    }
}