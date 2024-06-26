using Newtonsoft.Json;
using System.Text.Json;

namespace ThanhBuoi.Services
{
    public static class SessionService
    {
        public static JsonDocument ConvertStringToJson(string jsonString)
        {
            JsonDocument jsonDoc = JsonDocument.Parse(jsonString);
            return jsonDoc;
        }

       /* public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
                   T obj = JsonSerializer.Deserialize<T>(jsonData);

        }*/
    }
}
