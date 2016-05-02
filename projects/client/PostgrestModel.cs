using Newtonsoft.Json;

namespace Postgrest.Client
{
    public abstract class PostgrestModel
    {
        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetMinimalJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
