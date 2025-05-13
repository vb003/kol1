using System.Data.Common;
using Kolokwium.DTOs;
using Microsoft.Data.SqlClient;

namespace Kolokwium.Repositories;

public class DeliveriesRepository : IDeliveriesRepository
{
      //  private readonly string _connectionString = "Data Source=localhost, 1433; User=sa; Password=yourStrong(!)Password; Integrated Security=False;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False";
      private readonly string _connectionString;
  
    public DeliveriesRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    public async Task<CustomerRequest> GetCustomer(int deliveryId)
    {
        var customer = new CustomerRequest();
        
        string command = @"SELECT Customer.first_name, Customer.last_name,Customer.date_of_birth 
                    FROM Customer INNER JOIN Delivery ON Customer.customer_id = Delivery.customer_id
                    WHERE Delivery.delivery_id  = @deliveryId"; 
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@deliveryId", deliveryId);
            
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    customer.firstName = reader.GetString(reader.GetOrdinal("first_name"));
                    customer.lastName = reader.GetString(reader.GetOrdinal("last_name"));
                    customer.dateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth"));
                }
            }
        }

        return customer;
    }
    public async Task<DriverRequest> GetDriver(int deliveryId)
    {
        var driver = new DriverRequest();
        
        string command = @"SELECT Driver.first_name, Driver.last_name, Driver.licence_number 
                    FROM Driver INNER JOIN Delivery ON Driver.driver_id = Delivery.driver_id
                    WHERE Delivery.delivery_id  = @deliveryId"; 
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@deliveryId", deliveryId);
            
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    driver.firstName = reader.GetString(reader.GetOrdinal("first_name"));
                    driver.lastName = reader.GetString(reader.GetOrdinal("last_name"));
                    driver.licenceNumber= reader.GetString(reader.GetOrdinal("licence_number"));
                }
            }
        }

        return driver;
    }

    public async Task<List<ProductRequest>> GetProductsForDelivery(int deliveryId)
    {
        var products = new List<ProductRequest>();
        
        string command = @"SELECT Product.name, Product.price, Product_Delivery.amount FROM Product 
                    INNER JOIN Product_Delivery ON Product_Delivery.product_id = Product.product_id
                    WHERE Product_Delivery.delivery_id  = @deliveryId"; 
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@deliveryId", deliveryId);
            
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    products.Add(new ProductRequest()
                    {
                        name = reader.GetString(reader.GetOrdinal("name")),
                        price = reader.GetDouble(reader.GetOrdinal("price")),
                        amount = reader.GetInt32(reader.GetOrdinal("amount"))
                    });
                }
            }
        }

        return products;
        
    }
    public async Task<DeliveryRequest> GetDelivery(int deliveryId)
    {
        var delivery = new DeliveryRequest();
        
        string command = @"SELECT date FROM Delivery WHERE delivery_id = @id"; 
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", deliveryId);
            
            await conn.OpenAsync();
            
            DateTime date = (DateTime) await cmd.ExecuteScalarAsync();
            delivery.date = date;
        }

        return delivery;
    }
    
    public async Task<bool> DoesDeliveryExist(int deliveryId)
    {
        string command = @"SELECT COUNT(*) FROM Delivery WHERE delivery_id = @deliveryId";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@deliveryId", deliveryId);
            
            await conn.OpenAsync();
            
            int count = (int) await cmd.ExecuteScalarAsync();
            return count > 0;
        }
    }

    public async Task<bool> DoesCustomerExist(int customerId)
    {
        string command = @"SELECT COUNT(*) FROM Customer WHERE customer_id = @customerId";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@customerId", customerId);
            
            await conn.OpenAsync();
            
            int count = (int) await cmd.ExecuteScalarAsync();
            return count > 0;
        }
    }

    public async Task<bool> DoesDriverExist(string licenceNumber)
    {
        string command = @"SELECT COUNT(*) FROM Driver WHERE licence_number = @licenceNumber";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@licenceNumber", licenceNumber);
            
            await conn.OpenAsync();
            
            int count = (int) await cmd.ExecuteScalarAsync();
            return count > 0;
        }
    }

    public async Task<bool> DoesProductExist(string name)
    {
        string command = @"SELECT COUNT(*) FROM Product WHERE name = @name";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@name", name);
            
            await conn.OpenAsync();
            
            int count = (int) await cmd.ExecuteScalarAsync();
            return count > 0;
        }
    }

    public async Task<int> GetDriverId(string licenceNumber)
    {
        string command = @"SELECT driver_id FROM Driver WHERE licence_number = @licenceNumber";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@licenceNumber", licenceNumber);
            
            await conn.OpenAsync();
            
            int id = (int) await cmd.ExecuteScalarAsync();
            return id;
        }
    }
    
    public async Task<bool> CreateDelivery(DeliveryDto deliveryDto)
    {
        string command = @"insert into Delivery (delivery_id, customer_id, driver_id, date)
                      values (@deliveryId, @customerId, @driverId, @date)"; 
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@deliveryId", deliveryDto.Id);
            cmd.Parameters.AddWithValue("@customerId", deliveryDto.CustomerId);

            await conn.OpenAsync();
            
            // Znajdź id Drivera:
            int driverId =  await GetDriverId(deliveryDto.LicenceNumber);
            cmd.Parameters.AddWithValue("@driverId", driverId);
            
            cmd.Parameters.AddWithValue("@date", DateTime.Now);

            await cmd.ExecuteNonQueryAsync();
            
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
    }
}