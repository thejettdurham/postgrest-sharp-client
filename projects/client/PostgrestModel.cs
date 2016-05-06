using Newtonsoft.Json;

namespace Postgrest.Client
{
    /// <summary>
    /// A class to build Postgrest Models upon. This base class provides behavior needed by the library to know how to handle the data when its being sent to the API server.
    /// </summary>
    public abstract class PostgrestModel
    {
        /// <summary>
        /// Gets a full JSON representation of the object (nulls included)
        /// </summary>
        [JsonIgnore]
        public string Json => JsonConvert.SerializeObject(this);

        /// <summary>
        /// Gets a JSON representation of the object with nulls stripped out.
        /// </summary>
        [JsonIgnore]
        public string MinimalJson => JsonConvert.SerializeObject(this, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        /// <summary>
        /// Gets a CSV representation of the object
        /// </summary>
        [JsonIgnore]
        public abstract string Csv { get; }
    }
}