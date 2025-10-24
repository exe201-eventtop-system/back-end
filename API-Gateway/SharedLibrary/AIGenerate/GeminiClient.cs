using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedLibrary.AIGenerate
{
    public class GeminiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _geminiUrl;

        public GeminiClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            var baseUrl = config["GEMINI:BASE_URL"];
            var model = config["GEMINI:MODEL"];
            var apiKey = config["GEMINI:API_KEY"];
            _geminiUrl = $"{baseUrl}/models/{model}:generateContent?key={apiKey}";
        }

        public async Task<string> GenerateScriptAsync(string userInput)
        {
            var prompt = @$"
                Người dùng vừa nhập nội dung sau: {userInput}

                Dựa vào nội dung trên, hãy phân tích và tạo ra một kế hoạch tổ chức sự kiện **đầy đủ, rõ ràng, chi tiết** theo đúng cấu trúc JSON sau:

                {{
                  ""name"": string,
                  ""eventDate"": string, // định dạng dd/MM/yyyy — bắt buộc là ngày TƯƠNG LAI (>= ngày hôm nay) - Ngày tổ chức phải là một ngày hợp lý trong tương lai (không được nhỏ hơn ngày hôm nay).
                  ""location"": string,
                  ""expectedParticipants"": string,
                  ""themeColor"": string,  // Gồm 1 màu chính đại diện cho sự kiện hài hòa và hợp với nhau
                  ""budget"": string,
                  ""description"": string, // Viết chi tiết toàn bộ kế hoạch
                  ""eventType"": string
                }}

                Yêu cầu phần`description phải thực sự chi tiết và logic. Bao gồm:
                - Lý do chọn **ngày tổ chức**, **giờ tổ chức**, **địa điểm**.
                - Vì sao chọn **1 màu sắc** đại diện cho sự kiện (ghi rõ từng màu và ý nghĩa).
                - Danh sách **các hoạt động chính** trong sự kiện (ít nhất 2–3 hoạt động).
                - Các **bước thực hiện sự kiện theo thời gian cụ thể**.
                - Các **lưu ý quan trọng** trong khâu chuẩn bị, vận hành và sau khi kết thúc.
                - Sử dụng từ ngữ đơn giản, rõ ràng để người dùng có thể dễ dàng triển khai kế hoạch.

                **Chỉ trả về kết quả JSON thuần, không bao quanh bằng dấu ``` hoặc giải thích thêm.**
                ";

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_geminiUrl),
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Gemini API Error: {response.StatusCode} - {json}");


            //// Parse JSON an toàn
            //using var document = JsonDocument.Parse(json);
            //if (!document.RootElement.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
            //    throw new Exception("Không tìm thấy kết quả từ Gemini API");

            using var document = JsonDocument.Parse(json);
            var parts = document.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts");

            var responseText = string.Join("\n", parts.EnumerateArray()
                .Select(p => p.GetProperty("text").GetString()));

            return ExtractJson(responseText);
        }
        public async Task<string> ChatWithGeminiAsync(string userInput)
        {
            var payload = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = userInput }
                }
            }
        }
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_geminiUrl),
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Gemini API Error: {response.StatusCode} - {json}");

            using var document = JsonDocument.Parse(json);
            var parts = document.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts");

            var responseText = string.Join("\n", parts.EnumerateArray()
                .Select(p => p.GetProperty("text").GetString()));

            return responseText;
        }

        public string ExtractJson(string responseText)
        {
            responseText = responseText.Replace("```json", "").Replace("```", "").Trim();

            var match = Regex.Match(responseText, @"\{(?:[^{}]|(?<open>\{)|(?<-open>\}))+(?(open)(?!))\}");

            if (!match.Success)
                throw new Exception("Không tìm thấy JSON trong phản hồi AI.");

            return match.Value;
        }
    }
}