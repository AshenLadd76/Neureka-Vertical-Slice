using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Logger = ToolBox.Utils.Logger;



namespace ToolBox.Helpers
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize<T>(T obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Serialization failed: {ex.Message}");
                return "{}"; 
            }
        }

        public T Deserialize<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Deserialize failed: {ex.Message}");
                return default;
            }
        }

        public (bool IsValid, List<string> Errors) ValidateJson(string json, string schemaJson = null)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(json))
            {
                errors.Add("JSON string is null or empty.");
                return (false, errors);
            }

            try
            {
                // Try parsing the JSON
                JToken.Parse(json);
                return (true, errors);
            }
            catch (JsonReaderException ex)
            {
                errors.Add($"Invalid JSON: {ex.Message}");
                return (false, errors);
            }
            catch (Exception ex)
            {
                errors.Add($"Unexpected error: {ex.Message}");
                return (false, errors);
            }
        }
    }
}