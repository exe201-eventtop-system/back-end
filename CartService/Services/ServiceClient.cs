using Microsoft.Extensions.Logging;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ShareLibary.Model;

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
            var endpoint = $"services/{id}";
            Console.WriteLine("Calling API: " + _httpClient.BaseAddress + endpoint);

            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                Console.WriteLine("StatusCode: " + response.StatusCode);

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
    }

}
