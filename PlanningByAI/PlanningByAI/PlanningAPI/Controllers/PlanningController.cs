using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Repositories;
using Services.Commons;
using Services.DTOs;
using Services.Interfaces;
using ShareLibary.Model;
using Services.Implementations;
using Microsoft.Identity.Client;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Globalization;

namespace PlanningAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanningController : BaseController
    {
        


        private readonly IEventScriptGenerator _ai;
        private readonly PlanningRepository _repo;

        public PlanningController(IEventScriptGenerator ai, PlanningRepository repo)
        {
            _ai = ai;
            _repo = repo;
        }

        [HttpPost("generate")]
        [ProducesResponseType<ApiResponse<PlanningResponseDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GeneratePlanning([FromBody] string userInput)
        {
            try
            {
                // Gọi AI để lấy phản hồi JSON string
                var jsonResponse = await _ai.GenerateScriptAsync(userInput);

                // Parse JSON AI trả về
                var jsonDoc = JsonDocument.Parse(jsonResponse);
                var root = jsonDoc.RootElement;

                var planning = new Planning
                {
                    Id = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(), // TODO: lấy từ token nếu có
                    Name = root.GetProperty("eventName").GetString(),
                    DateOfEvent = DateTime.ParseExact(root.GetProperty("eventDate").GetString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Location = root.GetProperty("location").GetString(),
                    AboutNumberPeople = root.GetProperty("expectedParticipants").GetString(),
                    MainColor = string.Join(", ", root.GetProperty("themeColor").EnumerateArray().Select(x => x.GetString())),
                    Budget = ParseBudget(root.GetProperty("budget").GetString()),
                    Description = root.GetProperty("description").GetString(),
                    TypeOfEvent = root.GetProperty("eventType").GetString(),
                    GeneratedScript = CleanGeneratedScript(jsonResponse), // lưu cả phản hồi thô nếu cần
                    CreateAt = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                await _repo.AddAsync(planning);

                return Ok(new ApiResponse<PlanningResponseDto>
                {
                    Success = true,
                    Data = new PlanningResponseDto
                    {
                        Name = planning.Name,
                        Description = planning.Description,
                        Location = planning.Location,
                        DateOfEvent = planning.DateOfEvent,
                        Budget = planning.Budget,
                        AboutNumberPeople = planning.AboutNumberPeople,
                        MainColor = planning.MainColor,
                        TypeOfEvent = planning.TypeOfEvent,
                        GeneratedScript = planning.GeneratedScript
                    },
                    Message = "Tạo kế hoạch thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Lỗi khi sinh kế hoạch từ AI: " + ex.Message
                });
            }
        }

        private string CleanGeneratedScript(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "";

            // 1. Bỏ ```json và ```
            var clean = Regex.Replace(raw, @"```json|```", "", RegexOptions.IgnoreCase);

            // 2. Bỏ dấu sao * (Markdown format)
            clean = clean.Replace("*", "");

            // 3. Thay \n bằng xuống dòng thật hoặc <br>
            clean = clean.Replace("\\n", Environment.NewLine); // hoặc "<br>" nếu frontend dùng HTML

            // 4. Trim trắng đầu cuối
            return clean.Trim();
        }

        private decimal ParseBudget(string? rawBudget)
        {
            if (string.IsNullOrWhiteSpace(rawBudget)) return 0;

            // Loại bỏ ký tự không phải số hoặc dấu phẩy
            var cleaned = new string(rawBudget
                .Where(c => char.IsDigit(c) || c == ',' || c == '.')
                .ToArray());

            // Ưu tiên dấu chấm là phân cách hàng nghìn -> loại bỏ
            cleaned = cleaned.Replace(".", "").Replace(",", ".");

            return decimal.TryParse(cleaned, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var result)
                ? result
                : 0;
        }


    }
}
