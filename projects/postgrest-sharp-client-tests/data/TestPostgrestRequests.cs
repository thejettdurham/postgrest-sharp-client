using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Postgrest.Client.Tests.Unit.data
{
    class TestPostgrestRequests
    {
        internal const string TestRoute = "route";
        internal const string TestColumn = "column";
        internal const string TestRowFilterCondition = "foo";
        internal const string TestRowFilterExpression = "eq." + TestRowFilterCondition;
        internal const string TestModelCsvRepresentation = "csv";
        internal const string TestModelDefaultName = "user";

        class TestPostgrestModel : PostgrestModel
        {
            public override string Csv => TestModelCsvRepresentation;

            public string Name { get; set; } = TestModelDefaultName;
        }

        internal static Dictionary<string, string> TestProcedureArgs = new Dictionary<string, string>
        {
            {"arg1", "val1"}
        };

        internal static List<PostgrestFilter> TestRowFilters => new List<PostgrestFilter>
        {
            new PostgrestFilter(TestColumn, PostgrestFilterOperation.EqualTo, TestRowFilterCondition)
        };

        internal static PostgrestRequest BaseRead => new PostgrestRequest(TestRoute, PostgrestRequestType.Read);
        internal static PostgrestRequest BaseCreate => new PostgrestRequest(TestRoute, PostgrestRequestType.Create);
        internal static PostgrestRequest BaseUpdate => new PostgrestRequest(TestRoute, PostgrestRequestType.Update);
        internal static PostgrestRequest BaseDelete => new PostgrestRequest(TestRoute, PostgrestRequestType.Delete);
        internal static PostgrestRequest BaseSchema => new PostgrestRequest(TestRoute, PostgrestRequestType.Schema);
        internal static PostgrestRequest BaseProcedure => new PostgrestRequest(TestRoute, PostgrestRequestType.Procedure);

        internal static PostgrestRequest ProcedureWithArgs => new PostgrestRequest(BaseProcedure) 
        {
            ProcedureArgs = TestProcedureArgs
        };

        internal static PostgrestRequest UpdateWithEmptyRowFilters => new PostgrestRequest(BaseUpdate)
        {
            RowFilters = new List<PostgrestFilter>()
        };

        internal static PostgrestRequest UpdateWithRowFilters => new PostgrestRequest(BaseUpdate)
        {
            RowFilters = TestRowFilters
        };

        internal static PostgrestRequest CreateWithData => new PostgrestRequest(BaseCreate)
        {
            Data = new TestPostgrestModel()
        };

        internal static PostgrestRequest CreateWithDataAndRowFilters => new PostgrestRequest(CreateWithData)
        {
            RowFilters = TestRowFilters
        };

        internal static PostgrestRequest UpdateWithRowFiltersAndData => new PostgrestRequest(UpdateWithRowFilters)
        {
            Data = new TestPostgrestModel()
        };

        internal static PostgrestRequest ReadWithRowFilters => new PostgrestRequest(BaseRead)
        {
            RowFilters = TestRowFilters
        };

        internal static PostgrestRequest DeleteWithRowFilters => new PostgrestRequest(BaseDelete)
        {
            RowFilters = TestRowFilters
        };
    }
}
