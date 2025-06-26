using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class GeminiSettings
    {
        public string ApiKey { get; set; }
        public string Model { get; set; }
        public string BaseUrl { get; set; }

        public string FullUrl => $"{BaseUrl}/models/{Model}:generateContent?key={ApiKey}";
    }
}
