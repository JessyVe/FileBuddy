using Newtonsoft.Json;

namespace SharedResources.Services
{
    public static class JsonConverter
    {
        /// <summary>
        /// Returns a json string representing the given object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetAsJson(object obj, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }

        /// <summary>
        /// Returns an object which is represented by the given json string. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T GetObjectFromJson<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
