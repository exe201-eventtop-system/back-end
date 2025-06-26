using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Services.DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class GeminiClient : IEventScriptGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly string _geminiUrl;

        // ✅ Đặt key và Gemini URI tại đây


        public GeminiClient(HttpClient httpClient, IOptions<GeminiSettings> settings)
        {
            _httpClient = httpClient;
            _geminiUrl = $"{settings.Value.BaseUrl}/models/{settings.Value.Model}:generateContent?key={settings.Value.ApiKey}";
        }

        public async Task<string> GenerateScriptAsync(string userInput)
        {
            var prompt = @$"
                Người dùng vừa nhập nội dung sau: {userInput}

                Dựa vào nội dung trên, hãy phân tích và tạo ra một kế hoạch tổ chức sự kiện **đầy đủ, rõ ràng, chi tiết** theo đúng cấu trúc JSON sau:

                {{
                  ""eventName"": string,
                  ""eventDate"": string, // đúng định dạng dd/MM/yyyy
                  ""location"": string,
                  ""expectedParticipants"": string,
                  ""themeColor"": string,  // Gồm 3 màu chính đại diện cho sự kiện hài hòa và hợp với nhau
                  ""budget"": string,
                  ""description"": string, // Viết chi tiết toàn bộ kế hoạch
                  ""eventType"": string
                }}

                Yêu cầu phần`description phải thực sự chi tiết và logic. Bao gồm:
                - Lý do chọn **ngày tổ chức**, **giờ tổ chức**, **địa điểm**.
                - Vì sao chọn **3 màu sắc** đại diện cho sự kiện (ghi rõ từng màu và ý nghĩa).
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

            // Log nội dung trả về
            Console.WriteLine("Gemini Response:");
            Console.WriteLine(json);


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

        public string ExtractJson(string responseText)
        {
            // Gỡ bỏ các thẻ markdown nếu có
            responseText = responseText.Replace("```json", "").Replace("```", "").Trim();

            var match = Regex.Match(responseText, @"\{(?:[^{}]|(?<open>\{)|(?<-open>\}))+(?(open)(?!))\}");

            if (!match.Success)
                throw new Exception("Không tìm thấy JSON trong phản hồi AI.");

            return match.Value;
        }
    }
}