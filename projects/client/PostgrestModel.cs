using Newtonsoft.Json;

namespace Postgrest.Client
{
    public abstract class PostgrestModel
    {
        /// <summary>
        /// Gets a full JSON representation of the object (nulls included)
        /// </summary>
        public string Json => JsonConvert.SerializeObject(this);

        /// <summary>
        /// Gets a JSON representation of the object with nulls stripped out.
        /// </summary>
        public string MinimalJson => JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        /// <summary>
        /// Gets a CSV representation of the object
        /// </summary>
        public abstract string Csv { get; }
    }
}