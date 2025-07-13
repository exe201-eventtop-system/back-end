using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.System.APICall
{
    public class ApiCaller
    {
        private readonly HttpClient _httpClient;
        public ApiCaller(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetFromApiAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }

                var content = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HttpClient Error] {ex.Message}");
                return default;
            }
        }
        public async Task<T?> PutFromApiAsync<T>(string url, object data)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(data);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }

                var content = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[HttpClient PUT Error] {ex.Message}");
                return default;
            }
        }

    }

}
