using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Postgrest.Client.Tests.Unit.data
{
    class TestPostgrestModels
    {
        internal const string TestModelToCsvRepresentation = "csv";
        internal const string TestModelDefaultName = "user";

        public class User : PostgrestModel
        {

            public string Name { get; set; } = TestModelDefaultName;
            public string Password { get; set; }
            public override string ToCsv()
            {
                throw new NotImplementedException();
            }
        }

        public class UserOverrideContract : User
        {
            protected override IContractResolver ModelContractResolver => new DefaultContractResolver();

            //protected override JsonSerializerSettings JsonSerializationSettings => new JsonSerializerSettings
            //{
            //    ContractResolver = new DefaultContractResolver()
            //};
        }
    }
}
