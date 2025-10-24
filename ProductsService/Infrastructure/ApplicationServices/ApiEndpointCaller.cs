using Application.Commons.Interfaces.ApiCaller;
using System.Net.Http.Json;

namespace Infrastructure.Services
{
    public class ApiEndpointCaller : IApiEndpointCaller
    {
        private readonly HttpClient httpClient;

        public ApiEndpointCaller()
        {
            httpClient = new HttpClient();
        }

        public async Task<TResult?> GetAsync<TResult>(string uri)
        {
            var response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResult>();
            }

            throw new HttpRequestException($"Request data from {uri} failed with status code {response.StatusCode}.({response.Version.ToString()})", null, response.StatusCode);
        }

        public async Task<TResult?> PostAsync<TResult>(string uri, object body)
        {

            var response = await httpClient.PostAsJsonAsync(uri, body);

            if (response.IsSuccessStatusCode)
            {
                //var body_string = await response.Content.ReadAsStringAsync();
                //Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(body_string));
                //return await JsonSerializer.DeserializeAsync<TResult>(stream);

                return await response.Content.ReadFromJsonAsync<TResult>();
            }

            throw new HttpRequestException($"Request data from {uri} failed with status code {response.StatusCode}.(HTTP {response.Version.ToString()})", null, response.StatusCode);
        }

        public async Task<TResult?> PutAsync<TResult>(string uri, object body)
        {
            var response = await httpClient.PutAsJsonAsync(uri, body);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResult>();
            }

            throw new HttpRequestException($"Request data from {uri} failed with status code {response.StatusCode}.({response.Version.ToString()})", null, response.StatusCode);
        }

        public async Task<TResult?> DeleteAsync<TResult>(string uri)
        {
            var response = await httpClient.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TResult>();
            }

            throw new HttpRequestException($"Request data from {uri} failed with status code {response.StatusCode}.({response.Version.ToString()})", null, response.StatusCode);

        }
    }
}
