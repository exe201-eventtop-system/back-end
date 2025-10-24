using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs.AI
{
    public class PlanningAIResponseDTO
    {
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public string Location { get; set; }
        public string ExpectedParticipants { get; set; }
        [JsonConverter(typeof(StringOrArrayConverter))]
        public List<string> ThemeColor { get; set; }
        public string Budget { get; set; }
        public string Description { get; set; }
        public string EventType { get; set; }
    }
    public class StringOrArrayConverter : JsonConverter<List<string>>
    {
        public override List<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = new List<string>();

            if (reader.TokenType == JsonTokenType.String)
            {
                result.Add(reader.GetString());
            }
            else if (reader.TokenType == JsonTokenType.StartArray)
            {
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    result.Add(reader.GetString());
                }
            }
            else
            {
                throw new JsonException("Expected string or array for themeColor.");
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var s in value)
            {
                writer.WriteStringValue(s);
            }
            writer.WriteEndArray();
        }
    }

}
