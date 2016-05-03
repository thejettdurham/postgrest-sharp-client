using System.Data;
using System.Linq;
using Newtonsoft.Json;

namespace Postgrest.Client
{
    public abstract class PostgrestModel
    {
        public string Json => JsonConvert.SerializeObject(this);

        public string MinimalJson => JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
    }
}