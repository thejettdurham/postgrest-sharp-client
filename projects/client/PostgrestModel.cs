using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Postgrest.Client
{
    /// <summary>
    /// A class to build Postgrest Models upon. This base class provides behavior needed by the library to know how to handle the data when its being sent to the API server.
    /// </summary>
    public abstract class PostgrestModel
    {
        protected virtual IContractResolver ModelContractResolver => new PostgrestContractResolver();

        protected virtual JsonSerializerSettings JsonSerializationSettings => new JsonSerializerSettings
        {
            ContractResolver = ModelContractResolver
        };

        protected virtual JsonSerializerSettings MinimalJsonSerializationSettings => new JsonSerializerSettings
        {
            ContractResolver = ModelContractResolver,
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// Gets a full JSON representation of the object (nulls included)
        /// </summary>
        public virtual string ToJson()
        {
           return JsonConvert.SerializeObject(this, JsonSerializationSettings);
        }

        /// <summary>
        /// Gets a JSON representation of the object with nulls stripped out.
        /// </summary>
        public virtual string ToMinimalJson()
        {
            return JsonConvert.SerializeObject(this, MinimalJsonSerializationSettings);
        }

        /// <summary>
        /// Gets a CSV representation of the object
        /// </summary>
        public abstract string ToCsv();
    }

    public class PostgrestContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}