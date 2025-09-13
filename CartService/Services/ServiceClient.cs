using Google.Apis.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.DTOs;
using ShareLibary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class ServiceClient
    {
        private readonly HttpClient _httpClient;
        public ServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceDTO?> GetServiceByIdAsync(Guid id)
        {
           
            try
            {
                var response = await _httpClient.GetAsync($"api/services/{id}");
                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync();
                var baseResponse = JsonSerializer.Deserialize<ApiResponse<ServiceDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (baseResponse is { Success: true, Data: not null })
                {
                    return baseResponse.Data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling service: {ex.Message}");
            }

            return null;
        }
        public async Task<ProductDetailCustomer?> GetCustomerByIdAsync(Guid id)
        {
            try
            {
                // Nếu BaseAddress đang trỏ tới Service, bạn phải build URL đầy đủ
                var response = await _httpClient.GetAsync($"api/user/{id}");

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<ProductDetailCustomer>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthService] GetCustomerByIdAsync Error: {ex.Message}");
                return null;
            }
        }

    }

}
