using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postgrest.Client.Tests.Unit.data
{
    class TestPostgrestRequests
    {
        internal const string TestRoute = "route";

        internal PostgrestRequest BaseRead => new PostgrestRequest(TestRoute, PostgrestRequestType.Read);
        internal PostgrestRequest BaseCreate => new PostgrestRequest(TestRoute, PostgrestRequestType.Create);
        internal PostgrestRequest BaseUpdate => new PostgrestRequest(TestRoute, PostgrestRequestType.Update);
        internal PostgrestRequest BaseDelete => new PostgrestRequest(TestRoute, PostgrestRequestType.Delete);
        internal PostgrestRequest BaseSchema => new PostgrestRequest(TestRoute, PostgrestRequestType.Schema);
        internal PostgrestRequest BaseProcedure => new PostgrestRequest(TestRoute, PostgrestRequestType.Procedure);

        internal PostgrestRequest ProcedureWithArgs => new PostgrestRequest(BaseProcedure) 
        {
            ProcedureArgs = new Dictionary<string, string>
            {
                {"arg1", "val1"}
            }
        };
    }
}
