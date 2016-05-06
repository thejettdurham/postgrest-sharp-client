using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace Postgrest.Client.Sample
{
    public class SamplePostgrestModel : PostgrestModel
    {
        public override string ToCsv()
        {
            throw new NotImplementedException();
        }
    }
}
