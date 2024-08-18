using Newtonsoft.Json;

namespace DBL.Tools
{
    public static class CloneObjectHelper
    {
        public static T Clone<T>(this T source)
        {
            if (source == null)
            {
                return default(T);
            }

            var serializedObject = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }
    }
}
