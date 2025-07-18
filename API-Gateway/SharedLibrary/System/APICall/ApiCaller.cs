using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        public async Task<TResult?> PostToAsync<TResult>(string url, object data)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };

            var requestBody = JsonSerializer.Serialize(data, options);

            try
            {
                using (var response = await _httpClient.PostAsJsonAsync(url, requestBody))
                {
                    response.EnsureSuccessStatusCode();

                    return JsonSerializer.Deserialize<TResult>(await response.Content.ReadAsStringAsync(), options);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Http Client]: {ex.Message}\nStack trace:\n{ex.StackTrace}");
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
